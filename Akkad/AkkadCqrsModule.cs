using Akkad.CommandHandlers;
using Akkad.CommandQueue;
using Akkad.Commands;
using Autofac;
using CommonDomain.Implementation;
using CommonDomain.Persistence;

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

            builder.RegisterType<InMemoryCommandQueueService>().As<ICommandQueueService>();
            builder.RegisterType<CommandProcessor>();
        }
    }
}