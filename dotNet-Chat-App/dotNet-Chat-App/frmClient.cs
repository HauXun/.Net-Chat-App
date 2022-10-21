using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
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

			m_cCore = new CCore()
			{
				ClientListChanged = LoadClientList,
				ClearClientListContainer = ClearClientListContainer
			};

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
			SendLog();
		}

		private void M_msgTimer_Tick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(m_cCore.UserMsg))
				tbMessage.Text += m_cCore.UserMsg;
			m_cCore.SystemMsg = m_cCore.UserMsg = string.Empty;
		}

		private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
		{
			Closez();
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			Send($"{m_cCore.MyClient.Name}: {tbSend.Text}");
		}

		private async void Send(string message)
		{
			await m_cCore.SendPacket(new TransactionPacket((int)DoActions.Todo.PushMessage, message));
		}

		private async void SendLog()
		{
			if (m_cCore.MyClient != null)
			{
				await m_cCore.SendPacket(new TransactionPacket((int)DoActions.Todo.PushLog, new object[]
				{
					m_cCore.MyClient.ID,
					m_cCore.MyClient.Name,
				}));
			}
		}

		private async void Closez()
		{
			// Send the close message for the endpoint
			await Task.Run(() => m_cCore.SendPacket(new TransactionPacket((int)DoActions.Todo.PushMessage, $"The client closed")));
			m_msgTimer.Stop();
			m_cCore.Closing = true;
		}

		private void ClearClientListContainer()
		{
			this.Invoke(new MethodInvoker(delegate ()
			{
				int count = flpClientContainer.Controls.Count;
				for (int i = count - 1; i > 0; i--)
				{
					this.flpClientContainer.Controls.RemoveAt(i);
				}
			}));
		}

		private void LoadClientList(Client client)
		{
			this.Invoke(new MethodInvoker(delegate ()
			{
				this.flpClientContainer.Controls.Add(new ClientBox(client.Online, client.Name));
			}));
		}
	}
}
