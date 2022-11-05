using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Model.BusinessLogicLayer;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private object receivePacketData;

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
                ReceiveRequestPacket = ReceiveRequestPacket
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
        }

        private void M_msgTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_cCore.UserMsg))
                tbMessage.Text += m_cCore.UserMsg;
            if (!string.IsNullOrEmpty(m_cCore.SystemMsg))
                tbSend.Text += m_cCore.SystemMsg;
            m_cCore.SystemMsg = m_cCore.UserMsg = string.Empty;
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
            if (m_cCore.Client != null && m_cCore.Client.Connected)
            {
                await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.MessageType.ClientToServer, message)), m_cCore.Client);
            }
            else
            {
                Closez();
                Init();
            }
        }

        private async void Closez()
        {
            if (m_cCore.Client != null)
            {
                // Send the close message for the endpoint
                await Task.Run(() => m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.Todo.PushMessage, $"The client closed")), m_cCore.Client));
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
                                ClientBox box = new ClientBox(client.Online, client.Name, new Tuple<Socket, Socket>(m_cCore.Client, client.M_Client));
                                this.flpClientContainer.Controls.Add(box);
                            }));
                        }

                        foreach (ChatGroup group in m_cCore.GroupList.ToList())
                        {
                            this.Invoke(new MethodInvoker(delegate ()
                            {
                                this.flpGroupContainer.Controls.Add(new ClientBox(group.GroupName) { Tag = group.ID});
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

        private void ReceiveRequestPacket(object data)
        {
            receivePacketData = data;
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
