using System.IO;

namespace ConsoleRouter.Tests.Controllers
{
    public class ActionController
    {
        private TextWriter _output;

        public ActionController(TextWriter output)
        {
            _output = output;
        }

        public void Do()
        {
            _output.WriteLine("ActionController -> Do");
        }

        public void Help()
        {
            _output.WriteLine("ActionController -> Help");
        }

        public void Hi(string name, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _output.WriteLine($"Hi {name}!");
            }
        }
    }
}
