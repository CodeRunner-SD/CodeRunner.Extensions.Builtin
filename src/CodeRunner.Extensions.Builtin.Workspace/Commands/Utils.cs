using CodeRunner.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Extensions.Terminals;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Commands
{
    internal static class Utils
    {
        public static Task ResolveCallback(VariableCollection variables, ResolveContext resolveContext, ParserContext parser, PipelineContext pipeline)
        {
            _ = resolveContext.FromArgumentList(parser.UnparsedTokens);
            ITerminal terminal = pipeline.Services.GetTerminal();

            if (!terminal.FillVariables(variables, resolveContext))
                throw new ArgumentException();
            return Task.CompletedTask;
        }
    }
}
