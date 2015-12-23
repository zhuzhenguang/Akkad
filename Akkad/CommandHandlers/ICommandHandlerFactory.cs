using System;
using Akkad.Commands;

namespace Akkad.CommandHandlers
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> Get<TCommand>() where TCommand : ICommand;
    }
}