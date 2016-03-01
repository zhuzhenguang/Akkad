using System.Threading.Tasks;
using Akkad.Commands;

namespace Akkad.CommandQueue
{
    public interface ICommandQueueService
    {
        Task<AsyncTaskResult> Push(ICommand command);
        ICommand Take(int route);
    }
}