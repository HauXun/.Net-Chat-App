using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace dotNet_Chat_App
{
    public partial class frmServer : Form
    {
        private System.Windows.Forms.Timer m_msgTimer;
        private SCore m_sCore;
        private static Mutex mutex = new Mutex();

        public frmServer()
        {
            InitializeComponent();
            m_sCore = new SCore()
            {
                ClientListChanged = LoadClientList,
                ClearClientListContainer = ClearClientListContainer
            };
            m_sCore.IpAddress = IPServices.GetIPAddress();
            m_sCore.Init();
        }

        private void frmServer_Load(object sender, EventArgs e)
        {
            m_msgTimer = new System.Windows.Forms.Timer();

            m_msgTimer.Tick += M_msgTimer_Tick;
            m_msgTimer.Interval = 250;
            m_msgTimer.Start();
            GetClients();
        }

        private void M_msgTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_sCore.UserMsg))
                tbMessage.Text += m_sCore.UserMsg;
            if (!string.IsNullOrEmpty(m_sCore.SystemMsg))
                tbLog.Text += m_sCore.SystemMsg;

            lbOnlineTotal.Text = $"Online total: {m_sCore.OnlineClientCount}";
            m_sCore.SystemMsg = m_sCore.UserMsg = string.Empty;
        }

        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Closez();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await m_sCore.SendAll(new TransactionPacket((int)DoActions.MessageType.ServerSendAll, tbSend.Text));
        }

        private void Closez()
        {
            if (m_sCore.Listener != null && m_sCore.Listener.Connected)
            {
                m_msgTimer.Stop();
                m_sCore.Closing = true;
            }
        }

        private void ClearClientListContainer()
        {
            if (IsHandleCreated)
            {
                try
                {
                    mutex.WaitOne();
                    // Always synchronous.  (But you must watch out for cross-threading deadlocks!)
                    if (InvokeRequired)
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            int count = flpClientContainer.Controls.Count;
                            for (int i = count - 1; i > 0; i--)
                            {
                                this.flpClientContainer.Controls.RemoveAt(i);
                            }
                        }));
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private void LoadClientList()
        {
            if (IsHandleCreated)
            {
                try
                {
                    mutex.WaitOne();
                    // Always synchronous.  (But you must watch out for cross-threading deadlocks!)
                    if (InvokeRequired)
                        if (m_sCore != null && m_sCore.Clients != null && m_sCore.Clients.Count > 0)
                        {
                            foreach (Client client in m_sCore.Clients)
                            {
                                this.Invoke(new MethodInvoker(delegate ()
                                {
                                    ClientBox box = new ClientBox(client.Online, client.Name, new Tuple<Socket, Socket>(m_sCore.Listener, client.M_Client));
                                    box.Tag = client;
                                    box.lbName.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    box.pbStatus.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    box.pnlContainer.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    box.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                                    this.flpClientContainer.Controls.Add(box);
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

        private void GetClients()
        {
            if (m_sCore != null && m_sCore.Clients != null && m_sCore.Clients.Count > 0)
            {
                foreach (Client client in m_sCore.Clients)
                {
                    ClientBox box = new ClientBox(client.Online, client.Name, new Tuple<Socket, Socket>(m_sCore.Listener, client.M_Client));
                    box.Tag = client;
                    box.lbName.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                    box.pbStatus.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                    box.pnlContainer.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                    box.MouseDoubleClick += PnlContainer_MouseDoubleClick;
                    flpClientContainer.Controls.Add(box);
                }
            }
        }

        private static object GetParent<T>(Control control)
        {
            while (control.Parent != null)
            {
                if (control.Parent is T)
                {
                    return (T)(object)control.Parent;
                }
                control = control.Parent;
            }
            return null;
        }


        private void PnlContainer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ClientBox box = (ClientBox)GetParent<ClientBox>(sender as Control);
            //iterate through
            if (Application.OpenForms["frmChatDialog"] == null || Convert.ToInt32(Application.OpenForms["frmChatDialog"].Tag) != (box.Tag as Client).ID)
            {
                new frmChatDialog(m_sCore)
                {
                    ClientToSend = box.Tag as Client,
                    Tag = (box.Tag as Client).ID
                }.Show();
            }
            //new frmChatDialog(socket?.Item1, socket?.Item2).Show();
        }
    }
}
