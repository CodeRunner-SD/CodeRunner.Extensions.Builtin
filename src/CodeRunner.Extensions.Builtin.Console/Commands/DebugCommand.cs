using CodeRunner.Commands;
using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Terminals;
using CodeRunner.Extensions.Terminals.Rendering;
using CodeRunner.Loggings;
using CodeRunner.Pipelines;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Console.Commands
{
    internal class OutputTableColumnLogLevelView : OutputTableColumnStringView<LogItem>
    {
        public OutputTableColumnLogLevelView(string header) : base(x => x.Level.ToString(), header)
        {
        }

        public override void Render(ITerminal terminal, LogItem value, int length)
        {
            string levelStr = GetValue(value).PadRight(length);
            switch (value.Level)
            {
                case LogLevel.Information:
                    terminal.Output.WriteInformation(levelStr);
                    break;
                case LogLevel.Warning:
                    terminal.Output.WriteWarning(levelStr);
                    break;
                case LogLevel.Error:
                    terminal.Output.WriteError(levelStr);
                    break;
                case LogLevel.Fatal:
                    terminal.Output.WriteFatal(levelStr);
                    break;
                case LogLevel.Debug:
                    terminal.Output.WriteDebug(levelStr);
                    break;
            }
        }
    }

    [Export]
    public class DebugCommand : BaseCommand<DebugCommand.CArgument>
    {
        public override string Name => "debug";

        public override Command Configure()
        {
            Command res = new Command("debug", "Get information for debug.");
            return res;
        }

        public override Task<int> Handle(CArgument argument, ParserContext parser, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            ILogger logger = pipeline.Services.GetLogger();
            ITerminal terminal = pipeline.Services.GetTerminal();
            {
                terminal.Output.WriteTable(logger.View(),
                    new OutputTableColumnLogLevelView(nameof(LogItem.Level)),
                    new OutputTableColumnStringView<LogItem>(x => x.Scope, nameof(LogItem.Scope)),
                    new OutputTableColumnStringView<LogItem>(x => x.Time.ToString(CultureInfo.InvariantCulture.DateTimeFormat), nameof(LogItem.Time)),
                    new OutputTableColumnStringView<LogItem>(x => x.Content, nameof(LogItem.Content))
                );
            }
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
