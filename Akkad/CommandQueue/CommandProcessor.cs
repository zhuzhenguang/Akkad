using Akkad.CommandHandlers;
using Akkad.Commands;
using Akkad.Exceptions;

namespace Akkad.CommandQueue
{
    public class CommandProcessor
    {
        private const int CommandQueueSize = 3;
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public CommandProcessor(ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlerFactory = commandHandlerFactory;
        }

        public void Start(ICommandQueueService commandQueueService)
        {
            for (var i = 0; i < CommandQueueSize; i++)
            {
                var worker = new Worker("ProcessExecutedCommand", () =>
                {
                    var command = commandQueueService.Take(i);
                    if (command == null)
                    {
                        return;
                    }

                    var method = typeof (ICommandHandlerFactory).GetMethod("Get");
                    var makeGenericMethod = method.MakeGenericMethod(command.GetType());
                    var handler =
                        (ICommandHandler<CreateUserCommand>) makeGenericMethod.Invoke(_commandHandlerFactory, null);
                    if (handler == null)
                    {
                        throw new CommandHandlerNotFoundException();
                    }
                    handler.Handle((CreateUserCommand) command);
                });
                worker.Start();
            }
        }
    }
}