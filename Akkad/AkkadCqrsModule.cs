using Akkad.CommandHandlers;
using Akkad.Commands;
using Autofac;
using CommonDomain.Implementation;
using CommonDomain.Persistence;
using EventStore.ClientAPI;

namespace Akkad
{
    public class AkkadCqrsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetEventStoreRepository>().As<IRepository>();
            builder.RegisterType<StreamNamingConvention>().As<IStreamNamingConvention>();
            builder.RegisterType<AutoFactCommandBus>().As<ICommandBus>();
            builder.RegisterType<AutoFacCommandHandlerFactory>().As<ICommandHandlerFactory>();
            builder.RegisterType<CreateUserCommandHandler>().As<ICommandHandler<CreateUserCommand>>();
        }
    }
}