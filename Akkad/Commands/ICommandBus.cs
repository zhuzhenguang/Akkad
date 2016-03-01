using System.Threading.Tasks;

namespace Akkad.Commands
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
        Task<AsyncTaskResult> SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}