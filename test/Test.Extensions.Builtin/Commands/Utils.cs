using CodeRunner.Extensions.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Threading.Tasks;
using Builder = CodeRunner.Pipelines.PipelineBuilder<string[], CodeRunner.Pipelines.Wrapper<int>>;

namespace Test.Extensions.Builtin
{
    public static class Utils
    {
        public static readonly PipelineOperation<string[], Wrapper<int>> InitializeWorkspace = async context =>
        {
            IWorkspace workspace = context.Services.GetWorkspace();
            await workspace.Initialize();
            return 0;
        };

        public static Parser CreateDefaultParser(Command command, PipelineContext context) => CreateParserBuilder(command, context)
            .UseDefaults()
            .Build();

        public static CommandLineBuilder CreateParserBuilder(Command command, PipelineContext context) => new CommandLineBuilder(command)
            .UseMiddleware(inv => inv.BindingContext.AddService(typeof(PipelineContext), () => context));

        public static Builder ConfigureConsole(this Builder builder, IConsole console, TextReader input) => builder.Configure(nameof(ConfigureConsole),
            scope =>
            {
                scope.Add<IConsole>(console);
                scope.Add<TextReader>(input);
            });

        public static Builder ConfigureWorkspace(this Builder builder, IWorkspace workspace) => builder.Configure(nameof(ConfigureWorkspace),
            scope => scope.Add<IWorkspace>(workspace));

        public static Builder ConfigureLogger(this Builder builder, ILogger logger) => builder.Configure(nameof(ConfigureLogger),
            scope => scope.Add<ILogger>(logger));

        public static async Task<PipelineResult<Wrapper<int>>> UseSampleCommandInvoker(IWorkspace workspace, Command command, string[] origin, string input = "", PipelineOperation<string[], Wrapper<int>>? before = null, PipelineOperation<string[], Wrapper<int>>? after = null)
        {
            using MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input));
            using StreamReader sr = new StreamReader(ms);
            PipelineBuilder<string[], Wrapper<int>> builder = CreatePipelineBuilder(new TestTerminal(), sr, workspace);
            if (before != null)
            {
                _ = builder.Use("before", async context =>
                  {
                      _ = await before(context);
                      return context.IgnoreResult();
                  });
            }
            _ = builder.Use("main", async context =>
              {
                  Parser parser = CreateDefaultParser(command, context);
                  return await parser.InvokeAsync(context.Origin, context.Services.GetConsole());
              });
            if (after != null)
            {
                _ = builder.Use("after", async context =>
                  {
                      _ = await after(context);
                      return context.IgnoreResult();
                  });
            }
            return await ConsumePipelineBuilder(builder, new Logger(), origin);
        }

        public static PipelineBuilder<string[], Wrapper<int>> CreatePipelineBuilder(IConsole console, TextReader input, IWorkspace? workspace)
        {
            PipelineBuilder<string[], Wrapper<int>> builder = new PipelineBuilder<string[], Wrapper<int>>()
                .ConfigureConsole(console, input);
            if (workspace != null)
            {
                _ = builder.ConfigureWorkspace(workspace);
            }

            return builder;
        }

        public static async Task<PipelineResult<Wrapper<int>>> ConsumePipelineBuilder(PipelineBuilder<string[], Wrapper<int>> builder, Logger logger, string[] origin)
        {
            Pipeline<string[], Wrapper<int>> pipeline = await builder
                .ConfigureLogger(logger)
                .Build(origin, logger);
            return await pipeline.Consume();
        }
    }
}
