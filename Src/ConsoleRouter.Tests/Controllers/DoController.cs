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

        public void Task(string name, string subTask = "a")
        {
            _output.WriteLine($"{name} - {subTask}");
        }

        public void BigTask(string name, string subTask = "a", string arg = "a")
        {
            _output.WriteLine($"{name} - {subTask} - {arg}");
        }
    }
}
