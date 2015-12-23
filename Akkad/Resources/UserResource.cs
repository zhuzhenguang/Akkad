using System;
using Akkad.Commands;
using Akkad.Domain;

namespace Akkad.Resources
{
    public class UserResource
    {
        private readonly ICommandBus _commandBus;

        public UserResource(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public UserId Create(string name)
        {
            var createUserCommand = new CreateUserCommand(name);
            _commandBus.Send(createUserCommand);
            return new UserId(createUserCommand.AggregateId);
        }
    }
}