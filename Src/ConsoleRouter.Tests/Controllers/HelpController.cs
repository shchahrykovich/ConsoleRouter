using System.IO;

namespace ConsoleRouter.Tests.Controllers
{
    public class HelpController
    {
        private TextWriter _output;

        public HelpController(TextWriter output)
        {
            _output = output;
        }

        public void Help()
        {
            _output.WriteLine("HelpController -> Help");
        }
    }
}
