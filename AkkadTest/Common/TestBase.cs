using System;
using Akkad;
using Autofac;

namespace AkkadTest
{
    public class TestBase : IDisposable
    {
        private readonly MiniEventStoreServer _miniEventStoreServer;
        private readonly IContainer _rootContainer;

        protected TestBase()
        {
            _rootContainer = AkkadCqrs.Bootstrap();
            ScopeHolder.Scope = _rootContainer.BeginLifetimeScope();
            _miniEventStoreServer = new MiniEventStoreServer();
            MockDependency(_miniEventStoreServer.CreateConnection());
        }

        protected ILifetimeScope Scope
        {
            get { return ScopeHolder.Scope; }
        }

        public void Dispose()
        {
            _rootContainer.Dispose();
            _miniEventStoreServer.Dispose();
        }

        protected void MockDependency<TDependency>(TDependency dependency)
        {
            MockInstance(dependency);
        }

        protected void MockInstance<TDependency>(TDependency instance)
        {
            Mock(b => b.Register(c => instance).As<TDependency>());
        }

        protected void Mock(Action<ContainerBuilder> build)
        {
            var containerBuilder = new ContainerBuilder();
            build(containerBuilder);
            containerBuilder.Update(_rootContainer);
        }
    }

    public class ScopeHolder
    {
        public static ILifetimeScope Scope { get; set; }
    }
}