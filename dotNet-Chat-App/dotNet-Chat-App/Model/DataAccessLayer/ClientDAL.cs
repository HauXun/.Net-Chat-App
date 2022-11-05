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

        public DataTable IsLogin(string Id, string passWord)
        {
            byte[] tmp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] convertMD5 = new MD5CryptoServiceProvider().ComputeHash(tmp);
            string pass = String.Join("", convertMD5);

            string query = $"SELECT * FROM dbo.Clients WHERE ID = N'{Id}' AND Password = N'{pass}'";
            return DataProvider.Instance.ExcuteQuery(query);
        }

        public DataTable GetClients()
        {
            string query = "SELECT * FROM dbo.Clients";
            return DataProvider.Instance.ExcuteQuery(query);
        }

        public bool SaveStatus(int id, int status)
        {
            string query = $"UPDATE dbo.Clients SET Online = {status} WHERE ID = {id}";
            return DataProvider.Instance.ExcuteNonQuery(query) > 0;
        }
    }
}
