using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packaging;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Templates
{
    public class RemoveCommand : ItemManagers.RemoveCommand<ITemplateManager, TemplateSettings, Package<ITemplate>>
    {
        public override string Name => "template.remove";

        public override Task<ITemplateManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Templates);
        }
    }
}
