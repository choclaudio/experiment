//--------------------------------------------------------------
// Press F1 to get help about using script.
// To access an object that is not located in the current class, start the call with Globals.
// When using events and timers be cautious not to generate memoryleaks,
// please see the help for more information.
//---------------------------------------------------------------

namespace Neo.ApplicationFramework.Generated
{
    using System.Windows.Forms;
    using System;
    using System.Drawing;
    using Neo.ApplicationFramework.Tools;
    using Neo.ApplicationFramework.Common.Graphics.Logic;
    using Neo.ApplicationFramework.Controls;
    using Neo.ApplicationFramework.Interfaces;
	using Rebex.Net;
	using System.IO;
	using System.Threading;
	using System.Net.Sockets;

    
    
    public partial class WSVNC
    {
		public void Connect()
		{
			var vncClient = new TcpClient("localhost", 5900);
			var vncClientStream = vncClient.GetStream();
 
			// connect to cloud service 
			var webSocketTunnelClient = new WebSocketClient();

			webSocketTunnelClient.Connect("ws://40.127.108.43/ws");
            
			// pipe em up 
			var p = new Pipe(vncClientStream, webSocketTunnelClient);
			p.TunnelAsync();
		}
    }

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
