using CodeRunner.Extensions.Builtin.Workspace.Commands;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using CodeRunner.Test;
using CodeRunner.Test.Commands;
using CodeRunner.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TInitCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            Logger logger = new Logger();
            TestWorkspace workspace = new TestWorkspace(logger.CreateScope("main", LogLevel.Debug));
            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(new InitCommand().Build(),
                new string[] { "init" },
                workspace: workspace,
                after: context => Task.FromResult<Wrapper<int>>(0));
            logger.AssertInvoked(nameof(IWorkspace.Initialize));

            ResultAssert.OkWithZero(result);
        }

        [TestMethod]
        public async Task Delete()
        {
            Logger logger = new Logger();
            TestWorkspace workspace = new TestWorkspace(logger.CreateScope("main", LogLevel.Debug));
            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(
                new InitCommand().Build(),
                new string[] { "init", "--delete" },
                workspace: workspace,
                after: context => Task.FromResult<Wrapper<int>>(0));

            logger.AssertInvoked(nameof(IWorkspace.Clear));

            ResultAssert.OkWithZero(result);
        }
    }
}
