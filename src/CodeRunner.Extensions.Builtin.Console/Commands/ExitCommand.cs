using CodeRunner.Commands;
using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Console.Commands
{
    [Export]
    public class ExitCommand : BaseCommand<ExitCommand.CArgument>
    {
        public override string Name => "exit";

        public override Command Configure()
        {
            Command res = new Command("exit", "Exit CodeRunner.");
            return res;
        }

        public override Task<int> Handle(CArgument argument, ParserContext parser, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            IHost host = pipeline.Services.GetService<IHost>();
            host.Shutdown();
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
