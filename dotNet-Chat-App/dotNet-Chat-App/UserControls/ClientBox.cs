using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotNet_Chat_App.UserControls
{
    public partial class ClientBox : UserControl
    {
        public ClientBox(bool status, string name)
        {
            InitializeComponent();
            if (status)
            {
                this.pbStatus.BackgroundImage = Properties.Resources.Online;
            }
            else
            {
                this.pbStatus.BackgroundImage = Properties.Resources.Offline;
            }
            lbName.Text = name;
        }

        private void pnlContainer_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.WhiteSmoke;
        }

        private void pnlContainer_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }
    }
}
