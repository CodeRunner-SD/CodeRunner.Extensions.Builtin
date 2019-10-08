using CodeRunner.Commands;
using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Terminals;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
    public class NowCommand : BaseCommand<NowCommand.CArgument>
    {
        public override string Name => "now";

        public override Command Configure()
        {
            Command res = new Command("now", "Set current work-item.");
            {
                Argument<bool> arg = new Argument<bool>(nameof(CArgument.Clear), "", false)
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Clear)}".ToLower(), "Clear current work-item.")
                {
                    Argument = arg
                };
                optCommand.Aliases.Add("-c");
                res.Options.Add(optCommand);
            }
            return res;
        }

        public override async Task<int> Handle(CArgument argument, ParserContext parser, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            ITerminal terminal = pipeline.Services.GetTerminal();
            if (!argument.Clear)
            {
                IWorkItem? res = await workspace.Create("", null,
                    (vars, resolveContext) => Utils.ResolveCallback(vars, resolveContext, parser, pipeline));
                if (res != null)
                {
                    pipeline.Services.Replace<IWorkItem>(res);
                }
                else
                {
                    terminal.Output.WriteErrorLine("Create work-item failed.");
                    return -1;
                }
            }
            else
            {
                pipeline.Services.Remove<IWorkItem>();
            }
            return 0;
        }

        public class CArgument
        {
            public bool Clear { get; set; }
        }
    }
}
