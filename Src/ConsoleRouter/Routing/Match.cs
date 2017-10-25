using System;
using System.Reflection;

namespace ConsoleRouter.Routing
{
    internal class Match
    {
        public Match(Type type, MethodInfo methodInfo)
        {
            Type = type;
            MethodInfo = methodInfo;
            Parameters = null;
        }

        public Match(Type type, MethodInfo methodInfo, object[] parameters) : this(type, methodInfo)
        {
            Parameters = parameters;
        }

        public Type Type { get; private set; }

        public MethodInfo MethodInfo { get; internal set; }

        public Object[] Parameters { get; internal set; }
    }
}
