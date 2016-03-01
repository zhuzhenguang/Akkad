using Akkad.Commands;

namespace Akkad.CommandQueue
{
    public interface ICommandQueue
    {
        void Enqueue(ICommand command);
        ICommand Dequeue();
    }
}