using dotNet_Chat_App.Model.DataAccessLayer;
using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
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
            return ClientDAL.Instance.IsLogin(Id, passWord);
        }

        public List<Client> GetClients()
        {
            return ClientDAL.Instance.GetClients();
        }
    }
}
