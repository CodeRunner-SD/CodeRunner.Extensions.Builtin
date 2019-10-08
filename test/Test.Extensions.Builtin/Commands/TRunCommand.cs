using CodeRunner.Extensions.Builtin.Workspace.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Test;
using CodeRunner.Test.Commands;
using CodeRunner.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TRunCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            Logger logger = new Logger();
            TestWorkspace workspace = new TestWorkspace(logger.CreateScope("main", LogLevel.Debug),
                onExecute: (item, op, call, watcher, logger) => Task.FromResult(new PipelineResult<Wrapper<bool>>(true, null, Array.Empty<LogItem>())));
            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(
                new RunCommand().Build(),
                new string[] { "hello", "--", "name=a" },
                workspace: workspace,
                before: async context =>
                {
                    _ = await PipelineGenerator.InitializeWorkspace(context);
                    await context.Services.GetWorkspace().Operations.SetValue("hello", CodeRunner.Managements.FSBased.Templates.OperationsSpaceTemplate.Hello);
                });

            logger.AssertInvoked(nameof(IWorkspace.Execute));

            ResultAssert.OkWithZero(result);
        }
    }
}
