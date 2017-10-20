using System.IO;
using System.Threading;

namespace TestApp.Controllers
{
    public class HelpController
    {
        private TextWriter _output;
        private CancellationToken _token;

        public HelpController(TextWriter output, CancellationToken token)
        {
            _output = output;
            _token = token;
        }

        public void Help()
        {
            _output.WriteLine("This is a test app for ConsoleRunner nuget package.");
        }
    }
}
