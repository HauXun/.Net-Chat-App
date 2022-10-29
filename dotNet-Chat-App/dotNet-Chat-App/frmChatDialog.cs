using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace dotNet_Chat_App
{
    public partial class frmChatDialog : Form
    {
        IPEndPoint remoteIpEndPoint;
        IPEndPoint localIpEndPoint;
        SCore sCore;
        CCore cCore;
        private System.Windows.Forms.Timer sTimer;
        private System.Windows.Forms.Timer cTimer;

        public Client ClientToSend
        {
            get;
            set;
        }

        private SCore m_sCore;

        public frmChatDialog(Socket server, Socket client)
        {
            InitializeComponent();
            localIpEndPoint = server.LocalEndPoint as IPEndPoint;
            remoteIpEndPoint = client.RemoteEndPoint as IPEndPoint;
        }

        public frmChatDialog(SCore m_sCore)
        {
            InitializeComponent();
            this.m_sCore = m_sCore;
        }

        private async void frmChatDialog_Load(object sender, EventArgs e)
        {
            if (this.m_sCore == null)
            {
                sCore = new SCore()
                {
                    IpAddress = remoteIpEndPoint.Address.ToString(),
                    Port = remoteIpEndPoint.Port,
                    P2P = true
                };
                sCore.Init();

                cCore = new CCore()
                {
                    IpAddress = localIpEndPoint.Address.ToString(),
                    Port = localIpEndPoint.Port,
                    P2P = true
                };
                cCore.Listen();

                sTimer = new Timer();
                sTimer.Tick += STimer_Tick;
                sTimer.Interval = 250;

                cTimer = new Timer();
                cTimer.Tick += CTimer_Tick;
                cTimer.Interval = 250;
            }
        }

        private void STimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sCore.UserMsg))
                tbMessage.Text += sCore.UserMsg;
            if (!string.IsNullOrEmpty(sCore.SystemMsg))
                tbSend.Text += sCore.SystemMsg;
            sCore.SystemMsg = sCore.UserMsg = string.Empty;
        }

        private void CTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cCore.UserMsg))
                tbMessage.Text += cCore.UserMsg;
            if (!string.IsNullOrEmpty(cCore.SystemMsg))
                tbSend.Text += cCore.SystemMsg;
            cCore.SystemMsg = cCore.UserMsg = string.Empty;
        }

        private void frmChatDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            CClose();
            SClose();
        }

        private void SClose()
        {
            if (this.m_sCore == null)
            {
                if (sCore.Listener != null && sCore.Listener.Connected)
                {
                    sTimer.Stop();
                    sCore.Closing = true;
                }
            }
        }

        private void CClose()
        {
            if (this.m_sCore == null)
            {
                if (cCore.Client != null && cCore.Client.Connected)
                {
                    cTimer.Stop();
                    cCore.Closing = true;
                }
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await this.m_sCore?.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.MessageType.ServerToSingleClient, this.tbSend.Text)), this.ClientToSend.M_Client);
        }
    }
}
