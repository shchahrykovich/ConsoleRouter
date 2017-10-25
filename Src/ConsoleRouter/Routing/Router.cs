using ConsoleRouter.Controllers;
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

        public Match Get(string[] args)
        {
            Match result = null;
            foreach (var template in _templates)
            {
                result = template.Match(_controllers, args);
                if (null != result)
                {
                    break;
                }
            }

            return result ?? new Match(typeof(ErrorController), typeof(ErrorController).GetMethod("ShowNotFoundError"));
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