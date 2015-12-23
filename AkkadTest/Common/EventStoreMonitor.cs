using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;

namespace AkkadTest
{
    public class EventStoreMonitor
    {
        private readonly ClusterVNode _clusterVNode;
        private readonly ConnectionSettings _connectionSettings;

        public EventStoreMonitor(ClusterVNode clusterVNode, ConnectionSettings connectionSettings)
        {
            _clusterVNode = clusterVNode;
            _connectionSettings = connectionSettings;
        }

        public bool IsRunning()
        {
            using (var eventStoreConnection = EmbeddedEventStoreConnection.Create(_clusterVNode, _connectionSettings))
            {
                try
                {
                    eventStoreConnection.AppendToStreamAsync(Guid.NewGuid().ToString(), ExpectedVersion.Any).Wait();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}