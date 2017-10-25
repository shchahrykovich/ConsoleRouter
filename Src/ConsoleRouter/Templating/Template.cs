using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleRouter.Templating
{
    [DebuggerDisplay("{Raw}")]
    internal class Template
    {
        private Template()
        {
        }

        public string Raw { get; private set; }

        public IEnumerable<Token> Tokens => _tokens;

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
    }
}
