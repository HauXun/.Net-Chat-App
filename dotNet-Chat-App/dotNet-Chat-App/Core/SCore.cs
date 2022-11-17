using dotNet_Chat_App.Common;
using dotNet_Chat_App.Config;
using dotNet_Chat_App.IAction;
using dotNet_Chat_App.IProps;
using dotNet_Chat_App.Model.BusinessLogicLayer;
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
using System.Windows.Forms;

namespace dotNet_Chat_App.Core
{
    public class SCore : IActions, ITodo, ICommunicateSocket, ISCommunicate, IReceivePacket, ISendPacket, ITranspotDefine, IDelegateDefined
    {
        const int ReceiveTimeoutMs = 3000;
        const int SendTimeoutMs = 3000;

        #region Properties

        private Socket m_listener;
        private Thread m_thread;
        private Socket m_lastClient;
        private List<Socket> m_clientSockets = new List<Socket>();
        private int tokenID;

        private Task<TAPResultPattern<Socket>> acceptTask;
        private TAPResultPattern<Socket> acceptResult;

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
        private bool reLog = true;

        #endregion

        #region EncapsulationProperties

        public Socket Listener { get => m_listener; private set { } }
        public Thread Thread { get => m_thread; private set { } }
        public Socket LastClient { get => m_lastClient; private set { } }
        public List<Socket> ClientSockets { get => m_clientSockets; private set { } }
        public int TokenID { get => tokenID; set => tokenID = value; }

        public Task<TAPResultPattern<Socket>> AcceptTask { get => acceptTask; private set { } }
        public TAPResultPattern<Socket> AcceptResult { get => acceptResult; private set { } }

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
        public ReceiveRequestPacket ReceiveRequestPacket { get => receiveRequestPacket; set => receiveRequestPacket = value; }

        private static ILogger logger = new GUILogger();
        private CancellationTokenSource ts;
        private CancellationToken token;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SCore()
        {
            lenBuffer = new byte[4];
            buffer = new ReceiveBuffer(0);
            ts = new CancellationTokenSource();
            token = ts.Token;
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

            if (receiveResult.Failure)
            {
                if (m_closing)
                    return TAPResultPattern.Fail<int>("\r\nError reading packet from client, server was close");

                if (bytesReceived <= 0)
                    return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet from {socket.RemoteEndPoint}, the client may have been closed");

                if (bytesReceived != 4)
                    return TAPResultPattern.Fail<int>($"\r\nError reading packet from {socket.RemoteEndPoint}, format does not match");
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
                return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet from {socket.RemoteEndPoint}, the client may have been closed");
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
                    return TAPResultPattern.Fail<int>("\r\nError reading packet, client was close");

                if (bytesReceived <= 0)
                    return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet from {socket.RemoteEndPoint}, the client may have been closed");

                if (bytesReceived != 4)
                    return TAPResultPattern.Fail<int>($"\r\nError reading packet from {socket.RemoteEndPoint}, format does not match");
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
                            return TAPResultPattern.Fail<int>("\r\nError reading packet, server was close");

                        if (bytesReceived <= 0)
                            return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet from {socket.RemoteEndPoint}, the client may have been closed");

                        if (bytesReceived != 4)
                            return TAPResultPattern.Fail<int>($"\r\nError reading packet from {socket.RemoteEndPoint}, packet format does not match");
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
                        return TAPResultPattern.Fail<int>("\r\nError reading packet, server was close");

                    if (bytesReceived <= 0)
                        return TAPResultPattern.Fail<int>($"\r\nUnable to receive packet from {socket.RemoteEndPoint}, the client may have been closed");

                    if (bytesReceived != 4)
                        return TAPResultPattern.Fail<int>($"\r\nError reading packet from {socket.RemoteEndPoint}, packet format does not match");
                }
            }

            return TAPResultPattern.Ok(bytesReceived);
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
            GetClients();
        }

        /// <summary>
        /// Close the server and the connection comes from the clients
        /// </summary>
        public void Close()
        {
            m_listener?.Close();
            m_listener = null;
            ts.Cancel();

            m_clientSockets.ForEach(x => x.Close());

            m_lastClient?.Close();

            m_lastClient = null;
            m_clientSockets = null;
            m_thread.Interrupt();
            m_thread.Abort();
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
                logger.WriteLogEntry("\r\nServer listener started", ref m_systemMsg);
            }
            catch (Exception ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
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
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
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
                if (!p2p)
                {
                    LogServerMessagge();

                    AsynchronousServices.setInterval(() =>
                    {
                        ClearDisconnectClients();
                        PushState();
                        FetchMessage();
                    }, TimeSpan.FromSeconds(10), this.token);
                }
                while (m_listener != null)
                {
                    // The call to AcceptConnectionTask is not awaited, therefore this method
                    // continues executing
                    acceptTask = Task.Run(AcceptConnectionTask);
                    // Await the result of the ServerAcceptTask
                    acceptResult = await acceptTask.ConfigureAwait(false);

                    if (acceptResult.Failure)
                    {
                        logger.WriteLogEntry($"\r\n{acceptResult.Error}", ref m_systemMsg);
                        logger.WriteLogEntry("\r\nThere was an error connecting to the server/accepting connection from the client", ref m_systemMsg);

                        //m_systemMsg += $"{acceptResult.Error}";
                        //throw new Exception("There was an error connecting to the server/accepting connection from the client");
                    }

                    // Store the transfer socket if ServerAcceptTask was successful
                    m_lastClient = acceptResult.Value;

                    if (m_lastClient != null)
                    {
                        m_clientSockets?.Add(m_lastClient);

                        Thread handle = new Thread(WaitForHandle);

                        handle.IsBackground = true;
                        handle.Start(m_lastClient);
                        handle.Join();
                    }
                }
            }
            catch (Exception ex)
            {
                // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
            }
        }

        /// <summary>
        /// Remove disconnected sockets from the list
        /// </summary>
        private void ClearDisconnectClients()
        {
            foreach (Socket s in m_clientSockets.ToList())
            {
                if (s != null && !s.Connected)
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

            logger.WriteLogEntry($"\r\n{(socket.RemoteEndPoint as IPEndPoint).Port} Connected", ref m_systemMsg);
            //m_systemMsg += $"\r\n\r\n{(socket.RemoteEndPoint as IPEndPoint).Port} Connected";

            while (m_listener != null)
            {
                try
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
                catch (SocketException ex)
                {
                    switch (ex.SocketErrorCode)
                    {
                        case SocketError.ConnectionAborted:
                        case SocketError.ConnectionReset:
                            // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                            logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
                            break;
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                    logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
                }
                catch (IOException ex)
                {
                    // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                    logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
                }
                catch (SerializationException ex)
                {
                    if (m_listener == null || m_closing)
                        return;

                    receivePacketSizeTask = Task.Run(() => ReceivePacketSizeAsync(socket));

                    // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                    logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
                }
                catch (Exception ex)
                {
                    // m_systemMsg += $"\r\n{ex.Message} {ex.GetType()}";
                    logger.WriteLogEntry("\r\n" + ex.Message, ref m_systemMsg);
                }
            }
        }

        /// <summary>
        /// Handling received actions 
        /// </summary>
        /// <param name="packet">Packet will be receive</param>
        public async void HandleAction(TransactionPacket packet, Socket socket)
        {
            if (m_closing) return;

            object[] param;

            switch (packet.Todo)
            {
                case (int)DoActions.Todo.PushLog:
                    param = packet.Value as object[];
                    m_userMsg += $"\r\n{param[1]} was logging";

                    foreach (Client x in m_clients)
                    {
                        if (x.ID == Convert.ToInt32(param[0]))
                        {
                            x.Online = true;
                            x.M_Client = socket;
                            ClientBLL.Instance.SaveStatus(x.ID, Convert.ToInt32(x.Online));

                            PushMessage(x);

                            break;
                        }
                    }

                    groupList = ChatGroupBLL.Instance.GetGroupList();

                    PushState();
                    break;
                case (int)DoActions.Todo.PushMessage:
                    break;
                case (int)DoActions.Todo.PushStatus:
                    break;
                case (int)DoActions.Todo.PushOfflineMessage:
                    param = packet.Value as object[];

                    PushOfflineMessage(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
                    break;
                case (int)DoActions.Todo.PushOfflineGroupMessage:
                    break;
                case (int)DoActions.Todo.PushSyncAddGroup:
                    param = packet.Value as object[];

                    int idGroup = ChatGroupBLL.Instance.AddGroup(param[0].ToString());
                    if (idGroup > 0)
                    {
                        foreach (var clientID in param[1] as List<int>)
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(0.5));
                            ClientChatGroupBLL.Instance.SaveClientToGroup(idGroup, clientID);
                        }
                    }
                    break;
                case (int)DoActions.Todo.PushGroupList:
                    groupList = ChatGroupBLL.Instance.GetGroupList();
                    break;
                case (int)DoActions.Todo.PushLogout:
                    ClientBLL.Instance.SaveStatus(Convert.ToInt32(packet.Value), Convert.ToInt32(false));
                    break;
                case (int)DoActions.MessageType.ServerSendAll:
                    m_userMsg += $"\r\n{packet.Value}";
                    break;
                case (int)DoActions.MessageType.ServerToSingleClient:
                    param = packet.Value as object[];

                    await SendPacket(FragmentationServices.Serialize(packet), socket);

                    if (!reLog)
                        MessageBLL.Instance.SaveMessage(packet, Convert.ToInt32(param[1]), Convert.ToInt32(param[0]), tokenID);
                    break;
                case (int)DoActions.MessageType.ClientToServer:
                    param = packet.Value as object[];

                    m_userMsg += $"\r\n{param[1]}";

                    if (!reLog)
                        MessageBLL.Instance.SaveMessage(packet, Convert.ToInt32(m_listener.Connected), tokenID, Convert.ToInt32(param[0]));
                    break;
                case (int)DoActions.MessageType.ClientToClient:
                    param = packet.Value as object[];

                    foreach (Client x in m_clients)
                    {
                        if (x.ID == Convert.ToInt32(param[1]))
                        {
                            if (!reLog)
                                MessageBLL.Instance.SaveMessage(packet, Convert.ToInt32(x.Online), Convert.ToInt32(param[1]), Convert.ToInt32(param[0]));

                            if (x.M_Client != null && x.M_Client.Connected)
                            {
                                await SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.MessageType.ClientToClient, $"{param[2]}")), x.M_Client);
                            }
                        }
                    }
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
        /// Send the packet to the clients
        /// </summary>
        /// <param name="packet">Packet will be send</param>
        public async Task SendPacket(byte[] packet, Socket socket)
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

        public async Task SendAll(TransactionPacket packet)
        {
            try
            {
                await Task.WhenAll(m_clientSockets?.Select(x => SendPacket(FragmentationServices.Serialize(packet), x)));
            }
            finally
            {
                if (packet.Todo >= (int)DoActions.MessageType.ServerSendAll && !reLog)
                {
                    m_clients.ForEach(x =>
                    {
                        if (x.ID != tokenID)
                            MessageBLL.Instance.SaveMessage(packet, Convert.ToInt32(x.Online), x.ID, tokenID);
                    });
                }
            }
        }

        private Task SendClients(Client client)
        {
            return SendAll(new TransactionPacket((int)DoActions.Todo.PushStatus, new object[]
                {
                    client.ID,
                    client.Name,
                    client.Online,
                    client.M_Client?.RemoteEndPoint,
                }));
        }

        private Task SendGroupList(ChatGroup group)
        {
            return SendAll(new TransactionPacket((int)DoActions.Todo.PushGroupList, new object[]
                {
                    group.ID,
                    group.GroupName
                }));
        }

        private void GetClients()
        {
            if (!p2p)
            {
                m_clients = ClientBLL.Instance.GetClients();
            }
        }

        private async void PushState()
        {
            if (!p2p)
            {
                onlineClientCount = 0;
                await SendAll(new TransactionPacket((int)DoActions.Todo.PushStatus, "clear"));
                await Task.Run(() => this.ClearListContainer?.Invoke());

                foreach (Client client in m_clients.ToList())
                {
                    if (client != null && (client.M_Client != null && !client.M_Client.Connected) || (client.M_Client == null))
                        client.Online = false;

                    if (client.Online)
                        onlineClientCount++;

                    await SendClients(client);

                    ClientBLL.Instance.SaveStatus(client.ID, Convert.ToInt32(client.Online));

                    await Task.Delay(TimeSpan.FromMilliseconds(0.5));
                }

                foreach (ChatGroup group in groupList)
                {
                    await SendGroupList(group);
                    await Task.Delay(TimeSpan.FromMilliseconds(0.5));
                }

                await SendAll(new TransactionPacket((int)DoActions.Todo.PushStatus, "sended"));
                await Task.Run(() => this.ListChanged?.Invoke());
            }
        }

        private void LogServerMessagge()
        {
            List<Message> messages = MessageBLL.Instance.GetAllMessage();

            if (messages == null)
                return;

            foreach (Message message in messages)
            {
                if (message.ClientSent.Equals(tokenID))
                {
                    reLog = true;
                    HandleAction((TransactionPacket)FragmentationServices.Deserialize(message.DetailMessage), Listener);
                    reLog = false;

                    if (!message.Sent)
                    {
                    reSend:
                        if (!MessageBLL.Instance.FecthMessageStatus((int)message.ID, Convert.ToInt32(true)))
                            goto reSend;
                    }
                }
            }
        }

        private async void PushMessage(Client client)
        {
            List<Message> messages = MessageBLL.Instance.GetAllMessage();

            if (messages == null)
                return;

            foreach (Message message in messages)
            {
                if ((message.ClientSent == tokenID && client.ID == message.ClientReceiver) || (client.ID == message.ClientSent && message.ClientReceiver != tokenID))
                {
                    await SendPacket(message.DetailMessage, client.M_Client);

                    if (!message.Sent)
                    {
                    reSend:
                        if (!MessageBLL.Instance.FecthMessageStatus((int)message.ID, Convert.ToInt32(true)))
                            goto reSend;
                    }
                }
            }
        }

        private async void PushOfflineMessage(int clientSent, int clientRec)
        {
            List<Message> messages = MessageBLL.Instance.GetAllMessage();

            if (messages == null)
                return;

            foreach (Message message in messages)
            {
                if (clientSent == message.ClientSent && message.ClientReceiver == clientRec)
                {
                    foreach (Client client in m_clients)
                    {
                        if (client.ID == clientRec && client != null && client.M_Client != null && client.M_Client.Connected)
                        {
                            await SendPacket(message.DetailMessage, client.M_Client);

                            if (!message.Sent)
                            {
                            reSend:
                                if (!MessageBLL.Instance.FecthMessageStatus((int)message.ID, Convert.ToInt32(true)))
                                    goto reSend;
                            }
                        }
                    }
                }
            }
        }

        private async void FetchMessage()
        {
            List<Message> messages = MessageBLL.Instance.GetAllMessageUnsent();

            if (messages == null)
                return;

            foreach (Message message in messages)
            {
                if (!message.Sent)
                {
                    if (message.ClientReceiver.Equals(tokenID))
                    {
                        reLog = true;
                        HandleAction((TransactionPacket)FragmentationServices.Deserialize(message.DetailMessage), Listener);
                        reLog = false;

                        if (!message.Sent)
                        {
                        reSend:
                            if (!MessageBLL.Instance.FecthMessageStatus((int)message.ID, Convert.ToInt32(true)))
                                goto reSend;
                        }

                        continue;
                    }

                    foreach (Client x in m_clients)
                    {
                        if (x.ID == message.ClientReceiver && x.Online && x.M_Client != null && x.M_Client.Connected)
                        {
                            await SendPacket(message.DetailMessage, x.M_Client);

                        reSend:
                            if (!MessageBLL.Instance.FecthMessageStatus((int)message.ID, Convert.ToInt32(true)))
                                goto reSend;
                        }
                    }
                }
            }
        }
    }
}
