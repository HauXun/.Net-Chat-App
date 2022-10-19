using dotNet_Chat_App.Common;
using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotNet_Chat_App
{
	public partial class frmClient : Form
	{
		private Timer m_msgTimer;
		private CCore m_cCore;
		public frmClient()
		{
			InitializeComponent();

			for (int i = 0; i < 5; i++)
			{
				this.flpClientContainer.Controls.Add(new ClientBox(
					Properties.Resources.Online));
				this.flpClientContainer.Controls.Add(new ClientBox(
					Properties.Resources.Offline));
			}

			for (int i = 0; i < 5; i++)
			{
				this.flpGroupContainer.Controls.Add(new ClientBox(
					Properties.Resources.Online));
				this.flpGroupContainer.Controls.Add(new ClientBox(
					Properties.Resources.Offline));
			}
		}

		private void frmClient_Load(object sender, EventArgs e)
		{
			m_msgTimer = new Timer();
			m_cCore = new CCore();

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
			m_cCore.SendPacket(new TransactionPacket(0, "laidfshiahfia"));
		}
		
		private async void Closez()
		{
			await Task.Run(() => m_cCore.SendClose());
			m_msgTimer.Stop();
			m_cCore.Closing = true;
		}
	}
}
