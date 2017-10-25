using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleRouter.Routing
{
    [DebuggerDisplay("{Raw}")]
    internal class Token
    {
        private Token()
        {
        }

        private List<KeyValuePair<String, String>> _defaults = new List<KeyValuePair<string, string>>();

        public string Name { get; private set; }
        public string Raw { get; private set; }
        public string Shortcut { get; private set; }
        public string Alias { get; private set; }

        public static Token Parse(string raw)
        {
            var result = new Token();

            int shortcutEndIndex = raw.IndexOf("(");
            if (-1 != shortcutEndIndex)
            {
                result.Shortcut = raw.Substring(0, shortcutEndIndex);

                var defaultPairs = raw.Substring(shortcutEndIndex + 1, raw.Length - 1 - shortcutEndIndex - 1).
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var defaultPair in defaultPairs)
                {
                    (string name, string defaultValue) = ParsePair(defaultPair);
                    result._defaults.Add(new KeyValuePair<string, string>(name, defaultValue));
                }
            }
            else
            {
                int aliasEndIndex = raw.IndexOf("{");
                if(0 != aliasEndIndex)
                {
                    result.Alias = raw.Substring(0, aliasEndIndex);
                }

                var rawName = raw.Substring(aliasEndIndex).Replace("{", "").Replace("}", "");
                (string name, string defaultValue) = ParsePair(rawName);

                if (null != name)
                {
                    result.Name = name;
                }

                if (null != defaultValue)
                {
                    result._defaults.Add(new KeyValuePair<string, string>(result.Name, defaultValue));
                }

                result.Raw = raw;
            }
            return result;
        }

        public IEnumerable<KeyValuePair<String, String>> GetDefaults()
        {
            return _defaults;
        }

        private static (string name, string defaultValue) ParsePair(string rawPair)
        {
            var nameParts = rawPair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            string name = null;
            if (1 <= nameParts.Length)
            {
                name = nameParts[0];
            }

            string defaultValue = null;
            if (2 <= nameParts.Length)
            {
                defaultValue = nameParts[1];
            }

            return (name, defaultValue);
        }
    }
}
