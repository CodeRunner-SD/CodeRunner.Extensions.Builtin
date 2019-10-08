using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Terminals;
using CodeRunner.Extensions.Terminals.Rendering;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packaging;
using CodeRunner.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Operations
{
    public class ListCommand : ItemManagers.ListCommand<IOperationManager, OperationSettings, Package<IOperation>>
    {
        public override string Name => "operation.list";

        public override Task<IOperationManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Operations);
        }

        public override async Task RenderItems(ITerminal terminal, IAsyncEnumerable<(string, Package<IOperation>?)> items, PipelineContext pipeline)
        {
            List<(string, Package<IOperation>?)> sources = new List<(string, Package<IOperation>?)>();
            await foreach ((string, Package<IOperation>?) v in items)
                sources.Add(v);
            terminal.Output.WriteTable(sources,
                new OutputTableColumnStringView<(string, Package<IOperation>?)>(x => x.Item1, "Name"),
                new OutputTableColumnStringView<(string, Package<IOperation>?)>(x => x.Item2?.Metadata?.Name ?? "N/A", nameof(PackageMetadata.Name)),
                new OutputTableColumnStringView<(string, Package<IOperation>?)>(x => x.Item2?.Metadata?.Author ?? "N/A", nameof(PackageMetadata.Author)),
                new OutputTableColumnStringView<(string, Package<IOperation>?)>(x => x.Item2?.Metadata?.CreationTime.ToString() ?? "N/A", nameof(PackageMetadata.CreationTime)),
                new OutputTableColumnStringView<(string, Package<IOperation>?)>(x => x.Item2?.Metadata?.Version.ToString() ?? "N/A", nameof(PackageMetadata.Version))
            );
        }
    }
}
