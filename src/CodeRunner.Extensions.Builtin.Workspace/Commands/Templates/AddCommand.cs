using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packaging;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Templates
{
    public class AddCommand : ItemManagers.AddCommand<ITemplateManager, TemplateSettings, Package<ITemplate>>
    {
        public override string Name => "template.add";

        public override async Task<Package<ITemplate>> GetItem(FileInfo file)
        {
            using FileStream st = file.OpenRead();
            return await Package.Load<ITemplate>(st);
        }

        public override Task<ITemplateManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Templates);
        }
    }
}
