using CodeRunner.Extensions.Builtin.Workspace.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Resources.Programming;
using CodeRunner.Test;
using CodeRunner.Test.Commands;
using CodeRunner.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TNewCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            Logger logger = new Logger();
            TestWorkspace workspace = new TestWorkspace(logger.CreateScope("main", LogLevel.Debug));
            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(
                new NewCommand().Build(),
                new string[] { "new", "c", "a" },
                workspace: workspace,
                before: async context =>
                {
                    _ = await PipelineGenerator.InitializeWorkspace(context);
                    await context.Services.GetWorkspace().Templates.SetValue("c", FileTemplates.C);
                });

            logger.AssertInvoked(nameof(IWorkspace.Create));
            ResultAssert.OkWithZero(result);
        }
    }
}
