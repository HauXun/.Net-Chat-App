using dotNet_Chat_App.Model.BusinessLogicLayer;
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
    public partial class AddToGroup : Form
    {
        private List<Client> m_clients;
        private object[] clone;

        public AddToGroup(List<Client> m_clients)
        {
            InitializeComponent();
            lbToAdd.DisplayMember = "Name";
            lbToAdd.ValueMember = "ID";
            lbAdded.DisplayMember = "Name";
            lbAdded.ValueMember = "ID";

            this.m_clients = m_clients;
        }

        private void AddToGroup_Load(object sender, EventArgs e)
        {
            m_clients = ClientBLL.Instance.GetClients();
            foreach (var client in m_clients)
            {
                if (!client.Name.Equals("server"))
                    lbToAdd.Items.Add(client);
            }
            lbToAdd.SelectedIndex = 0;
        }

        private void btnFoward_Click(object sender, EventArgs e)
        {
            if (lbToAdd.SelectedIndex != -1)
            {
                if (!lbAdded.Items.Contains(lbToAdd.SelectedItem))
                {
                    lbAdded.Items.Add(lbToAdd.SelectedItem);
                    lbToAdd.Items.RemoveAt(lbToAdd.SelectedIndex);
                }
            }
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            if (lbAdded.SelectedIndex != -1)
            {
                if (!lbToAdd.Items.Contains(lbAdded.SelectedItem))
                {
                    lbToAdd.Items.Add(lbAdded.SelectedItem);
                    lbAdded.Items.RemoveAt(lbAdded.SelectedIndex);
                }
            }

        }

        private void btnFowardAll_Click(object sender, EventArgs e)
        {
            clone = new object[lbToAdd.Items.Count];
            lbToAdd.Items.CopyTo(clone, 0);
            foreach (var item in clone)
            {
                if (!lbAdded.Items.Contains(item))
                {
                    lbAdded.Items.Add(item);
                    lbToAdd.Items.Remove(item);
                }
            }
        }

        private void btnBackwardAll_Click(object sender, EventArgs e)
        {
            clone = new object[lbAdded.Items.Count];
            lbAdded.Items.CopyTo(clone, 0);
            foreach (var item in clone)
            {
                if (!lbToAdd.Items.Contains(item))
                {
                    lbToAdd.Items.Add(item);
                    lbAdded.Items.Remove(item);
                }
            }
        }
    }
}
