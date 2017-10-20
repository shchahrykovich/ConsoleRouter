using ConsoleRouter.Services;
using System.IO;

namespace ConsoleRouter.Controllers
{
    internal class HelpController
    {
        private TextWriter _output;
        private DefaultHelpString _helpString;

        public HelpController(TextWriter output, DefaultHelpString helpString)
        {
            _output = output;
            _helpString = helpString;
        }

        public int Help()
        {
            _output.WriteLine(_helpString.Message);
            return 0;
        }
    }
}
