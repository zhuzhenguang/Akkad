using Autofac;
using Autofac.Core;

namespace Akkad
{
    public class AkkadCqrs
    {
        public static IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AkkadCqrsModule>();
            return builder.Build();
        } 
    }
}