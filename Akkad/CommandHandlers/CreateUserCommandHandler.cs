using Akkad.Commands;
using Akkad.Domain;
using CommonDomain.Persistence;

namespace Akkad.CommandHandlers
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IRepository _repository;

        public CreateUserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateUserCommand command)
        {
            var user = new User(command.AggregateId, command.Name);
            _repository.Save(user);
        }
    }
}