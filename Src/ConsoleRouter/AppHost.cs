using ConsoleRouter.Routing;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleRouter
{
    public class AppHost
    {
        private Router _router;
        private ControllerActivator _activator;

        internal AppHost(IEnumerable<Assembly> assemblies, Dictionary<Type, Func<Object>> services)
        {
            _router = new Router(assemblies);
            _activator = new ControllerActivator(services);
        }

        internal void RegisterRoute(string route)
        {
            _router.Add(route);
        }

        public int Run(string[] args)
        {
            var route = _router.Get(args);

            var controller = _activator.Create(route);
            var rawResult = route.MethodInfo.Invoke(controller, route.Parameters);

            var result = 0;
            if(route.MethodInfo.ReturnType == typeof(int))
            {
                result = (int)rawResult;
            }
            return result;
        }
    }
}