using ConsoleRouter.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace ConsoleRouter
{
    public class AppHostBuilder
    {
        private List<String> _routes = new List<String>();
        private Dictionary<Type, Func<Object>> _services;

        private CancellationTokenSource _source = new CancellationTokenSource();

        public AppHostBuilder()
        {
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                _source.Cancel();
                e.Cancel = true;
            };

            _services = new Dictionary<Type, Func<object>>
            {
                {typeof(TextWriter), () => Console.Out },
                {typeof(CancellationToken), () => _source.Token },
                {typeof(DefaultHelpString), () => new DefaultHelpString("Please specify help message via 'WithHelp' helper method.") },
            };
        }

        public AppHostBuilder WithRoute(string route)
        {
            _routes.Add(route);
            return this;
        }

        public AppHostBuilder WithHelp(string help)
        {
            _services[typeof(DefaultHelpString)] = () => new DefaultHelpString(help);
            return this;
        }

        public AppHost Build()
        {
            var assembly = Assembly.GetCallingAssembly();

            AppHost result = new AppHost(new[] { assembly, Assembly.GetExecutingAssembly() }, _services);
            foreach (var route in _routes)
            {
                result.RegisterRoute(route);
            }

            return result;
        }
    }
}