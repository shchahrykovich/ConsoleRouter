using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ConsoleRouter.Routing
{
    [DebuggerDisplay("{Raw}")]
    internal class Template
    {
        private Template()
        {

        }

        public string Raw { get; private set; }

        private List<Token> _tokens = new List<Token>();

        public static Template Parse(string route)
        {
            var result = new Template();
            result.Raw = route;

            var parts = route.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var g = Token.Parse(part);
                result._tokens.Add(g);
            }

            return result;
        }

        public Match Match(IEnumerable<Type> controllers, string[] args)
        {
            Dictionary<String, String> items = new Dictionary<string, string>();

            int index = 0;
            foreach (var g in _tokens)
            {
                if (index == args.Length)
                {
                    break;
                }

                if (null != g.Shortcut)
                {
                    if (0 == String.Compare(args[index], g.Shortcut, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var defaultItem in g.GetDefaults())
                        {
                            items.Add(defaultItem.Key, defaultItem.Value);
                        }
                    }
                }
                else
                {
                    if(null != g.Alias && 0 == String.Compare(args[index], g.Alias, StringComparison.OrdinalIgnoreCase))
                    {
                        index++;
                        if (index == args.Length)
                        {
                            break;
                        }
                    }
                    items.Add(g.Name, args[index]);
                }
                
                index++;
            }

            for (int i = index, j = 0; i < args.Length; i++, j++)
            {
                items.Add($"${j}", args[i]);
            }

            foreach (var g in _tokens)
            {
                if (null != g.Name && !items.ContainsKey(g.Name))
                {
                    foreach (var defaultItem in g.GetDefaults())
                    {
                        items.Add(defaultItem.Key, defaultItem.Value);
                    }
                }
            }

            Type controller = null;
            if (items.ContainsKey("controller"))
            {
                var controllerName = items["controller"] + "Controller";
                controller = controllers.FirstOrDefault(c => 0 == String.Compare(controllerName, c.Name, StringComparison.OrdinalIgnoreCase));
            }

            MethodInfo method = null;
            if (null != controller && items.ContainsKey("action"))
            {
                var methodName = items["action"];
                method = controller.GetMethods().FirstOrDefault(m => m.IsPublic &&
                                                                !m.IsAbstract &&
                                                                !m.IsStatic &&
                                                                0 == String.Compare(methodName, m.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (null != controller && null != method)
            {
                bool paramsMatch = true;

                List<Object> parameters = new List<Object>();

                int paramIndex = 0;
                foreach (var p in method.GetParameters())
                {
                    var name = p.Name;
                    if (!items.ContainsKey(name))
                    {
                        name = $"${paramIndex}";
                    }
                    if (items.ContainsKey(name))
                    {
                        var paramValue = items[name];
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
                    return new Match(controller, method, parameters.ToArray());
                }
            }

            return null;
        }
    }
}
