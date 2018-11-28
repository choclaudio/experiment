using Rebex.Net;
using System;
using System.IO;
using System.Threading;

namespace VncDeviceProxy
{
    static class StreamExtensions
    {
        public static Thread CopyToAsync(this WebSocketClient source, Stream destination)
        {
            var t = new Thread(() => CopyToSync(source, destination, 81920));
            t.Start();
            return t;
        }

        public static Thread CopyToAsync(this Stream source, WebSocketClient destination)
        {
            var t = new Thread( () => CopyToSync(source, destination, 81920));
            t.Start();
            return t;
        }

        private static void CopyToSync(Stream source, WebSocketClient destination, int bufferSize)
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
            //if (!destination.CanRead && !destination.CanWrite)
            //{
            //    throw new ObjectDisposedException("destination", ("ObjectDisposed_StreamClosed"));
            //}
            if (!source.CanRead)
            {
                throw new NotSupportedException(("NotSupported_UnreadableStream"));
            }
            //if (!destination.CanWrite)
            //{
            //    throw new NotSupportedException(("NotSupported_UnwritableStream"));
            //}
            CopyToSyncInternal(source, destination, bufferSize);
        }


        private static void CopyToSyncInternal(Stream source, WebSocketClient destination, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                byte[] sendBuffer = bytesRead == bufferSize ? buffer : null;
                if (sendBuffer == null)
                {
                    sendBuffer = new byte[bytesRead];
                    Array.Copy(buffer, sendBuffer, bytesRead);
                }
                destination.Send(sendBuffer);
            }
        }


        private static void CopyToSync(WebSocketClient source, Stream destination, int bufferSize)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize", ("ArgumentOutOfRange_NeedPosNum"));
            }
            //if (!source.CanRead && !source.CanWrite)
            //{
            //    throw new ObjectDisposedException(null, ("ObjectDisposed_StreamClosed"));
            //}
            if (!destination.CanRead && !destination.CanWrite)
            {
                throw new ObjectDisposedException("destination", ("ObjectDisposed_StreamClosed"));
            }
            //if (!source.CanRead)
            //{
            //    throw new NotSupportedException(("NotSupported_UnreadableStream"));
            //}
            if (!destination.CanWrite)
            {
                throw new NotSupportedException(("NotSupported_UnwritableStream"));
            }
            CopyToSyncInternal(source, destination, bufferSize);
        }

        private static void CopyToSyncInternal(WebSocketClient source, Stream destination, int bufferSize)
        {
            var buffer = new ArraySegment<byte>(new byte[bufferSize]);
            int bytesRead;
            while ((bytesRead = source.Receive(buffer).Count) != 0)
            {
                destination.Write(buffer.Array, 0, bytesRead);
            }
        }
    }

   
}
