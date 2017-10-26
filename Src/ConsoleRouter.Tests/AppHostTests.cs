using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace ConsoleRouter.Tests
{
    public class AppHostTests
    {
        private AppHost _host;
        private StringBuilder _output;
        private StringWriter _writer;

        public AppHostTests()
        {
            _output = new StringBuilder();
            _writer = new StringWriter(_output);
            var services = new Dictionary<Type, Func<Object>>();
            services.Add(typeof(TextWriter), () => _writer);

            _host = new AppHost(new[] { Assembly.GetExecutingAssembly() }, services);
        }
        
        [Theory]
        [InlineData("action do", "Action", "Do")]
        [InlineData("Action Do", "Action", "Do")]
        [InlineData("Do It", "Do", "It")]
        [InlineData("do it", "Do", "It")]
        public void Should_Call_Correct_Method(string commandLine, string expectedControllerName, string expectedMethodName)
        {
            // Arrange
            var args = commandLine.Split(" ");
            _host.RegisterRoute("{controller} {action}");

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal($"{expectedControllerName}Controller -> {expectedMethodName}", _output.ToString().TrimEnd());
        }

        [Theory]
        [InlineData("action do", "")]
        [InlineData("", "{controller} {action}")]
        [InlineData("action1 do", "{controller} {action}")]
        [InlineData("action do1", "{controller} {action}")]
        public void Should_Return_Default_Controller(string commandLine, string route)
        {
            // Arrange
            var args = commandLine.Split(" ");
            if (!String.IsNullOrWhiteSpace(route))
            {
                _host.RegisterRoute(route);
            }

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal("Can't find route, please check arguments.", _output.ToString().TrimEnd());
        }

        [Theory]
        [InlineData("", "HelpController -> Help")]
        [InlineData("help", "HelpController -> Help")]
        [InlineData("action", "ActionController -> Help")]
        public void Should_Call_Default_Method(string commandLine, string expectedResult)
        {
            // Arrange
            var args = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _host.RegisterRoute("{controller=help} {action=help}");

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal(expectedResult, _output.ToString().TrimEnd());
        }
        
        [Theory]
        [InlineData("action hi sergey 2", "Hi sergey!\r\nHi sergey!")]
        public void Should_Pass_Method_Params_By_Default(string commandLine, string expectedResult)
        {
            // Arrange
            var args = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _host.RegisterRoute("{controller=help} {action=help}");

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal(expectedResult, _output.ToString().TrimEnd());
        }

        [Theory]
        [InlineData("hello sergey 2", "Hi sergey!\r\nHi sergey!")]
        [InlineData("a hi sergey 1", "Hi sergey!")]
        [InlineData("d", "DoController -> It")]
        [InlineData("D", "DoController -> It")]
        public void Should_Parse_Shortcuts(string commandLine, string expectedResult)
        {
            // Arrange
            var args = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _host.RegisterRoute("hello(controller=action,action=hi)");
            _host.RegisterRoute("a(controller=action) {action}");
            _host.RegisterRoute("d(controller=do) {action=it}");
            _host.RegisterRoute("{controller=help} {action=help}");

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal(expectedResult, _output.ToString().TrimEnd());
        }

        [Theory]
        [InlineData("hello -n sergey 2", "Hi sergey!\r\nHi sergey!")]
        [InlineData("hello -n sergey -c 2", "Hi sergey!\r\nHi sergey!")]
        [InlineData("hello -N sergey -c 2", "Hi sergey!\r\nHi sergey!")]
        [InlineData("hello sergey -c 2", "Hi sergey!\r\nHi sergey!")]
        [InlineData("hello sergey -c", "Can't find route, please check arguments.")]
        [InlineData("hello sergey 2", "Hi sergey!\r\nHi sergey!")]
        public void Should_Parse_Aliases(string commandLine, string expectedResult)
        {
            // Arrange
            var args = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _host.RegisterRoute("hello(controller=action,action=hi) -n{name} -c{count}");

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal(expectedResult, _output.ToString().TrimEnd());
        }

        [Theory]
        [InlineData("do task -name report --subTask xml", "report - xml")]
        [InlineData("do task report --subTask xml", "report - xml")]
        [InlineData("do task report /subTask xml", "report - xml")]
        [InlineData("do task report /subTask", "report - a")]
        public void Should_Process_Undefined_Arguments(String commandLine, string expectedResult)
        {
            // Arrange
            var args = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _host.RegisterRoute("{controller} {action}");

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal(expectedResult, _output.ToString().TrimEnd());
        }

        [Theory]
        [InlineData("do task report xml", "report - xml")]
        [InlineData("do task report", "report - a")]
        [InlineData("do bigtask report b b", "report - b - b")]
        [InlineData("do bigtask report b", "report - b - a")]
        [InlineData("do bigtask report", "report - a - a")]
        public void Should_Process_Default_Method_Arguments(String commandLine, string expectedResult)
        {
            // Arrange
            var args = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _host.RegisterRoute("{controller} {action}");

            // Act
            _host.Run(args);

            // Assert
            Assert.Equal(expectedResult, _output.ToString().TrimEnd());
        }
    }
}
