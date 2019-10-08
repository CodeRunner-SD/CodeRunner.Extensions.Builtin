using CodeRunner.Extensions.Builtin.Console.Commands;
using CodeRunner.Pipelines;
using CodeRunner.Test.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Extensions.Builtin
{
    [TestClass]
    public class TClearCommand
    {
        [TestMethod]
        public async Task Basic()
        {
            PipelineResult<Wrapper<int>> result = await PipelineGenerator.CreateBuilder().UseSampleCommandInvoker(
                new ClearCommand().Build(),
                new string[] { "clear" });

            ResultAssert.OkWithZero(result);
        }
    }
}
