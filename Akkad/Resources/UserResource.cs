using System;
using System.Threading.Tasks;
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

        public Task<UserId> Create(string name)
        {
            var createUserCommand = new CreateUserCommand(name);
            var result = _commandBus.SendAsync(createUserCommand).Result;

            if (result.IsSuccess())
            {
                throw new Exception(result.ErrorMessage);
            }
            return new Task<UserId>(() => new UserId(createUserCommand.AggregateId));
        }
    }
}