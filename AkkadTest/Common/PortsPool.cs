using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AkkadTest
{
    public class PortsPool
    {
        private const int PortStart = 45000;
        private readonly int _poolSize;
        private readonly ConcurrentQueue<int> _availablePorts;

        public PortsPool(int poolSize = 1000)
        {
            _poolSize = poolSize;
            _availablePorts = new ConcurrentQueue<int>(Enumerable.Range(PortStart, poolSize));

            InitPorts(IPAddress.Loopback);
        }

        private void InitPorts(IPAddress ip)
        {
            int p;
            while (_availablePorts.TryDequeue(out p))
            {
            }

            var succ = 0;
            for (int port = PortStart; port < PortStart + _poolSize; ++port)
            {
                try
                {
                    var listener = new TcpListener(ip, port);
                    listener.Start();
                    listener.Stop();
                }
                catch (Exception)
                {
                    continue;
                }

                try
                {
                    var httpListener = new HttpListener();
                    httpListener.Prefixes.Add(string.Format("http://127.0.0.1:{0}/", port));
                    httpListener.Start();

                    Exception httpListenerError = null;
                    var listenTask = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var context = httpListener.GetContext();
                            context.Response.Close(new byte[] { 1, 2, 3 }, true);
                        }
                        catch (Exception exc)
                        {
                            httpListenerError = exc;
                        }
                    });

                    var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}:{1}/", ip, port));
                    var buffer = new byte[256];
                    var responseStream = request.GetResponse().GetResponseStream();
                    if (responseStream != null)
                    {
                        var read = responseStream.Read(buffer, 0, buffer.Length);
                        if (read != 3 || buffer[0] != 1 || buffer[1] != 2 || buffer[2] != 3)
                            throw new Exception(string.Format("Unexpected response received from HTTP on port {0}.",
                                port));
                    }

                    if (!listenTask.Wait(5000))
                        throw new Exception("PortsHelper: time out waiting for HttpListener to return.");
                    if (httpListenerError != null)
                        throw httpListenerError;

                    httpListener.Stop();
                }
                catch (Exception)
                {
                    continue;
                }

                _availablePorts.Enqueue(port);
                succ += 1;
            }
            if (succ <= _poolSize / 2)
                throw new Exception("More than half requested ports are unavailable.");
        }

        public IPEndPoint GetAvailablePort(IPAddress ip)
        {
            for (var i = 0; i < 50; ++i)
            {
                int port;
                if (!_availablePorts.TryDequeue(out port))
                    throw new Exception("Could not get free TCP port for MiniNode.");
                try
                {
                    var httpListener = new HttpListener();
                    httpListener.Prefixes.Add(string.Format("http://127.0.0.1:{0}/", port));
                    httpListener.Start();
                    httpListener.Stop();
                }
                catch (Exception)
                {
                    _availablePorts.Enqueue(port);
                    continue;
                }
                return new IPEndPoint(ip, port);
            }
            throw new Exception("Reached trials limit while trying to get free port for MiniNode");
        }

        public void ReturnPort(int port)
        {
            _availablePorts.Enqueue(port);
        }
    }
}