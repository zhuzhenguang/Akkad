using Akkad.Commands;

namespace Akkad.CommandHandlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}