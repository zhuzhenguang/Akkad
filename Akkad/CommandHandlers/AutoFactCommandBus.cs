using Akkad.Commands;
using Akkad.Exceptions;

namespace Akkad.CommandHandlers
{
    public class AutoFactCommandBus : ICommandBus
    {
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public AutoFactCommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlerFactory = commandHandlerFactory;
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _commandHandlerFactory.Get<TCommand>();
            if (handler == null)
            {
                throw new CommandHandlerNotFoundException();
            }
            handler.Handle(command);
        }
    }
}