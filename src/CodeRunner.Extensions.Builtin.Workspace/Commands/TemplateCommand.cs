using CodeRunner.Commands;
using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
    public class TemplateCommand : BaseCommand<TemplateCommand.CArgument>
    {
        public override string Name => "template";

        public override Command Configure()
        {
            Command res = new Command("template", "Manage templates.");
            res.Commands.Add(new Templates.ListCommand().Build());
            res.Commands.Add(new Templates.AddCommand().Build());
            res.Commands.Add(new Templates.RemoveCommand().Build());
            return res;
        }

        public override Task<int> Handle(CArgument argument, ParserContext parser, PipelineContext operation, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
