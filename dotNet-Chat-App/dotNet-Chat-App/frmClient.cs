using dotNet_Chat_App.UserControls;
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
    public partial class frmClient : Form
    {
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
    }
}
