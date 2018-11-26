using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace VncDeviceProxy
{
    class MirrorAndImplementVNCServer
    {
        public Stream GetProxyStream()
        {
            return m_ProxyStream;
        }

        private Stream m_ProxyStream;
        public MirrorAndImplementVNCServer(string vncHost, int vncPort)
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 666);
            listener.Start();

            var threadsToJoin = new List<Thread>();

            Thread t = new Thread(() =>
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread joinThread = new VncServerHandler(client.GetStream(), vncHost, vncPort).Start();
                    threadsToJoin.Add(joinThread); // we never joing them but..
                }
            });
            t.Start();
            t.Join();
        }
    }
}
