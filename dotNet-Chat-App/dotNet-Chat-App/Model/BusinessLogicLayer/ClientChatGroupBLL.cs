using dotNet_Chat_App.Model.DataAccessLayer;
using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Model.BusinessLogicLayer
{
    public class ClientChatGroupBLL
    {
        private volatile static ClientChatGroupBLL instance;

        private ClientChatGroupBLL()
        {

        }
        public static ClientChatGroupBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientChatGroupBLL();
                return instance;
            }
            set => instance = value;
        }

        public bool SaveClientToGroup(int idGroup, int idClient)
        {
            return ClientChatGroupDAL.Instance.SaveClientToGroup(idGroup, idClient);
        }
    }
}
