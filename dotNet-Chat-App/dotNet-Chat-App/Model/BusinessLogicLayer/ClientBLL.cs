using dotNet_Chat_App.Model.DataAccessLayer;
using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Model.BusinessLogicLayer
{
    public class ClientBLL
    {
        private volatile static ClientBLL instance;

        private ClientBLL()
        {

        }
        public static ClientBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientBLL();
                return instance;
            }
            set => instance = value;
        }


        public Client IsLogin(string Id, string passWord)
        {
            DataTable data = ClientDAL.Instance.IsLogin(Id, passWord);

            if (data != null && data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    return new Client(row);
                }
            }
            return null;
        }

        public List<Client> GetClients()
        {
            DataTable data = ClientDAL.Instance.GetClients();
            

            if (data != null & data.Rows.Count > 0)
            {
                List<Client> clients = new List<Client>();
                foreach (DataRow item in data.Rows)
                {
                    clients.Add(new Client(item));
                }
                return clients;
            }

            return null;
        }
        public bool SaveStatus(int id, int status)
        {
            return ClientDAL.Instance.SaveStatus(id, status);
        }
    }
}
