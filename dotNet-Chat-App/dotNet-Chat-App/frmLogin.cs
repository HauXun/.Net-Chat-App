using dotNet_Chat_App.Model.BusinessLogicLayer;
using dotNet_Chat_App.Model.DataAccessLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotNet_Chat_App
{
    public partial class frmLogin : Form
    {
        static string Id;
        static string passWord;
        const int SERVER_ID = 1;
        static frmServer frm_server;
        static frmClient frm_client;


        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Id = txbUserName.Text;
            passWord = txbPass.Text;
            Client client = IsLogin(Id, passWord);

            if (client.ID > 0 && client.ID != SERVER_ID)
            {
                frm_client = new frmClient(client);
                frm_client.FormClosed += Client_FormClosed;
                this?.Hide();
                frm_client?.Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
            }
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            this?.Show();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {

            Id = txbUserName.Text;
            passWord = txbPass.Text;
            Client client = IsLogin(Id, passWord);

            if (client.ID > 0 && client.ID == SERVER_ID)
            {
                frm_server = new frmServer();
                frm_server.FormClosed += Server_FormClosed;
                this?.Hide();
                frm_server?.Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
            }
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            this?.Show();

        }

        private Client IsLogin(string Id, string passWord)
        {
            return ClientBLL.Instance.IsLogin(Id, passWord);
        }
    }
}
