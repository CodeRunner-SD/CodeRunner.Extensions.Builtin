using CodeRunner.Extensions.Builtin.Console.Commands;
using CodeRunner.Pipelines;
using CodeRunner.Test.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CommandLine;
using System.Threading.Tasks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TDebugCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(
                new DebugCommand().Build(),
                new string[] { "debug" },
                after: context =>
                 {
                     Assert.IsFalse(string.IsNullOrEmpty(context.Services.GetService<IConsole>().Out.ToString()));
                     return Task.CompletedTask;
                 });

            ResultAssert.OkWithZero(result);
        }
    }
}
