using CodeRunner.Extensions.Managements;
using CodeRunner.Managements;
using CodeRunner.Templates;
using System;
using System.Threading.Tasks;

namespace CodeRunner.Extensions.Builtin.Workspace.Managements
{
    [Export]
    public class FSWorkspace : IWorkspaceProvider
    {
        public string Name => "fs";

        public ITemplate<IWorkspace> GetProvider() => new FunctionBasedTemplate<IWorkspace>(context =>
        {
            CodeRunner.Managements.FSBased.Workspace res = new CodeRunner.Managements.FSBased.Workspace(new System.IO.DirectoryInfo(Environment.CurrentDirectory));
            return Task.FromResult<IWorkspace>(res);
        });
    }
}
