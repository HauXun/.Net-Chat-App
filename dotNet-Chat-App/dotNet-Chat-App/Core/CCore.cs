using dotNet_Chat_App.Common;
using dotNet_Chat_App.Config;
using dotNet_Chat_App.IAction;
using dotNet_Chat_App.IProps;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.TAP_Socket_Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Core
{
    public class CCore : IActions, ITodo, ICommunicateSocket, ICCommunicate, IReceivePacket, ISendPacket, ITranspotDefine, IDelegateDefined
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
        private Task<TAPResultPattern<int>> receivePacketTask;
        private TAPResultPattern<int> receivePacketTaskResult;

        private Task<TAPResultPattern<int>> handleSendTask;
        private TAPResultPattern<int> handleSendTaskResult;

        private string m_ipAddress = string.Empty;
        private int m_port = 8000;
        private string m_systemMsg = string.Empty;
        private string m_userMsg = string.Empty;
        private int onlineClientCount;
        private List<Client> m_clients = new List<Client>();
        private List<ChatGroup> groupList = new List<ChatGroup>();
        private bool p2p;
        private bool m_closing;

        // Receive struct
        private byte[] lenBuffer;
        private ReceiveBuffer buffer;
        private ListChanged listChanged;
        private ClearListContainer clearListContainer;
        private ReceiveRequestPacket receiveRequestPacket;
        private int sentBytes;
        private int toSent;

        #endregion

        #region EncapsulationProperties

        public Socket Client { get => m_client; private set { } }
        public Client MyClient { get => m_myClient; set => m_myClient = value; }

        public TAPResultPattern ConnectResult { get => connectResult; private set { } }

        public Task<TAPResultPattern<int>> ReceivePacketSizeTask { get => receivePacketSizeTask; private set { } }
        public TAPResultPattern<int> ReceivePacketSizeTaskResult { get => receivePacketSizeTaskResult; private set { } }
        public Task<TAPResultPattern<int>> ReceivePacketTask { get => receivePacketTask; private set { } }
        public TAPResultPattern<int> ReceivePacketTaskResult { get => receivePacketTaskResult; private set { } }

        public Task<TAPResultPattern<int>> HandleSendTask { get => handleSendTask; private set { } }
        public TAPResultPattern<int> HandleSendTaskResult { get => handleSendTaskResult; private set { } }

        public string IpAddress { get => m_ipAddress; set => m_ipAddress = value; }
        public int Port { get => m_port; set => m_port = value; }
        public string SystemMsg { get => m_systemMsg; set => m_systemMsg = value; }
        public string UserMsg { get => m_userMsg; set => m_userMsg = value; }
        public int OnlineClientCount { get => onlineClientCount; set => onlineClientCount = value; }
        public List<Client> Clients { get => m_clients; set => m_clients = value; }
        public List<ChatGroup> GroupList { get => groupList; set => groupList = value; }
        public bool P2P { get => p2p; set => p2p = value; }

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

        public ListChanged ListChanged { get => this.listChanged; set => listChanged = value; }
        public ClearListContainer ClearListContainer { get => this.clearListContainer; set => clearListContainer = value; }
        public ReceiveRequestPacket ReceiveRequestPacket { get => this.receiveRequestPacket; set => receiveRequestPacket = value; }

        private static ILogger logger = new GUILogger();

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CCore()
        {
            lenBuffer = new byte[4];
            buffer = new ReceiveBuffer(0);
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
                    return TAPResultPattern.Fail<int>($"\r\nError reading packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                if (bytesReceived <= 0)
                    return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                if (bytesReceived != 4)
                    return TAPResultPattern.Fail<int>("\r\nError reading packet from endpoint, format does not match");
            }

            return TAPResultPattern.Ok(BitConverter.ToInt32(lenBuffer, 0));
        }

        /// <summary>
        /// Create a Task that finishes receiving the packet
        /// </summary>
        /// <returns>A TAPResultPattern<Socket> object task to wait execute</returns>
        public async Task<TAPResultPattern<int>> ReceivePacketAsync(Socket socket)
        {
            var receiveResult = await socket.ReceiveAsyncz
                (buffer.Buffer, 0, buffer.Buffer.Length, SocketFlags.None).ConfigureAwait(false);

            var bytesReceived = receiveResult.Value;

            if (bytesReceived <= 0)
            {
                return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet from {socket.RemoteEndPoint}, the endpoint may have been closed");
            }

            buffer.BufferStream.Write(buffer.Buffer, 0, bytesReceived);
            buffer.ToReceive -= bytesReceived;

            if (buffer.ToReceive > 0)
            {
                Array.Clear(buffer.Buffer, 0, buffer.Buffer.Length);

                await ReceivePacketAsync(socket).ConfigureAwait(false);
            }

            return TAPResultPattern.Ok(bytesReceived);
        }

        /// <summary>
        /// Createa Task that start sending the data to the endpoint
        /// </summary>
        /// <param name="data">Data will be send</param>
        /// <returns>The byte number have been sent</returns>
        public async Task<TAPResultPattern<int>> SendPacketAsync(byte[] data, Socket socket)
        {
            var sendSizeResult = await socket.SendWithTimeoutAsyncz(
                BitConverter.GetBytes(data.Length), 0, 4, 0, SendTimeoutMs).ConfigureAwait(false);

            var bytesReceived = sendSizeResult.Value;

            if (sendSizeResult.Failure)
            {
                if (m_closing)
                    return TAPResultPattern.Fail<int>($"\r\nError reading packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                if (bytesReceived <= 0)
                    return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                if (bytesReceived != 4)
                    return TAPResultPattern.Fail<int>("\r\nError reading packet from endpoint, format does not match");
            }

            TAPResultPattern<int> sendResult = null;

            if (data.Length >= 1024)
            {
                sentBytes = 0;
                toSent = 1024;
                while ((data.Length - sentBytes) > 0)
                {
                    sendResult = await socket.SendWithTimeoutAsyncz(data, sentBytes, toSent, 0, SendTimeoutMs).ConfigureAwait(false);
                    bytesReceived = sendResult.Value;

                    if (sendResult.Failure)
                    {
                        if (m_closing)
                            return TAPResultPattern.Fail<int>($"\r\nError reading packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                        if (bytesReceived <= 0)
                            return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                        if (bytesReceived != 4)
                            return TAPResultPattern.Fail<int>("\r\nError reading packet from endpoint, packet format does not match");
                    }
                    sentBytes += 1024;
                    toSent = data.Length - sentBytes;
                }
            }
            else
            {
                sendResult = await socket.SendWithTimeoutAsyncz(data, 0, data.Length, 0, SendTimeoutMs).ConfigureAwait(false);
                bytesReceived = sendResult.Value;

                if (sendResult.Failure)
                {
                    if (m_closing)
                        return TAPResultPattern.Fail<int>($"\r\nError reading packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                    if (bytesReceived <= 0)
                        return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet, may have lost connection with the endpoint {socket.RemoteEndPoint}");

                    if (bytesReceived != 4)
                        return TAPResultPattern.Fail<int>("\r\nError reading packet from endpoint, packet format does not match");
                }
            }

            return TAPResultPattern.Ok(bytesReceived);
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

            // With another socket, connect to the bound socket and await the result (ClientConnectTask)
            connectResult = await m_client.ConnectWithTimeoutAsyncz(ipAddress.ToString(), m_port, ConnectTimeoutMs).ConfigureAwait(false);

            if (connectResult.Failure)
            {
                //m_systemMsg += $"\r\n{connectResult.Error}";
                //m_systemMsg += $"\r\nThere was an error connecting to the server/accepting connection from the client";
                logger.WriteLogEntry($"\r\n{connectResult.Error}", ref m_systemMsg);
                logger.WriteLogEntry($"\r\nThere was an error connecting to the server/accepting connection from the client", ref m_systemMsg);

                //AsynchronousServices.setTimeout(() => Listen(), TimeSpan.FromSeconds(5));
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
        public async void Listen()
        {
            try
            {
                await Task.Run(() => Worker());
                ThreadPool.QueueUserWorkItem(WaitForHandle, m_client);
            }
            catch (ArgumentNullException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
            }
            catch (SocketException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
            }
            catch (Exception ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
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
            logger.WriteLogEntry("\r\nThere is a connection incoming", ref m_systemMsg);
            //m_systemMsg += $"\r\n{"There is a connection incoming"}";

            if (!p2p)
                AsynchronousServices.setTimeout(() => SendLog(), TimeSpan.FromMilliseconds(0.5));

            try
            {
                while (m_client != null)
                {
                    if (m_closing) return;

                    receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));
                    receivePacketSizeTaskResult = await receivePacketSizeTask.ConfigureAwait(false);

                    if (receivePacketSizeTaskResult.Failure)
                    {
                        logger.WriteLogEntry(receivePacketSizeTaskResult.Error, ref m_systemMsg);
                        return;
                    }

                    buffer = new ReceiveBuffer(receivePacketSizeTaskResult.Value);

                    receivePacketTask = Task.Run(() => ReceivePacketAsync(socket));
                    receivePacketTaskResult = await receivePacketTask.ConfigureAwait(false);

                    if (receivePacketTaskResult.Failure)
                    {
                        logger.WriteLogEntry(receivePacketTaskResult.Error, ref m_systemMsg);
                        return;
                    }

                    buffer.BufferStream.Position = 0;

                    TransactionPacket packet = (TransactionPacket)FragmentationServices.
                        Deserialize(buffer.BufferStream.ToArray());

                    HandleAction(packet, socket);

                    buffer.Dispose();
                }
            }
            catch (ArgumentNullException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
            }
            catch (SocketException ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
            }
            catch (SerializationException ex)
            {
                if (m_client == null || m_closing)
                    return;

                receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));

                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
            }
            catch (Exception ex)
            {
                //m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
            }
        }


        /// <summary>
        /// Handling received actions 
        /// </summary>
        /// <param name="packet">Packet will be receive</param>
        /// <param name="socket">In CCore does not use this</param>
        public async void HandleAction(TransactionPacket packet, Socket socket)
        {
            if (m_closing) return;

            object[] param;

            switch (packet.Todo)
            {
                case (int)DoActions.Todo.PushLog:
                    break;
                case (int)DoActions.Todo.PushStatus:
                    if (!p2p)
                    {
                        if (typeof(string) == packet.Value.GetType() && packet.Value.ToString().Equals("clear"))
                        {
                            m_clients.Clear();
                            groupList.Clear();
                            await Task.Run(() => this.ClearListContainer?.Invoke());
                        }

                        if (typeof(object[]) == packet.Value.GetType())
                        {
                            param = packet.Value as object[];
                            m_clients.Add(new Client()
                            {
                                ID = Convert.ToInt32(param[0]),
                                Name = param[1].ToString(),
                                Online = (bool)param[2],
                                M_EndPoint = param[3] as EndPoint
                            });
                        }

                        if (typeof(string) == packet.Value.GetType() && packet.Value.ToString().Equals("sended"))
                        {
                            await Task.Run(() => this.ListChanged?.Invoke());
                        }
                    }
                    break;
                case (int)DoActions.Todo.PushMessage:
                    break;
                case (int)DoActions.Todo.PushOfflineMessage:
                    break;
                case (int)DoActions.Todo.PushOfflineGroupMessage:
                    break;
                case (int)DoActions.Todo.PushGroupList:
                    param = packet.Value as object[];

                    groupList.Add(new ChatGroup()
                    {
                        ID = Convert.ToInt32(param[0]),
                        GroupName = param[1].ToString(),
                    });
                    break;
                case (int)DoActions.MessageType.ServerSendAll:
                    m_userMsg += $"\r\nEndpoint: {packet.Value}";
                    break;
                case (int)DoActions.MessageType.ServerToSingleClient:
                    m_userMsg += $"\r\nEndpoint: {packet.Value}";
                    break;
                case (int)DoActions.MessageType.ClientToServer:
                    break;
                case (int)DoActions.MessageType.ClientToClient:
                    m_userMsg += $"\r\n{packet.Value}";
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
        public async Task SendPacket(byte[] packet, Socket socket)
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    handleSendTask = Task.Run(() => SendPacketAsync(packet, socket));
                    handleSendTaskResult = await handleSendTask.ConfigureAwait(false);

                    if (handleSendTaskResult.Failure)
                    {
                        logger.WriteLogEntry(handleSendTaskResult.Error, ref m_systemMsg);
                        return;
                    }
                }
            }
            finally
            {
                // SendOffMessageToDatabase
            }
        }

        private async void SendLog()
        {
            if (m_client != null && m_client.Connected && m_myClient != null)
            {
                await SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.Todo.PushLog, new object[]
                {
                    m_myClient.ID,
                    m_myClient.Name,
                })), m_client);
            }
        }
    }
}
