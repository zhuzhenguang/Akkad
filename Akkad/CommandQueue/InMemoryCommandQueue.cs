using System.Collections.Concurrent;
using Akkad.Commands;

namespace Akkad.CommandQueue
{
    public class InMemoryCommandQueue : ICommandQueue
    {
        private readonly ConcurrentQueue<ICommand> _commandQueue = new ConcurrentQueue<ICommand>();
        private long _maxOffset = 0;
        private long _consumeOffset = 0;

        public void Enqueue(ICommand command)
        {
            _commandQueue.Enqueue(command);
            _maxOffset = _commandQueue.Count;
        }

        public ICommand Dequeue()
        {
            ICommand command;
            _commandQueue.TryDequeue(out command);
            _maxOffset = _commandQueue.Count;
            return command;
        }
    }
}