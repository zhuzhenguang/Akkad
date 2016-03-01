using System;
using System.Collections;
using System.Threading.Tasks;
using Akkad.CommandQueue;
using Akkad.Commands;
using Akkad.Exceptions;

namespace Akkad.CommandHandlers
{
    public class AutoFactCommandBus : ICommandBus
    {
        private readonly ICommandHandlerFactory _commandHandlerFactory;
        private readonly ICommandQueueService _commandQueueService;

        public AutoFactCommandBus(ICommandHandlerFactory commandHandlerFactory,
            ICommandQueueService commandQueueService)
        {
            _commandHandlerFactory = commandHandlerFactory;
            _commandQueueService = commandQueueService;
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

        public Task<AsyncTaskResult> SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            try
            {
                return _commandQueueService.Push(command);
            }
            catch (Exception e)
            {
                return
                    Task.FromResult(new AsyncTaskResult(AsyncTaskStatus.Failed, new NotImplementedException().Message));
            }
        }
    }
}