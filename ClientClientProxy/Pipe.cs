using Rebex.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace VncDeviceProxy
{
    class Pipe
    {
        Stream m_Stream;
        WebSocketClient m_WebSocket;

        public Pipe(Stream stream, WebSocketClient webSocket)
        {
            m_Stream = stream;
            m_WebSocket = webSocket;
        }

        public Thread TunnelAsync()
        {
            Thread joinThread = new Thread(() =>
            {
                Thread t1 = m_Stream.CopyToAsync(m_WebSocket);
                Thread t2 = m_WebSocket.CopyToAsync(m_Stream);
                t1.Join();
                t2.Join();
            });
            joinThread.Start();
            return joinThread;
        }
    }

}
