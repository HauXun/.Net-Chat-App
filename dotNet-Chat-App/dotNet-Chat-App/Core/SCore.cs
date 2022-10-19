using dotNet_Chat_App.Common;
using dotNet_Chat_App.Config;
using dotNet_Chat_App.IAction;
using dotNet_Chat_App.IProps;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.TAP_Socket_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Core
{
	public class SCore : IActions, ITodo, ICommunicateSocket, ISCommunicate, IReceivePacket, ITranspotDefine
	{
		const int ReceiveTimeoutMs = 3000;
		const int SendTimeoutMs = 3000;

		#region Properties

		private Socket m_listener;
		private Thread m_thread;
		private Socket m_lastClient;
		private List<Socket> m_clientSockets = new List<Socket>();

		private Task<TAPResultPattern<Socket>> acceptTask;
		private TAPResultPattern<Socket> acceptResult;

		private Task<TAPResultPattern<int>> receivePacketSizeTask;
		private TAPResultPattern<int> receivePacketSizeTaskResult;

		private Task<TAPResultPattern> receivePacketTask;

		private string m_ipAddress = string.Empty;
		private int m_port = 8000;
		private string m_systemMsg = string.Empty;
		private string m_userMsg = string.Empty;
		private List<Client> m_clients = new List<Client>();
		private GetLog m_getLog;
		private bool m_closing;

		// Receive struct
		private byte[] lenBuffer;
		private ReceiveBuffer buffer;

		#endregion

		#region EncapsulationProperties

		public Socket Listener { get => m_listener; private set { } }
		public Thread Thread { get => m_thread; private set { } }
		public Socket LastClient { get => m_lastClient; private set { } }
		public List<Socket> ClientSockets { get => m_clientSockets; private set { } }
		public Task<TAPResultPattern<Socket>> AcceptTask { get => acceptTask; private set { } }
		public TAPResultPattern<Socket> AcceptResult { get => acceptResult; private set { } }

		public Task<TAPResultPattern<int>> ReceivePacketSizeTask { get => receivePacketSizeTask; private set { } }
		public TAPResultPattern<int> ReceivePacketSizeTaskResult { get => receivePacketSizeTaskResult; private set { } }
		public Task<TAPResultPattern> ReceivePacketTask { get => receivePacketTask; private set { } }

		public string IpAddress { get => m_ipAddress; set => m_ipAddress = value; }
		public int Port { get => m_port; set => m_port = value; }
		public string SystemMsg { get => m_systemMsg; set => m_systemMsg = value; }
		public string UserMsg { get => m_userMsg; set => m_userMsg = value; }
		public List<Client> Clients { get => m_clients; set => m_clients = value; }
		public GetLog GetLog { private get => m_getLog; set => m_getLog = value; }

		public bool Closing
		{
			get => m_closing;
			set
			{
				m_closing = value;
				if (value)
				{
					Close();
					buffer.Dispose();
				}
			}
		}

		public byte[] LenBuffer { get => lenBuffer; private set { } }
		public ReceiveBuffer Buffer { get => buffer; private set { } }

		private static ILogger logger = new GUILogger();

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public SCore()
		{
			lenBuffer = new byte[4];
			buffer = new ReceiveBuffer(0);
			Init();
		}

		#region Asynchronous Behavior

		/// <summary>
		/// Create a Task that accept the next incoming connection (ServerAcceptTask)
		/// </summary>
		/// <returns>A TAPResultPattern<Socket> object task to wait execute</returns>
		async Task<TAPResultPattern<Socket>> AcceptConnectionTask()
		{
			return await m_listener.AcceptAsyncz().ConfigureAwait(false);
		}

		/// <summary>
		/// Create a Task that receive the size of the packet before it finishes receiving the packet
		/// </summary>
		/// <returns>A TAPResultPattern<Socket> object task to wait execute</returns>
		public async Task<TAPResultPattern<int>> ReceivePacketSizeAsync(Socket socket)
		{
			var receiveResult = await socket.ReceiveAsyncz
				(lenBuffer, 0, lenBuffer.Length, SocketFlags.None).ConfigureAwait(false);
			var bytesReceived = receiveResult.Value;

			if (bytesReceived == 0)
			{
				if (m_closing)
					return TAPResultPattern.Fail<int>("Error reading packet from client, server was close");

				if (bytesReceived <= 0)
					return TAPResultPattern.Fail<int>("Unable to receive packet from client, the client may have been closed");

				if (bytesReceived != 4)
					return TAPResultPattern.Fail<int>("Error reading packet from client, packet format does not match");
			}

			return TAPResultPattern.Ok(BitConverter.ToInt32(lenBuffer, 0));
		}

		/// <summary>
		/// Create a Task that finishes receiving the packet
		/// </summary>
		/// <returns>A TAPResultPattern<Socket> object task to wait execute</returns>
		public async Task<TAPResultPattern> ReceivePacketAsync(Socket socket)
		{
			var receiveResult = await socket.ReceiveAsyncz
				(buffer.Buffer, 0, buffer.Buffer.Length, SocketFlags.None).ConfigureAwait(false);

			var bytesReceived = receiveResult.Value;

			if (bytesReceived <= 0)
				m_systemMsg += "Unable to receive packet from client, the client may have been closed";

			buffer.BufferStream.Write(buffer.Buffer, 0, bytesReceived);
			buffer.ToReceive -= bytesReceived;

			if (buffer.ToReceive > 0)
			{
				Array.Clear(buffer.Buffer, 0, buffer.Buffer.Length);

				await ReceivePacketAsync(socket);
			}

			return TAPResultPattern.Ok();
		}

		/// <summary>
		/// Createa Task that start sending the data to the endpoint
		/// </summary>
		/// <param name="data">Data will be send</param>
		/// <returns>The byte number have been sent</returns>
		public async Task<TAPResultPattern<int>> SendPacketAsync(object args)
		{
			Tuple<byte[], Socket> data = args as Tuple<byte[], Socket>;
			var sendSizeResult = await data.Item2.SendWithTimeoutAsyncz(
				BitConverter.GetBytes(data.Item1.Length), 0, 4, 0, SendTimeoutMs).ConfigureAwait(false);

			var bytesReceived = sendSizeResult.Value;

			if (bytesReceived == 0)
			{
				if (m_closing)
					return TAPResultPattern.Fail<int>("Error reading packet, client was close");

				if (sendSizeResult.Value <= 0)
					return TAPResultPattern.Fail<int>("Unable to receive packet from client");

				if (bytesReceived != 4)
					return TAPResultPattern.Fail<int>("Error reading packet from client, packet format does not match");
			}

			if (sendSizeResult.Success)
			{
				var sendResult = await data.Item2.SendWithTimeoutAsyncz(data.Item1, 0, data.Item1.Length, 0, SendTimeoutMs).ConfigureAwait(false);

				bytesReceived = sendResult.Value;

				if (sendResult.Failure)
				{
					// m_systemMsg += $"\r\n{sendResult.Error}";
					logger.WriteLogEntry(sendResult.Error, ref m_systemMsg);
				}

				if (bytesReceived == 0)
				{
					if (m_closing)
						return TAPResultPattern.Fail<int>("Error reading packet, client was close");

					if (sendSizeResult.Value <= 0)
						return TAPResultPattern.Fail<int>("Unable to receive packet from client");

					if (bytesReceived != 4)
						return TAPResultPattern.Fail<int>("Error reading packet from client, packet format does not match");
				}
			}
			else
			{
				// m_systemMsg += $"\r\n{sendSizeResult.Error}";
				logger.WriteLogEntry(sendSizeResult.Error, ref m_systemMsg);
			}
			return TAPResultPattern.Ok(bytesReceived);
		}

		public async Task<TAPResultPattern<int>> SendPacketAsync(byte[] data)
		{
			return null;
		}

		#endregion

		/// <summary>
		/// Initialize and bind socket connect
		/// </summary>
		public void Init()
		{
			string ipAddress = m_ipAddress;
			if (string.IsNullOrEmpty(ipAddress))
			{
				ipAddress = IPServices.GetIPAddress();
			}

			IPAddress ipa = IPAddress.Parse(ipAddress);

			m_listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
			m_listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

			// Bind a socket to a local TCP port and Listen for incoming connections
			m_listener.Bind(new IPEndPoint(ipa, m_port));

			StartListening();
		}

		/// <summary>
		/// Close the server and the connection comes from the clients
		/// </summary>
		public void Close()
		{
			m_listener?.Close();

			foreach (Socket clientSocket in m_clientSockets)
			{
				clientSocket.Close();
			}

			m_lastClient?.Close();

			m_listener = null;
			m_lastClient = null;
			m_clientSockets = null;
		}

		/// <summary>
		/// Prepare for start listen from another thread
		/// </summary>
		public void StartListening()
		{
			try
			{
				m_thread = new Thread(new ThreadStart(Listen));
				m_thread.IsBackground = true;
				m_thread.Start();
				// m_systemMsg += "\r\nServer listener started";
				logger.WriteLogEntry("Server listener started", ref m_systemMsg);
			}
			catch (Exception ex)
			{
				// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
				logger.WriteLogEntry(ex.Message, ref m_systemMsg);
			}
		}

		/// <summary>
		/// Listen for incoming connections from the client
		/// </summary>
		public void Listen()
		{
			try
			{
				m_listener.Listen(-1);

				ThreadPool.QueueUserWorkItem(Worker, null);
			}
			catch (Exception ex)
			{
				// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
				logger.WriteLogEntry(ex.Message, ref m_systemMsg);
				m_thread.Interrupt();
				m_thread.Abort();
				StartListening();
			}
		}

		/// <summary>
		/// Continuous listening for incoming connections from the client
		/// </summary>
		/// <param name="token">Token as client connection</param>
		public async void Worker(object token = null)
		{
			try
			{
				while (m_listener != null)
				{
					// The call to AcceptConnectionTask is not awaited, therefore this method
					// continues executing
					acceptTask = Task.Run(AcceptConnectionTask);
					// Await the result of the ServerAcceptTask
					acceptResult = await acceptTask.ConfigureAwait(false);

					if (acceptResult.Failure)
					{
						m_systemMsg += $"{acceptResult.Error}";
						throw new Exception("There was an error connecting to the server/accepting connection from the client");
					}

					// Store the transfer socket if ServerAcceptTask was successful
					m_lastClient = acceptResult.Value;

					m_clientSockets.Add(m_lastClient);


					Thread handle = new Thread(WaitForHandle);

					handle.IsBackground = true;
					handle.Start(m_lastClient);
					handle.Join();
				}
			}
			catch (Exception ex)
			{
				// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
				logger.WriteLogEntry(ex.Message, ref m_systemMsg);
			}
		}

		/// <summary>
		/// Remove disconnected sockets from the list
		/// </summary>
		private void ClearDisconnectClients()
		{
			foreach (Socket s in m_clientSockets.ToList())
			{
				if (!s.Connected)
				{
					m_clientSockets.Remove(s);
					receivePacketSizeTask = null;
					receivePacketTask = null;
				}
			}
		}

		/// <summary>
		/// Handling received connections
		/// </summary>
		/// <param name="token">Client socket</param>
		public async void WaitForHandle(object token = null)
		{
			Socket socket = token as Socket;

			m_systemMsg += $"\r\n\r\n{(socket.RemoteEndPoint as IPEndPoint).Port} Connected";
			// logger.WriteLogEntry($"\r\n{(socket.RemoteEndPoint as IPEndPoint).Port} Connected", m_getLog);
			receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));

			while (m_listener != null)
			{
				try
				{
					if (m_closing) return;

					if (!socket.Connected && receivePacketSizeTask == null)
						receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));

					receivePacketSizeTaskResult = await receivePacketSizeTask.ConfigureAwait(false);

					if (receivePacketSizeTaskResult.Success && socket.Connected)
					{
						buffer = new ReceiveBuffer(receivePacketSizeTaskResult.Value);

						receivePacketTask = Task.Run(() => ReceivePacketAsync(socket));
						await receivePacketTask.ConfigureAwait(false);

						buffer.BufferStream.Position = 0;

						TransactionPacket packet = (TransactionPacket)FragmentationServices.
							Deserialize(buffer.BufferStream.ToArray());

						HandleAction(packet, socket);

						buffer.Dispose();
					}
					else
					{
						if (!string.IsNullOrEmpty(receivePacketSizeTaskResult.Error))
							// m_systemMsg += $"\r\n{receivePacketSizeTaskResult.Error}";
							logger.WriteLogEntry(receivePacketSizeTaskResult.Error, ref m_systemMsg);
						if (!socket.Connected)
						{
							ClearDisconnectClients();
						}
					}

					if (socket.Connected)
						receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));
					else
					{
						ClearDisconnectClients();
						return;
					}
				}
				catch (SocketException ex)
				{
					switch (ex.SocketErrorCode)
					{
						case SocketError.ConnectionAborted:
						case SocketError.ConnectionReset:
							// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
							logger.WriteLogEntry(ex.Message, ref m_systemMsg);
							break;
					}
				}
				catch (ObjectDisposedException ex)
				{
					// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
					logger.WriteLogEntry(ex.Message, ref m_systemMsg);
				}
				catch (IOException ex)
				{
					// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
					logger.WriteLogEntry(ex.Message, ref m_systemMsg);
				}
				catch (SerializationException ex)
				{
					if (m_listener == null || m_closing)
						return;

					// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
					logger.WriteLogEntry(ex.Message, ref m_systemMsg);
				}
				catch (Exception ex)
				{
					// m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
					logger.WriteLogEntry(ex.Message, ref m_systemMsg);
				}
			}
		}

		/// <summary>
		/// Handling received actions 
		/// </summary>
		/// <param name="packet">Packet will be receive</param>
		public void HandleAction(TransactionPacket packet, Socket socket)
		{
			switch (packet.Todo)
			{
				case (int)DoActions.Todo.PushLog:
					m_userMsg += $"\r\n{packet.Value} from {socket.RemoteEndPoint as IPEndPoint}";
					break;
				case (int)DoActions.Todo.PushStatus:
					break;
				case (int)DoActions.Todo.PushOfflineMessage:
					break;
				case (int)DoActions.Todo.PushOfflineGroupMessage:
					break;
				case (int)DoActions.MessageType.ServerSendAll:
					break;
				case (int)DoActions.MessageType.ServerToSingleClient:
					break;
				case (int)DoActions.MessageType.ClientToServer:
					break;
				case (int)DoActions.MessageType.ClientToClient:
					break;
				case (int)DoActions.MessageType.OfflineSending:
					break;
				case (int)DoActions.MessageType.ChatToGroup:
					break;
				default:
					break;
			}
		}

		public async void SendPacket(TransactionPacket packet)
		{
		}

		/// <summary>
		/// Send the packet to the clients
		/// </summary>
		/// <param name="packet">Packet will be send</param>
		public async void SendPacket(TransactionPacket packet, Socket socket)
		{
			Task<TAPResultPattern<int>> handleSend = Task.Run(() =>
			SendPacketAsync(new Tuple<byte[], Socket>(FragmentationServices.Serialize(packet), socket)));
			TAPResultPattern<int> sendResult = await handleSend;

			if (sendResult.Failure)
			{
				m_systemMsg += sendResult.Error;
			}
		}

		public void SendOK()
		{
			foreach (Socket client in m_clientSockets)
			{
				if (client != null && client.Connected)
				{
					SendPacket(new TransactionPacket(0, $"Hello Client"), client);
				}
			}
		}
	}
}
