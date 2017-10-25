using ConsoleRouter.Controllers;
using ConsoleRouter.Templating;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleRouter.Routing
{
    internal class Router
    {
        private IEnumerable<Assembly> _assemblies;
        private List<Type> _controllers;
        private List<Template> _templates = new List<Template>();

        public Router(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
            _controllers = new List<Type>(GetControllers());
        }

        public void Add(string route)
        {
            _templates.Add(Template.Parse(route));
        }

        public Route Get(string[] args)
        {
            Route result = null;
            RouteDataExtractor extractor = new RouteDataExtractor(args);
            foreach (var template in _templates)
            {
                result = TryMatch(template, extractor);
                if (null != result)
                {
                    break;
                }
            }

            return result ?? new Route(typeof(ErrorController), typeof(ErrorController).GetMethod("ShowNotFoundError"));
        }

        private Route TryMatch(Template template, RouteDataExtractor extractor)
        {
            Dictionary<String, String> routeData = extractor.Extract(template.Tokens);
            Route result = RouteResolver.Resolve(_controllers, routeData);
            return result;
        }

        private IEnumerable<Type> GetControllers()
        {
            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (null != type.Namespace &&
                        type.Namespace.Contains("Controllers") &&
                        type.Name.EndsWith("Controller"))
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}