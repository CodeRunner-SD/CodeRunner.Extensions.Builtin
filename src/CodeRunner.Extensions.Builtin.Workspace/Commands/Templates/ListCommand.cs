﻿using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Terminals;
using CodeRunner.Extensions.Terminals.Rendering;
using CodeRunner.Managements;
using CodeRunner.Managements.Configurations;
using CodeRunner.Packaging;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands.Templates
{
    public class ListCommand : ItemManagers.ListCommand<ITemplateManager, TemplateSettings, Package<ITemplate>>
    {
        public override string Name => "template.list";

        public override Task<ITemplateManager> GetManager(PipelineContext pipeline)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            return Task.FromResult(workspace.Templates);
        }

        public override async Task RenderItems(ITerminal terminal, IAsyncEnumerable<(string, Package<ITemplate>?)> items, PipelineContext pipeline)
        {
            List<(string, Package<ITemplate>?)> sources = new List<(string, Package<ITemplate>?)>();
            await foreach ((string, Package<ITemplate>?) v in items)
                sources.Add(v);
            terminal.Output.WriteTable(sources,
                new OutputTableColumnStringView<(string, Package<ITemplate>?)>(x => x.Item1, "Name"),
                new OutputTableColumnStringView<(string, Package<ITemplate>?)>(x => x.Item2?.Metadata?.Name ?? "N/A", nameof(PackageMetadata.Name)),
                new OutputTableColumnStringView<(string, Package<ITemplate>?)>(x => x.Item2?.Metadata?.Author ?? "N/A", nameof(PackageMetadata.Author)),
                new OutputTableColumnStringView<(string, Package<ITemplate>?)>(x => x.Item2?.Metadata?.CreationTime.ToString() ?? "N/A", nameof(PackageMetadata.CreationTime)),
                new OutputTableColumnStringView<(string, Package<ITemplate>?)>(x => x.Item2?.Metadata?.Version.ToString() ?? "N/A", nameof(PackageMetadata.Version))
            );
        }
    }
}
