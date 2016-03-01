using System.Threading.Tasks;
using Akkad.CommandHandlers;
using Akkad.Commands;
using Akkad.Exceptions;

namespace Akkad.CommandQueue
{
    public class InMemoryCommandQueueService : ICommandQueueService
    {
        private const int CommandQueueSize = 3;
        private readonly ICommandQueue[] _commandQueues;
        private ICommandHandlerFactory _commandHandlerFactory;

        public InMemoryCommandQueueService(CommandProcessor commandProcessor, ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlerFactory = commandHandlerFactory;
            commandProcessor.Start(this);

            _commandQueues = new ICommandQueue[CommandQueueSize];
            for (var i = 0; i < CommandQueueSize; i++)
            {
                _commandQueues[i] = new InMemoryCommandQueue();
            }
        }

        public Task<AsyncTaskResult> Push(ICommand command)
        {
            var route = command.AggregateId.GetHashCode()%CommandQueueSize;
            _commandQueues[route].Enqueue(command);

            /*var method = typeof(ICommandHandlerFactory).GetMethod("Get");
            var makeGenericMethod = method.MakeGenericMethod(command.GetType());
            var handler =
                (ICommandHandler<CreateUserCommand>)makeGenericMethod.Invoke(_commandHandlerFactory, null);
            if (handler == null)
            {
                throw new CommandHandlerNotFoundException();
            }
            handler.Handle((CreateUserCommand)command);*/
            return new Task<AsyncTaskResult>(() => AsyncTaskResult.Success);
        }

        public ICommand Take(int route)
        {
            return _commandQueues.Length <= 0 ? null : _commandQueues[route].Dequeue();
        }
    }
}