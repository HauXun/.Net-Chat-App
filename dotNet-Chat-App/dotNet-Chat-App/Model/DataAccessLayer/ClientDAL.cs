using dotNet_Chat_App.Model.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Model.DataAccessLayer
{
    public class ClientDAL
    {
        private volatile static ClientDAL instance;

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
    }
}
