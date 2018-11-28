using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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

        public Thread Start()
        {
            Thread joinThread = new Thread(() =>
            {
                Thread t1 = m_ClientStream.CopyToAsync(m_VncServerStream);
                Thread t2 = m_VncServerStream.CopyToAsync(m_ClientStream);
                t1.Join();
                t2.Join();
            });
            joinThread.Start();
            return joinThread;
        }
    }

}
