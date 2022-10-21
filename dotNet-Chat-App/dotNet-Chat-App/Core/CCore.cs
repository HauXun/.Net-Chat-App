using dotNet_Chat_App.Common;
using dotNet_Chat_App.Config;
using dotNet_Chat_App.IAction;
using dotNet_Chat_App.IProps;
using dotNet_Chat_App.Model.BusinessLogicLayer;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.TAP_Socket_Model;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Core
{
    public class CCore : IActions, ITodo, ICommunicateSocket, ICCommunicate, IReceivePacket, ISendPacket, ITranspotDefine
    {
        const int ConnectTimeoutMs = 3000;
        const int ReceiveTimeoutMs = 3000;
        const int SendTimeoutMs = 3000;

        #region Properties

        private Socket m_client;
        private Client m_myClient = new Client();

        private TAPResultPattern connectResult;
        private Task<TAPResultPattern<int>> receivePacketSizeTask;
        private TAPResultPattern<int> receivePacketSizeTaskResult;

        private Task<TAPResultPattern<int>> handleSendTask;
        private TAPResultPattern<int> handleSendTaskResult;

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
        private ClientListChanged ClientListChanged;

        #endregion

        #region EncapsulationProperties

        public Socket Client { get => m_client; private set { } }
        public Client MyClient { get => m_myClient; set => m_myClient = value; }

        public TAPResultPattern ConnectResult { get => connectResult; private set { } }
        public Task<TAPResultPattern<int>> ReceivePacketSizeTask { get => receivePacketSizeTask; private set { } }
        public TAPResultPattern<int> ReceivePacketSizeTaskResult { get => receivePacketSizeTaskResult; private set { } }

        public Task<TAPResultPattern<int>> HandleSendTask { get => handleSendTask; private set { } }
        public TAPResultPattern<int> HandleSendTaskResult { get => handleSendTaskResult; private set { } }

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
        public CCore(ClientListChanged clientListChanged)
        {
            lenBuffer = new byte[4];
            buffer = new ReceiveBuffer(0);
            ClientListChanged = clientListChanged;
        }

        #region Asynchronous Behavior

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
                    return TAPResultPattern.Fail<int>("Error reading packet");

                if (bytesReceived <= 0)
                    return TAPResultPattern.Fail<int>("Unable to receive packet, may have lost connection");

                if (bytesReceived != 4)
                    return TAPResultPattern.Fail<int>("Error reading packet, format does not match");
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
                m_systemMsg += "Unable to receive packet from client";

            buffer.BufferStream.Write(buffer.Buffer, 0, bytesReceived);
            buffer.ToReceive -= bytesReceived;

            if (buffer.ToReceive > 0)
            {
                Array.Clear(buffer.Buffer, 0, buffer.Buffer.Length);

                await ReceivePacketAsync(socket).ConfigureAwait(false);
            }

            return TAPResultPattern.Ok();
        }

        /// <summary>
        /// Createa Task that start sending the data to the endpoint
        /// </summary>
        /// <param name="data">Data will be send</param>
        /// <returns>The byte number have been sent</returns>
        public async Task<TAPResultPattern<int>> SendPacketAsync(byte[] data)
        {
            var sendSizeResult = await m_client.SendWithTimeoutAsyncz(
                BitConverter.GetBytes(data.Length), 0, 4, 0, SendTimeoutMs).ConfigureAwait(false);

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
                var sendResult = await m_client.SendWithTimeoutAsyncz(data, 0, data.Length, 0, SendTimeoutMs).ConfigureAwait(false);

                bytesReceived = sendResult.Value;

                if (sendResult.Failure)
                {
                    // m_systemMsg += $"\r\n{sendResult.Error}";
                    logger.WriteLogEntry(sendSizeResult.Error, ref m_systemMsg);
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
                //m_systemMsg += $"\r\n{sendSizeResult.Error}";
                logger.WriteLogEntry(sendSizeResult.Error, ref m_systemMsg);
            }
            return TAPResultPattern.Ok(bytesReceived);
        }

        public async Task<TAPResultPattern<int>> SendPacketAsync(object args)
        {
            return null;
        }


        #endregion

        /// <summary>
        /// Initialize and connect to socket
        /// </summary>
        public async void Init()
        {
            string ipAddress = m_ipAddress;
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = IPServices.GetIPAddress();
            }

            m_client = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            m_client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, false);

            // With another socket, connect to the bound socket and await the result (ClientConnectTask)
            connectResult = await m_client.ConnectWithTimeoutAsyncz(ipAddress.ToString(), m_port,
                    ConnectTimeoutMs).ConfigureAwait(false);


            if (connectResult.Failure)
            {
                m_systemMsg += $"\r\n{connectResult.Error}";
                //logger.WriteLogEntry(connectResult.Error, m_getLog);
                m_systemMsg += $"\r\nThere was an error connecting to the server/accepting connection from the client";
            }
        }

        /// <summary>
        /// Close the server and the connection comes from the clients
        /// </summary>
        public void Close()
        {
            m_closing = true;
            m_client?.Close();
            m_client = null;
        }

        /// <summary>
        /// Connect instead of Listen for client
        /// </summary>
        public void Listen()
        {
            try
            {
                Worker();
                ThreadPool.QueueUserWorkItem(WaitForHandle, m_client);
            }
            catch (ArgumentNullException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry(ex.Message, ref m_systemMsg);
            }
            catch (SocketException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry(ex.Message, ref m_systemMsg);
            }
            catch (Exception ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry(ex.Message, ref m_systemMsg);
            }
        }

        /// <summary>
        /// Start connecting to the remote endpoint
        /// </summary>
        /// <param name="token"></param>
        public void Worker(object token = null)
        {
            Init();
        }

        /// <summary>
        /// Handling received connections
        /// </summary>
        /// <param name="token">Incoming socket</param>
        public async void WaitForHandle(object token = null)
        {
            Socket socket = token as Socket;
            m_systemMsg += $"\r\n{"There is a connection incoming"}";
            // logger.WriteLogEntry("There is a connection incoming", m_getLog);

            receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));

            try
            {
                while (m_client != null)
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
                    else if (!string.IsNullOrEmpty(receivePacketSizeTaskResult.Error))
                    {
                        // m_systemMsg += $"\r\n{receivePacketSizeTaskResult.Error}";
                        logger.WriteLogEntry(receivePacketSizeTaskResult.Error, ref m_systemMsg);
                    }

                    if (socket.Connected)
                        receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));
                }
            }
            catch (ArgumentNullException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry(ex.Message, ref m_systemMsg);
            }
            catch (SocketException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry(ex.Message, ref m_systemMsg);
            }
            catch (SerializationException ex)
            {
                if (m_client == null || m_closing)
                    return;

                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry(ex.Message, ref m_systemMsg);
            }
            catch (Exception ex)
            {
                //m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry(ex.Message, ref m_systemMsg);
            }
        }


        /// <summary>
        /// Handling received actions 
        /// </summary>
        /// <param name="packet">Packet will be receive</param>
        /// <param name="socket">In CCore does not use this</param>
        public void HandleAction(TransactionPacket packet, Socket socket)
        {
            switch (packet.Todo)
            {
                case (int)DoActions.Todo.PushLog:
                    m_userMsg += $"\r\n{packet.Value} from {m_client.RemoteEndPoint} endpoint";
                    break;
                case (int)DoActions.Todo.PushStatus:
                    //int count = -1;
                    //if (typeof(int) == packet.Value.GetType())
                    //{
                    //    count = Convert.ToInt32(packet.Value);
                    //}
                    //else
                    {
                        m_clients.Add(packet.Value as Client);
                    }

                    if (typeof(string) == packet.Value.GetType() && packet.Value.ToString().Equals("send clients oke"))
                    {
                        this.ClientListChanged();
                    }
                    break;
                case (int)DoActions.Todo.PushMessage:
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

        /// <summary>
        /// Send the packet to the endpoint
        /// </summary>
        /// <param name="packet">Packet will be send</param>
        public async Task SendPacket(TransactionPacket packet)
        {
            if (handleSendTask == null)
            {
                handleSendTask = Task.Run(() =>
                SendPacketAsync(FragmentationServices.Serialize(packet)));
            }
            handleSendTaskResult = await handleSendTask.ConfigureAwait(false);

            if (handleSendTaskResult.Success)
            {
                handleSendTask = Task.Run(() =>
                SendPacketAsync(FragmentationServices.Serialize(packet))); ;
            }
            else
            {
                m_systemMsg += handleSendTaskResult.Error;
            }
        }

        public async Task SendPacket(TransactionPacket packet, Socket socket)
        {
        }
    }
}
