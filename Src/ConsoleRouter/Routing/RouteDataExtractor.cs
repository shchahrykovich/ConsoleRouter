﻿using System;
using System.Collections.Generic;
using ConsoleRouter.Templating;

namespace ConsoleRouter.Routing
{
    internal class RouteDataExtractor
    {
        private string[] _args;
        private int _argsCursor;

        public RouteDataExtractor(string[] args)
        {
            _args = args;
        }

        internal Dictionary<string, string> Extract(IEnumerable<Token> tokens)
        {
            ResetArgsReader();

            Dictionary<String, String> routeData = new Dictionary<string, string>();

            AddRequired(tokens, routeData);
            AddOptional(routeData);
            AddDefault(tokens, routeData);

            return routeData;
        }

        private void ResetArgsReader()
        {
            _argsCursor = 0;
        }

        private void AddDefault(IEnumerable<Token> tokens, Dictionary<string, string> routeData)
        {
            foreach (var token in tokens)
            {
                if (null != token.RouteDataName && !routeData.ContainsKey(token.RouteDataName))
                {
                    foreach (var defaultItem in token.GetDefaultRouteData())
                    {
                        routeData.Add(defaultItem.Key, defaultItem.Value);
                    }
                }
            }
        }

        private void AddOptional(Dictionary<string, string> routeData)
        {
            for (int argsIndex = _argsCursor, positionIndex = 0; argsIndex < _args.Length; argsIndex++, positionIndex++)
            {
                routeData.Add($"${positionIndex}", _args[argsIndex]);
            }
        }

        private void AddRequired(IEnumerable<Token> tokens, Dictionary<string, string> routeData)
        {
            foreach (var token in tokens)
            {
                if (_argsCursor == _args.Length)
                {
                    break;
                }

                if (null != token.Shortcut)
                {
                    if (0 == String.Compare(_args[_argsCursor], token.Shortcut, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var defaultItem in token.GetDefaultRouteData())
                        {
                            routeData.Add(defaultItem.Key, defaultItem.Value);
                        }
                    }
                }
                else
                {
                    if (null != token.Alias && 0 == String.Compare(_args[_argsCursor], token.Alias, StringComparison.OrdinalIgnoreCase))
                    {
                        _argsCursor++;
                        if (_argsCursor == _args.Length)
                        {
                            break;
                        }
                    }
                    routeData.Add(token.RouteDataName, _args[_argsCursor]);
                }

                _argsCursor++;
            }
        }
    }
}
