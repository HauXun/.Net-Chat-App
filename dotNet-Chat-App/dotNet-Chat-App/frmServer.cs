using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
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
			m_msgTimer.Stop();
			m_sCore.Closing = true;
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			m_sCore.SendAll(new TransactionPacket((int)DoActions.Todo.PushMessage, tbSend.Text));
		}

		private void ClearClientListContainer()
		{
			try
			{
				mutex.WaitOne();
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

		private void LoadClientList()
		{
			try
			{
				mutex.WaitOne();
				if (m_sCore != null && m_sCore.Clients != null && m_sCore.Clients.Count > 0)
				{
					foreach (Client client in m_sCore.Clients)
					{
						this.Invoke(new MethodInvoker(delegate ()
						{
							this.flpClientContainer.Controls.Add(new ClientBox(client.Online, client.Name));
						}));
					}
				}
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}

		private void GetClients()
		{
			if (m_sCore != null && m_sCore.Clients != null && m_sCore.Clients.Count > 0)
			{
				foreach (Client client in m_sCore.Clients)
				{
					flpClientContainer.Controls.Add(new ClientBox(client.Online, client.Name));
				}
			}
		}
	}
}
