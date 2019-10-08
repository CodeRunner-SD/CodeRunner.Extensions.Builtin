using CodeRunner.Commands;
using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
    public class OperationCommand : BaseCommand<OperationCommand.CArgument>
    {
        public override string Name => "operation";

        public override Command Configure()
        {
            Command res = new Command("operation", "Manage operations.");
            res.Aliases.Add("task");
            res.Commands.Add(new Operations.ListCommand().Build());
            res.Commands.Add(new Operations.AddCommand().Build());
            res.Commands.Add(new Operations.RemoveCommand().Build());
            return res;
        }

        public override Task<int> Handle(CArgument argument, ParserContext parser, PipelineContext pipeline, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
