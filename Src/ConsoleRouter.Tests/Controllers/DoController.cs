using System.IO;

namespace ConsoleRouter.Tests.Controllers
{
    public class DoController
    {
        private TextWriter _output;

        public DoController(TextWriter output)
        {
            _output = output;
        }

        public void It()
        {
            _output.WriteLine("DoController -> It");
        }
    }
}
