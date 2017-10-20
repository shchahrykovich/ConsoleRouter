using System.IO;
using System.Threading;

namespace TestApp.Controllers
{
    public class ActionController
    {
        private TextWriter _output;
        private CancellationToken _token;

        public ActionController(TextWriter output, CancellationToken token)
        {
            _output = output;
            _token = token;
        }

        public void Help()
        {
            _output.WriteLine("This is help for action controller.");
        }

        public void Hi(string name)
        {
            _output.WriteLine($"Hi {name}!");
        }

        public void DoLongAction()
        {
            _output.WriteLine("Long action has been started. Press Ctrl-C to finish.");
            WaitHandle.WaitAll(new WaitHandle[] { _token.WaitHandle });
            _output.WriteLine("Long action has been finished.");
        }
    }
}