using dotNet_Chat_App.Core;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.UserControls;
using System;
using System.Windows.Forms;

namespace dotNet_Chat_App
{
	public partial class frmServer : Form
	{
		private Timer m_msgTimer;
		private SCore m_sCore;

		public frmServer()
		{
			InitializeComponent();

			for (int i = 0; i < 5; i++)
			{
				this.flpClientContainer.Controls.Add(new ClientBox(
					Properties.Resources.Online));
				this.flpClientContainer.Controls.Add(new ClientBox(
					Properties.Resources.Offline));
			}
		}

		private void frmServer_Load(object sender, EventArgs e)
		{
			m_msgTimer = new Timer();
			m_sCore = new SCore();

			m_msgTimer.Tick += M_msgTimer_Tick;
			m_msgTimer.Interval = 250;
			m_msgTimer.Start();
			m_sCore.IpAddress = IPServices.GetIPAddress();
		}

		private void M_msgTimer_Tick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(m_sCore.UserMsg))
				tbSend.Text += m_sCore.UserMsg;
			if (!string.IsNullOrEmpty(m_sCore.SystemMsg))
				tbSend.Text += m_sCore.SystemMsg;
			m_sCore.SystemMsg = m_sCore.UserMsg = string.Empty;
		}

		private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_msgTimer.Stop();
			m_sCore.Closing = true;
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			m_sCore.SendOK();
		}
	}
}
