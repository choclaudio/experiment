using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace VncDeviceProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // vnc client
            var vncClient = new TcpClient("localhost", 5900);
            var vncClientStream = vncClient.GetStream();

            // connect to cloud service 
            var webSocketTunnelClient = new WebSocketClient();
            webSocketTunnelClient.Connect("ws://jnb:9999");
            
            // pipe em up 
            var p = new Pipe(vncClientStream, webSocketTunnelClient);
            p.TunnelAsync().Join();
        }
    }
}
