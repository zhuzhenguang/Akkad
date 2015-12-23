using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;

namespace AkkadTest
{
    public class MiniEventStoreServer
    {
        private readonly ClusterVNode _clusterVNode;
        private readonly List<IPEndPoint> _usedEndPoints = new List<IPEndPoint>();
        private readonly ConcurrentQueue<IEventStoreConnection> _connections = new ConcurrentQueue<IEventStoreConnection>();

        private static readonly PortsPool PortsPool = new PortsPool();

        ~MiniEventStoreServer()
        {
            Dispose();
        }

        public MiniEventStoreServer()
        {
            var ipAddress = IPAddress.Loopback;
            _clusterVNode = EmbeddedVNodeBuilder.AsSingleNode()
                .WithExternalHttpOn(GetAvailablePort(ipAddress))
                .WithInternalHttpOn(GetAvailablePort(ipAddress))
                .WithExternalTcpOn(GetAvailablePort(ipAddress))
                .WithInternalTcpOn(GetAvailablePort(ipAddress))
                .RunInMemory()
                .Build();
            _clusterVNode.Start();
            WaitUntilEventStoreIsReady();
        }

        private void WaitUntilEventStoreIsReady()
        {
            var eventStoreMonitor = new EventStoreMonitor(_clusterVNode, ConnectionSettings());
            WaitHelper.WaitUntil(eventStoreMonitor.IsRunning);
        }

        public void Dispose()
        {
            CloseConnections();
            _clusterVNode.Stop();
            _usedEndPoints.ForEach(c => PortsPool.ReturnPort(c.Port));
        }

        private void CloseConnections()
        {
            IEventStoreConnection connection;
            while (_connections.TryDequeue(out connection))
            {
                connection.Close();
            }
        }

        private IPEndPoint GetAvailablePort(IPAddress ipAddress)
        {
            var ipEndPoint = PortsPool.GetAvailablePort(ipAddress);
            _usedEndPoints.Add(ipEndPoint);
            return ipEndPoint;
        }

        public IEventStoreConnection CreateConnection()
        {
            var eventStoreConnection = EmbeddedEventStoreConnection.Create(_clusterVNode, ConnectionSettings());
            eventStoreConnection.ConnectAsync().Wait();
            _connections.Enqueue(eventStoreConnection);
            return eventStoreConnection;
        }

        private static ConnectionSettings ConnectionSettings()
        {
            var connectionSettings = EventStore.ClientAPI.ConnectionSettings.Create()
                .KeepReconnecting()
                .KeepRetrying()
                .EnableVerboseLogging()
                .UseConsoleLogger()
                .Build();
            return connectionSettings;
        }
    }
}