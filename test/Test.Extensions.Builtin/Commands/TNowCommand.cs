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
    public class TNowCommand
    {
        [TestMethod]
        public async Task File()
        {
            Logger logger = new Logger();
            TestWorkspace workspace = new TestWorkspace(logger.CreateScope("main", LogLevel.Debug),
                onCreate: (name, tem, callback) => Task.FromResult<IWorkItem?>(new TestWorkItem("")));

            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(
                new NowCommand().Build(),
                new string[] { "now", "--", "type=f", "target=a.c" },
                workspace: workspace);

            logger.AssertInvoked(nameof(IWorkspace.Create));
            Assert.IsTrue(result.IsOk);
            Assert.AreEqual<int>(0, result.Result!);
        }

        [TestMethod]
        public async Task Directory()
        {
            Logger logger = new Logger();
            TestWorkspace workspace = new TestWorkspace(logger.CreateScope("main", LogLevel.Debug),
                onCreate: (name, tem, callback) => Task.FromResult<IWorkItem?>(new TestWorkItem("")));

            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(
                new NowCommand().Build(),
                new string[] { "now", "--", "type=d", "target=a" },
                workspace: workspace);

            logger.AssertInvoked(nameof(IWorkspace.Create));

            ResultAssert.OkWithZero(result);
        }
    }
}
