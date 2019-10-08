using CodeRunner.Commands;
using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Terminals;
using CodeRunner.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Console.Commands
{
    [Export]
    public class ClearCommand : BaseCommand<ClearCommand.CArgument>
    {
        public override string Name => "clear";

        public override Command Configure()
        {
            Command res = new Command("clear", "Clear screen.");
            return res;
        }

        public override Task<int> Handle(CArgument argument, ParserContext parser, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            ITerminal terminal = pipeline.Services.GetTerminal();
            terminal.Clear();
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
