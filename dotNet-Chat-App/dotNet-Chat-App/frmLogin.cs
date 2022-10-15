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
        string Id;
        string passWord;
        const int SERVER_ID = 1;
        
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Id = txbUserName.Text;
            passWord = txbPass.Text;
            int ID = IsLogin(Id, passWord);

            if (ID > 0 && ID != SERVER_ID)
            {
                new frmClient().Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
            }
        }

        private void btnServer_Click(object sender, EventArgs e)
        {

            Id = txbUserName.Text;
            passWord = txbPass.Text;
            int ID = IsLogin(Id, passWord);

            if (ID > 0 && ID == SERVER_ID)
            {
                new frmServer().Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
            }
        }
        private int IsLogin(string Id, string passWord)
        {
            return ClientBLL.Instance.IsLogin(Id, passWord);
        }
    }
}
