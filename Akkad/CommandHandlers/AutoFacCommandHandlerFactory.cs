using System;
using Akkad.Commands;
using Autofac;

namespace Akkad.CommandHandlers
{
    public class AutoFacCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IComponentContext _componentContext;

        public AutoFacCommandHandlerFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public ICommandHandler<TCommand> Get<TCommand>() where TCommand : ICommand
        {
            return _componentContext.Resolve<ICommandHandler<TCommand>>();
        }
    }
}