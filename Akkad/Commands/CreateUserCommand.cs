using System;

namespace Akkad.Commands
{
    public class CreateUserCommand : Command
    {
        public string Name { get; private set; }

        public CreateUserCommand(string name) : base(Guid.NewGuid())
        {
            Name = name;
        }
    }
}