using ConsoleRouter.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouter
{
    internal class ControllerActivator
    {
        private Dictionary<Type, Func<Object>> _services;

        internal ControllerActivator(Dictionary<Type, Func<Object>> services)
        {
            _services = services;
        }

        internal object Create(Match route)
        {
            var parameters = GetParameters(route);
            return Activator.CreateInstance(route.Type, parameters);
        }

        private Object[] GetParameters(Match route)
        {
            Object[] result = null;

            var ctor = route.Type.GetConstructors().First();
            var ctorArguments = ctor.GetParameters().ToArray();
            if (ctorArguments.Any())
            {
                List<Object> parameters = new List<Object>();

                foreach (var a in ctorArguments)
                {
                    if (_services.ContainsKey(a.ParameterType))
                    {
                        parameters.Add(_services[a.ParameterType]());
                    }
                    else
                    {
                        throw new Exception("Can't create controller " + route.Type.Name);
                    }
                }

                result = parameters.ToArray();
            }

            return result;
        }
    }
}