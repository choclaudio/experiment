using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace VncDeviceProxy
{
    static class StreamExtensions
    {
        public static Thread CopyToAsync(this Stream source, Stream destination)
        {
            var t = new Thread( () => CopyToSync(source, destination, 81920));
            t.Start();
            return t;
        }

        private static void CopyToSync(Stream source, Stream destination, int bufferSize)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize", ("ArgumentOutOfRange_NeedPosNum"));
            }
            if (!source.CanRead && !source.CanWrite)
            {
                throw new ObjectDisposedException(null, ("ObjectDisposed_StreamClosed"));
            }
            if (!destination.CanRead && !destination.CanWrite)
            {
                throw new ObjectDisposedException("destination", ("ObjectDisposed_StreamClosed"));
            }
            if (!source.CanRead)
            {
                throw new NotSupportedException(("NotSupported_UnreadableStream"));
            }
            if (!destination.CanWrite)
            {
                throw new NotSupportedException(("NotSupported_UnwritableStream"));
            }
            CopyToSyncInternal(source, destination, bufferSize);
        }


        private static void CopyToSyncInternal(Stream source, Stream destination, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, bytesRead);
            }
        }


    }
}
