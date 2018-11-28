using System.IO;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace VncDeviceProxy
{
    class Pipe
    {
        private WebSocket S1 { get; }
        private Stream S2 { get; }
        public Pipe(WebSocket s1, Stream s2)
        {
            this.S1 = s1;
            this.S2 = s2;
        }

        public async Task TunnelAsync()
        {
            await Task.WhenAll(S1.CopyToAsync(S2), S2.CopyToAsync(S1));
        }


    }

}
