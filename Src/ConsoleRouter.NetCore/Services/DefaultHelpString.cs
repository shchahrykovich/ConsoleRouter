using System;

namespace ConsoleRouter.Services
{
    public class DefaultHelpString
    {
        public String Message { get; private set; }

        public DefaultHelpString(String helpMessage)
        {
            Message = helpMessage;
        }
    }
}
