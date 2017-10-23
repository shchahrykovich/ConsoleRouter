using System.IO;

namespace ConsoleRouter.Controllers
{
    internal class ErrorController
    {
        private TextWriter _output;

        public ErrorController(TextWriter output)
        {
            _output = output;
        }

        public int ShowNotFoundError()
        {
            _output.WriteLine("Can't find route, please check arguments.");
            return -1;
        }
    }
}
