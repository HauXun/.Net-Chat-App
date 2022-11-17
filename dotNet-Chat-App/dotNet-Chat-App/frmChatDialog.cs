using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace dotNet_Chat_App
{
    public partial class frmChatDialog : Form
    {
        public Client ClientToSend { get; set; }
        private SCore m_sCore;
        private CCore m_cCore;
        private List<Message> listCacheMessage;

        public frmChatDialog(SCore m_sCore)
        {
            InitializeComponent();
            this.m_sCore = m_sCore;
        }

        public frmChatDialog(CCore m_cCore)
        {
            InitializeComponent();
            this.m_cCore = m_cCore;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (m_sCore != null)
            {
                this.m_sCore.HandleAction(new TransactionPacket((int)DoActions.MessageType.ServerToSingleClient, new object[]
                {
                    this.ClientToSend.ID,
                    this.ClientToSend.Online,
                    this.tbSend.Text
                }), this.ClientToSend.M_Client);
            }
            else
            {
                // Internet connection checking
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.MessageType.ClientToClient, new object[]
                    {
                    this.m_cCore.MyClient.ID,
                    this.ClientToSend.ID,
                    $"{this.m_cCore.MyClient.Name}: {this.tbSend.Text}"
                    })), m_cCore.Client);
                }
                else
                {
                    if (listCacheMessage == null)
                        listCacheMessage = new List<Message>();

                    Message messageToCahe = new Message()
                    {
                        DetailMessage = FragmentationServices.Serialize($"{this.m_cCore.MyClient.Name}: {this.tbSend.Text}"),
                        ClientSent = this.m_cCore.MyClient.ID,
                        ClientReceiver = this.ClientToSend.ID,
                        MessageType = (int)DoActions.MessageType.ClientToClient
                    };

                    listCacheMessage.Add(messageToCahe);
                    JsonServices.JSONEncode(listCacheMessage);
                }
            }
        }

        private async void frmChatDialog_Load(object sender, EventArgs e)
        {
            this.Text = $"Chat Dialog - {this.ClientToSend.Name}";

            await m_cCore.SendPacket(FragmentationServices.Serialize(new TransactionPacket((int)DoActions.Todo.PushOfflineMessage, new object[]
            {
                    this.ClientToSend.ID,
                this.m_cCore.MyClient.ID,
            })), m_cCore.Client);
        }
    }
}
