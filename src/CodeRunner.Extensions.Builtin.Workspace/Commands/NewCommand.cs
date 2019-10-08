using CodeRunner.Commands;
using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Terminals;
using CodeRunner.Managements;
using CodeRunner.Packaging;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    [Export]
    public class NewCommand : BaseCommand<NewCommand.CArgument>
    {
        public override string Name => "new";

        public override Command Configure()
        {
            Command res = new Command("new", "Create new item from template.");
            {
                Argument<string> argTemplate = new Argument<string>(nameof(CArgument.Template))
                {
                    Arity = ArgumentArity.ExactlyOne,
                };
                res.Arguments.Add(argTemplate);
            }
            {
                Argument<string> argTemplate = new Argument<string>(nameof(CArgument.Name))
                {
                    Arity = ArgumentArity.ExactlyOne,
                };
                res.Arguments.Add(argTemplate);
            }
            return res;
        }

        public override async Task<int> Handle(CArgument argument, ParserContext parser, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            IWorkspace workspace = pipeline.Services.GetWorkspace();
            ITerminal terminal = pipeline.Services.GetTerminal();
            string template = argument.Template;
            Package<ITemplate>? tplItem = await workspace.Templates.GetValue(template);
            if (tplItem == null)
            {
                terminal.Output.WriteErrorLine($"No this template: {template}.");
                return 1;
            }
            ITemplate? tpl = tplItem.Data;
            if (tpl == null)
            {
                terminal.Output.WriteErrorLine($"Can not load this template: {template}.");
                return 1;
            }

            IWorkItem? item = null;
            try
            {
                item = await workspace.Create(argument.Name, tpl,
                    (vars, resolveContext) => Utils.ResolveCallback(vars, resolveContext, parser, pipeline));
            }
            catch (ArgumentException)
            {
                return -1;
            }
            return 0;
        }

        public class CArgument
        {
            public string Template { get; set; } = "";

            public string Name { get; set; } = "";
        }
    }
}
