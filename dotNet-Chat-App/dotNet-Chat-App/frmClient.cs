using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotNet_Chat_App
{
    public partial class frmClient : Form
    {
        private System.Windows.Forms.Timer m_msgTimer;
        private CCore m_cCore;
        public frmClient(Client client)
        {
            InitializeComponent();

            m_cCore = new CCore(ClientListChanged);
            m_cCore.MyClient = client;
            this.Text += $" - {client.Name}";
        }


        private void frmClient_Load(object sender, EventArgs e)
        {
            m_msgTimer = new System.Windows.Forms.Timer();

            m_msgTimer.Tick += M_msgTimer_Tick; ;
            m_msgTimer.Interval = 250;
            m_msgTimer.Start();
            m_cCore.IpAddress = IPServices.GetIPAddress();
            m_cCore.Listen();
        }

        private void M_msgTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_cCore.UserMsg))
                tbSend.Text += m_cCore.UserMsg;
            if (!string.IsNullOrEmpty(m_cCore.SystemMsg))
                tbSend.Text += m_cCore.SystemMsg;
            m_cCore.SystemMsg = m_cCore.UserMsg = string.Empty;
        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            Closez();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send(tbSend.Text);
            Send(tbSend.Text);
        }

        private async void Send(string message)
        {
            await m_cCore.SendPacket(new TransactionPacket((int)DoActions.Todo.PushMessage, message));
        }

        private async void SendLog()
        {
            if (m_cCore.MyClient != null)
                await m_cCore.SendPacket(new TransactionPacket((int)DoActions.Todo.PushLog, m_cCore.MyClient));
        }

        private async void Closez()
        {
            // Send the close message for the endpoint
            await Task.Run(() => m_cCore.SendPacket(new TransactionPacket((int)DoActions.Todo.PushMessage, $"The client closed")));
            m_msgTimer.Stop();
            m_cCore.Closing = true;
        }
        private void ClientListChanged()
        {
            List<Client> clientList = m_cCore.Clients;
            if (clientList != null && clientList.Count > 0)
            {
                foreach (Client item in clientList)
                {
                    this.flpClientContainer.Controls.Add(new ClientBox(item.Online, item.Name));
                }
            }

        }

        public void setTimeout(Action TheAction, int Timeout)
        {
            Thread t = new Thread(
                () =>
                {
                    Thread.Sleep(Timeout);
                    TheAction.Invoke();
                }
            );
            t.Start();
            t.Join();

            t.Interrupt();
            t.Abort();

            t = null;
        }

        private void lbTitle_Click(object sender, EventArgs e)
        {
            setTimeout(SendLog, 3000);
        }
    }
}
