using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System.IO;

namespace dotNet_Chat_App.TAP_Socket_Model
{
	public static class TAPSocket
	{
		/// <summary>
		/// Socket TAP connect with timeout asynchronous
		/// </summary>
		/// <param name="socket">The socket context</param>
		/// <param name="remoteIpAddress">IP Address of remote</param>
		/// <param name="port">Port of remote</param>
		/// <param name="timeoutMs">The timeout value to connect</param>
		/// <returns>The TAPResultPattern object that contains information about the action taken</returns>
		public static async Task<TAPResultPattern> ConnectWithTimeoutAsyncz(
				this Socket socket,
				string remoteIpAddress,
				int port,
				int timeoutMs
			)
		{
			try
			{
				var connectTask = Task.Factory.FromAsync(
					socket.BeginConnect,
					socket.EndConnect,
					remoteIpAddress,
					port,
					null);

				if (connectTask == await Task.WhenAny(connectTask, Task.Delay(timeoutMs)).ConfigureAwait(false))
				{
					await connectTask.ConfigureAwait(false);
				}
				else
				{
					throw new TimeoutException();
				}
			}
			catch (SocketException ex)
			{
				return TAPResultPattern.Fail($"{ex.Message} ({ex.GetType()})");
			}
			catch (TimeoutException ex)
			{
				return TAPResultPattern.Fail($"{ex.Message} ({ex.GetType()})");
			}
			catch (Exception ex)
			{
				return TAPResultPattern.Fail($"{ex.Message} ({ex.GetType()})");
			}

			return TAPResultPattern.Ok();
		}

		/// <summary>
		/// Socket TAP connect asynchronous
		/// </summary>
		/// <param name="socket">The socket context</param>
		/// <param name="remoteIpAddress">IP Address of remote</param>
		/// <param name="port">Port of remote</param>
		/// <returns>The TAPResultPattern object that contains information about the action taken</returns>
		public static async Task<TAPResultPattern> ConnectAsyncz
			(
				this Socket socket,
				string remoteIpAddress,
				int port
			)
		{
			try
			{
				var connectTask = Task.Factory.FromAsync(
					socket.BeginConnect,
					socket.EndConnect,
					remoteIpAddress,
					port,
					null);

				await connectTask.ConfigureAwait(false);
			}
			catch (SocketException ex)
			{
				return TAPResultPattern.Fail($"{ex.Message} ({ex.GetType()})");
			}
			catch (Exception ex)
			{
				return TAPResultPattern.Fail($"{ex.Message} ({ex.GetType()})");
			}

			return TAPResultPattern.Ok();
		}

		/// <summary>
		/// Perform an asynchronous action to accept incoming sockets
		/// </summary>
		/// <param name="socket">The socket context</param>
		/// <returns>The TAPResultPattern object that contains information about the action taken</returns>
		public static async Task<TAPResultPattern<Socket>> AcceptAsyncz(this Socket socket)
		{
			Socket transferSocket;
			try
			{
				var acceptTask = Task<Socket>.Factory.FromAsync(socket.BeginAccept, socket.EndAccept, null);
				transferSocket = await acceptTask.ConfigureAwait(false);
			}
			catch (SocketException ex)
			{
				return TAPResultPattern.Fail<Socket>($"{ex.Message} ({ex.GetType()})");
			}
			catch (InvalidOperationException ex)
			{
				return TAPResultPattern.Fail<Socket>($"{ex.Message} ({ex.GetType()})");
			}
			catch (Exception ex)
			{
				return TAPResultPattern.Fail<Socket>($"{ex.Message} ({ex.GetType()})");
			}

			return TAPResultPattern.Ok(transferSocket);
		}

		/// <summary>
		/// Socket TAP receive with timeout asynchronous
		/// </summary>
		/// <param name="socket">The socket context</param>
		/// <param name="buffer">An array of type System.Byte that is the storage location for the received data.</param>
		/// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the System.Net.Sockets.SocketFlags values.</param>
		/// <param name="timeoutMs">The timeout value to connect</param>
		/// <returns>The TAPResultPattern object that contains information about the action taken (The number of bytes received)</returns>
		public static async Task<TAPResultPattern<int>> ReceiveWithTimeoutAsyncz(
			this Socket socket,
			byte[] buffer,
			int offset,
			int size,
			SocketFlags socketFlags,
			int timeoutMs)
		{
			int bytesReceived;
			try
			{
				var asyncResult = socket.BeginReceive(buffer, offset, size, socketFlags, null, null);
				var receiveTask = Task<int>.Factory.FromAsync(asyncResult, _ => socket.EndReceive(asyncResult));

				if (receiveTask == await Task.WhenAny(receiveTask, Task.Delay(timeoutMs)).ConfigureAwait(false))
				{
					bytesReceived = await receiveTask.ConfigureAwait(false);
				}
				else
				{
					throw new TimeoutException();
				}
			}
			catch (SocketException ex)
			{
				throw ex;
			}
			catch (ObjectDisposedException ex)
			{
				throw ex;
			}
			catch (TimeoutException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (Exception ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}

			return TAPResultPattern.Ok(bytesReceived);
		}

		/// <summary>
		/// Socket TAP receive asynchronous
		/// </summary>
		/// <param name="socket">The socket context</param>
		/// <param name="buffer">An array of type System.Byte that is the storage location for the received data.</param>
		/// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the System.Net.Sockets.SocketFlags values.</param>
		/// <returns>The TAPResultPattern object that contains information about the action taken (The number of bytes received)</returns>
		public static async Task<TAPResultPattern<int>> ReceiveAsyncz(
			this Socket socket,
			byte[] buffer,
			int offset,
			int size,
			SocketFlags socketFlags)
		{
			int bytesReceived;
			try
			{
				int receivePacketCallback(IAsyncResult ar)
				{
					int rec = -1;
					try
					{
						rec = socket.EndReceive(ar);
					}
					catch (Exception e)
					{
						return -1;
					}	
					return rec;
				}

				var asyncResult = socket.BeginReceive(buffer, offset, size, socketFlags, null, null);
				bytesReceived = await Task<int>.Factory.FromAsync(asyncResult, receivePacketCallback);
			}
			catch (SocketException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (ObjectDisposedException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (Exception ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}

			return TAPResultPattern.Ok(bytesReceived);
		}

		/// <summary>
		/// Socket TAP send with timeout asynchronous
		/// </summary>
		/// <param name="socket">The socket context</param>
		/// <param name="buffer">An array of type System.Byte that is the storage location for the received data.</param>
		/// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the System.Net.Sockets.SocketFlags values.</param>
		/// <param name="timeoutMs">The timeout value to connect</param>
		/// <returns>The TAPResultPattern object that contains information about the action taken (The number of bytes sent to the System.Net.Sockets.Socket)</returns>
		public static async Task<TAPResultPattern<int>> SendWithTimeoutAsyncz(
			this Socket socket,
			byte[] buffer,
			int offset,
			int size,
			SocketFlags socketFlags,
			int timeoutMs)
		{
			int bytesSent;
			try
			{
				int sendPacketCallback(IAsyncResult ar)
				{
					int rec = -1;
					try
					{
						rec = socket.EndSend(ar);
					}
					catch (Exception e)
					{
						return -1;
					}
					return rec;
				}

				var asyncResult = socket.BeginSend(buffer, offset, size, socketFlags, null, null);
				var sendBytesTask = Task<int>.Factory.FromAsync(asyncResult, sendPacketCallback);

				if (sendBytesTask != await Task.WhenAny(sendBytesTask, Task.Delay(timeoutMs)).ConfigureAwait(false))
				{
					throw new TimeoutException();
				}

				bytesSent = await sendBytesTask;
			}
			catch (SocketException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (TimeoutException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (IOException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (Exception ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}

			return TAPResultPattern.Ok(bytesSent);
		}

		/// <summary>
		/// Socket TAP send asynchronous
		/// </summary>
		/// <param name="socket">The socket context</param>
		/// <param name="buffer">An array of type System.Byte that is the storage location for the received data.</param>
		/// <param name="offset">The zero-based position in the buffer parameter at which to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the System.Net.Sockets.SocketFlags values.</param>
		/// <returns>The TAPResultPattern object that contains information about the action taken (The number of bytes sent to the System.Net.Sockets.Socket)</returns>
		public static async Task<TAPResultPattern<int>> SendAsyncz(
			this Socket socket,
			byte[] buffer,
			int offset,
			int size,
			SocketFlags socketFlags)
		{
			int bytesSent;
			try
			{
				var asyncResult = socket.BeginSend(buffer, offset, size, socketFlags, null, null);
				var sendBytesTask = Task<int>.Factory.FromAsync(asyncResult, _ => socket.EndSend(asyncResult));

				bytesSent = await sendBytesTask;
			}
			catch (SocketException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (IOException ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}
			catch (Exception ex)
			{
				return TAPResultPattern.Fail<int>($"{ex.Message} ({ex.GetType()})");
			}

			return TAPResultPattern.Ok(bytesSent);
		}
	}
}