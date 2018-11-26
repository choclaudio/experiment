using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VncDeviceProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            new MirrorAndImplementVNCServer(args[0], int.Parse(args[1]));
            Thread.Sleep(-1);
        }
    }
}
