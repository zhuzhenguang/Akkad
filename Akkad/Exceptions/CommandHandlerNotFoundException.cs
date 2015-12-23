using System;

namespace Akkad.Exceptions
{
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException() : base("Command Handler Not Found")
        {
        }
    }
}