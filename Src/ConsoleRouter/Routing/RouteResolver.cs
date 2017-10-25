using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouter.Routing
{
    internal class RouteResolver
    {
        public static Route Resolve(IEnumerable<Type> controllers, Dictionary<String, String> routeData)
        {
            Type controller = null;
            if (routeData.ContainsKey("controller"))
            {
                var controllerName = routeData["controller"] + "Controller";
                controller = controllers.FirstOrDefault(c => 0 == String.Compare(controllerName, c.Name, StringComparison.OrdinalIgnoreCase));
            }

            MethodInfo method = null;
            if (null != controller && routeData.ContainsKey("action"))
            {
                var methodName = routeData["action"];
                method = controller.GetMethods().FirstOrDefault(m => m.IsPublic &&
                                                                !m.IsAbstract &&
                                                                !m.IsStatic &&
                                                                0 == String.Compare(methodName, m.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (null != controller && null != method)
            {
                List<Object> parameters = new List<Object>();

                int paramIndex = 0;
                bool paramsMatch = true;
                foreach (var p in method.GetParameters())
                {
                    var name = p.Name;
                    if (!routeData.ContainsKey(name))
                    {
                        name = $"${paramIndex}";
                    }
                    if (routeData.ContainsKey(name))
                    {
                        var paramValue = routeData[name];
                        if (p.ParameterType == typeof(string))
                        {
                            parameters.Add(paramValue);
                        }
                        else if (p.ParameterType == typeof(int))
                        {
                            parameters.Add(Int32.Parse(paramValue));
                        }
                        else
                        {
                            paramsMatch = false;
                            break;
                        }
                    }
                    else if (p.HasDefaultValue)
                    {
                        parameters.Add(p.DefaultValue);
                    }
                    else
                    {
                        paramsMatch = false;
                        break;
                    }

                    paramIndex++;
                }

                if (paramsMatch)
                {
                    return new Route(controller, method, parameters.ToArray());
                }
            }

            return null;
        }
    }
}
