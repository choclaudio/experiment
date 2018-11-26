using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VncDeviceProxy
{
    class VncServerHandler
    {
        Stream m_ClientStream;
        Stream m_VncServerStream;

        public VncServerHandler(Stream stream, string vncHost, int vncPort)
        {
            m_ClientStream = stream;
            var vncClient = new TcpClient();
            vncClient.NoDelay = true;  // turn-off Nagle's Algorithm for better interactive performance with host.
            vncClient.Connect(vncHost, vncPort);
            m_VncServerStream = vncClient.GetStream();
            
        }

        public void Start()
        {

            Task.Run(async () =>
            {
                try
                {
                    Task t1 = m_ClientStream.CopyToAsync(m_VncServerStream);
                    Task t2 = m_VncServerStream.CopyToAsync(m_ClientStream);
                    await Task.WhenAll(t1, t2);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Forwarded");
                }
            });
        }
    }

}
