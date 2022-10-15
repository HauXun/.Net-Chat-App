using dotNet_Chat_App.Model.BusinessLogicLayer;
using dotNet_Chat_App.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public int IsLogin(string Id, string passWord)
        {
            string query = $"SELECT ID FROM dbo.Clients WHERE ID = N'{Id}' AND Password = N'{passWord}'";
            object data = (DataProvider.Instance.ExcuteScalar(query));

            if (data == null || data == DBNull.Value)
                return default(int);
            else
                return (int)data;
        }
    }
}
