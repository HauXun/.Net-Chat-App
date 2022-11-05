using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Model.DataAccessLayer
{
    public class ClientChatGroupDAL
    {
        private volatile static ClientChatGroupDAL instance;

        private ClientChatGroupDAL()
        {

        }
        public static ClientChatGroupDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientChatGroupDAL();
                return instance;
            }
            set => instance = value;
        }

        public bool SaveClientToGroup(int idGroup, int idClient)
        {
            string query = $"INSERT dbo.ClientsChatGroup(IDGroup, IDClient) VALUES ({idGroup} , {idClient})";

            return DataProvider.Instance.ExcuteNonQuery(query) > 0;
        }
    }
}
