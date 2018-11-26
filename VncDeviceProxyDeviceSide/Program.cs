using Rebex.Net;
using System.Net.Sockets;

namespace VncDeviceProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // vnc client
            var vncClient = new TcpClient("10.101.100.244", 5900);
            var vncClientStream = vncClient.GetStream();

            // connect to cloud service 
            var webSocketTunnelClient = new WebSocketClient();
            webSocketTunnelClient.Connect("ws://10.101.100.75:5000/ws");

            // pipe em up 
            var p = new Pipe(vncClientStream, webSocketTunnelClient);
            p.TunnelAsync().Join();
        }
    }
}
