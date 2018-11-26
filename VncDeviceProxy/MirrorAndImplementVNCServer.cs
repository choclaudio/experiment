using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            Task.Run(async () =>
            {
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    new VncServerHandler(client.GetStream(), vncHost, vncPort).Start();
                }
            });
        }



    }
}
