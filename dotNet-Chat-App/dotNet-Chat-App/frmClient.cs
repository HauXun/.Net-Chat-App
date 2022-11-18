using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Model.BusinessLogicLayer;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Net.NetworkInformation;

namespace dotNet_Chat_App
{
    public partial class frmClient : Form
    {
        private System.Windows.Forms.Timer m_msgTimer;
        private CCore m_cCore;
        private static Mutex mutex = new Mutex();
        private Client client;
        private AddToGroup toGroup;
        private List<int> cloneID;
        private frmChatDialog chatDialog;
        private List<Message> listCacheMessage;

        public frmClient(Client client)
        {
            InitializeComponent();
            this.client = client;
        }

        private void Init()
        {
            m_cCore = new CCore()
            {
                ListChanged = LoadList,
                ClearListContainer = ClearListContainer,
            };

            m_cCore.MyClient = this.client;
            this.Text = $"Messenger Fakke - {this.client.Name}";

            m_msgTimer = new System.Windows.Forms.Timer();

            m_msgTimer.Tick += M_msgTimer_Tick;
            m_msgTimer.Interval = 250;
            m_msgTimer.Start();
            m_cCore.IpAddress = IPServices.GetIPAddress();
            m_cCore.Listen();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            Init();
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        private async void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                List<Message> messages = JsonServices.JSONDecode();

                foreach (Message message in messages.ToList())
                {
                    switch (message.MessageType)
                    {
                        case (int)DoActions.MessageType.ClientToServer:
                            if (m_cCore.Client != null && m_cCore.Client.Connected)
                            {
                                await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)message.MessageType, new object[]
                                {
                                    message.ClientSent,
                                    FragmentationServices.Deserialize(message.DetailMessage),
                                })), m_cCore.Client);

                                messages.Remove(message);
                            }
                            break;
                        case (int)DoActions.MessageType.ClientToClient:
                            await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)message.MessageType, new object[]
                            {
                                message.ClientSent,
                                message.ClientReceiver,
                                FragmentationServices.Deserialize(message.DetailMessage),
                            })), m_cCore.Client);

                            messages.Remove(message);
                            break;
                    }

                    JsonServices.JSONEncode(messages);
                }
            }
        }

        private void M_msgTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_cCore.UserMsg))
                tbMessage.Text += m_cCore.UserMsg;
            if (!string.IsNullOrEmpty(m_cCore.SystemMsg))
                tbSend.Text += m_cCore.SystemMsg;
            if (!string.IsNullOrEmpty(m_cCore.P2PMsg) && chatDialog != null)
                chatDialog.tbMessage.Text += m_cCore.P2PMsg;

            m_cCore.SystemMsg = m_cCore.UserMsg = m_cCore.P2PMsg = string.Empty;
        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.Parent = null;
            e.Cancel = true;
            Closez();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send($"{m_cCore.MyClient.Name}: {tbSend.Text}");
        }

        private async void Send(string message)
        {
            // Internet connection checking
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (m_cCore.Client != null && m_cCore.Client.Connected)
                {
                    await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.MessageType.ClientToServer, new object[]
                    {
                        m_cCore.MyClient.ID,
                        message,
                    })), m_cCore.Client);
                }
                else
                {
                    Closez();
                    Init();
                }
            }
            else
            {
                if (listCacheMessage == null)
                    listCacheMessage = new List<Message>();

                listCacheMessage.Add(new Message()
                {
                    DetailMessage = FragmentationServices.Serialize(message),
                    ClientSent = m_cCore.MyClient.ID,
                    MessageType = (int)DoActions.MessageType.ClientToServer
                });
                JsonServices.JSONEncode(listCacheMessage);
            }
        }

        private async void Closez()
        {
            if (m_cCore.Client != null)
            {
                // Send the close message for the endpoint
                await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.Todo.PushLogout, m_cCore.MyClient.ID)), m_cCore.Client);
                m_msgTimer.Stop();
                m_cCore.Closing = true;
            }
        }

        private void ClearListContainer()
        {
            if (IsHandleCreated)
            {
                try
                {
                    mutex.WaitOne();
                    // Always synchronous.  (But you must watch out for cross-threading deadlocks!)
                    if (!InvokeRequired)
                        return;

                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        if (m_cCore.Closing || this.IsDisposed || this.Disposing)
                            return;

                        int count = flpClientContainer.Controls.Count;
                        for (int i = count - 1; i > 0; i--)
                        {
                            this.flpClientContainer.Controls.RemoveAt(i);
                        }

                        count = flpGroupContainer.Controls.Count;
                        for (int i = count - 1; i > 0; i--)
                        {
                            this.flpGroupContainer.Controls.RemoveAt(i);
                        }
                    }));
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private void LoadList()
        {
            if (IsHandleCreated)
            {
                try
                {
                    mutex.WaitOne();
                    // Always synchronous.  (But you must watch out for cross-threading deadlocks!)
                    if (!InvokeRequired)
                        return;

                    if (m_cCore != null && m_cCore.Clients != null && m_cCore.Clients.Count > 0)
                    {
                        if (m_cCore.Closing || this.IsDisposed || this.Disposing)
                            return;

                        foreach (Client client in m_cCore.Clients.ToList())
                        {
                            this.Invoke(new MethodInvoker(delegate ()
                            {
                                if (client != null && !client.ID.Equals(m_cCore.MyClient.ID) && !client.Name.Equals("server"))
                                {
                                    ClientBox box = new ClientBox(client.Online, client.Name);
                                    box.Tag = client;
                                    box.lbName.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    box.pbStatus.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    box.pnlContainer.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    box.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    this.flpClientContainer.Controls.Add(box);
                                }
                            }));
                        }

                        foreach (ChatGroup group in m_cCore.GroupList.ToList())
                        {
                            this.Invoke(new MethodInvoker(delegate ()
                            {
                                this.flpGroupContainer.Controls.Add(new ClientBox(group.GroupName) { Tag = group.ID });
                            }));
                        }
                    }
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private void PnlContainer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ClientBox box = (ClientBox)ControlServices.GetParent<ClientBox>(sender as Control);
            Client client = box.Tag as Client;

            //iterate through
            if (Application.OpenForms["frmChatDialog"] == null || Convert.ToInt32(Application.OpenForms["frmChatDialog"].Tag) != (box.Tag as Client).ID)
            {
                chatDialog = new frmChatDialog(m_cCore)
                {
                    ClientToSend = client,
                    Tag = client.ID,
                };
                chatDialog.Show();
            }
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            toGroup = new AddToGroup(m_cCore.Clients);
            toGroup.btnAdd.Click += BtnAdd_Click;
            toGroup.FormClosing += ToGroup_FormClosing;
            toGroup.Show();
        }

        private void ToGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            toGroup.Parent = null;
            toGroup.Dispose();
            toGroup = null;
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            if (toGroup?.lbAdded.Items.Count > 0)
            {
                cloneID = new List<int>(toGroup.lbAdded.Items.Count);
                foreach (var item in toGroup.lbAdded.Items)
                {
                    cloneID.Add((item as Client).ID);
                }

                await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.Todo.PushSyncAddGroup, new object[]
                {
                    toGroup.tbGroupName.Text.Trim(),
                    cloneID
                })), m_cCore.Client);

                await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.Todo.PushGroupList, "give me some nude")), m_cCore.Client);
            }
            toGroup?.Close();
        }
    }
}
