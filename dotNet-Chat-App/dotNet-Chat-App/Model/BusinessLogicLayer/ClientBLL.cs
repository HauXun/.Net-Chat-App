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
    }
}
