namespace Akkad.Commands
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}