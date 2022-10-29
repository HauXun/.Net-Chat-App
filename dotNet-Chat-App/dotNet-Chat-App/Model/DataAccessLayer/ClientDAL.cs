using dotNet_Chat_App.Model.BusinessLogicLayer;
using dotNet_Chat_App.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Model.DataAccessLayer
{
    public class ClientDAL
    {
        private volatile static ClientDAL instance;

        private ClientDAL()
        {

        }
        public static ClientDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientDAL();
                return instance;
            }
            set => instance = value;
        }

        public Client IsLogin(string Id, string passWord)
        {
            byte[] tmp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] convertMD5 = new MD5CryptoServiceProvider().ComputeHash(tmp);
            string pass = String.Join("", convertMD5);

            string query = $"SELECT * FROM dbo.Clients WHERE ID = N'{Id}' AND Password = N'{pass}'";
            DataTable data = (DataProvider.Instance.ExcuteQuery(query));

            foreach (DataRow row in data.Rows)
            {
                return new Client(row);
            }
            return null;
        }

        public List<Client> GetClients()
        {
            string query = "SELECT * FROM dbo.Clients";
            DataTable data = (DataProvider.Instance.ExcuteQuery(query));
            List<Client> clients = new List<Client>();
            foreach (DataRow item in data.Rows)
            {
                clients.Add(new Client(item));
            }
            return clients;
        }
    }
}
