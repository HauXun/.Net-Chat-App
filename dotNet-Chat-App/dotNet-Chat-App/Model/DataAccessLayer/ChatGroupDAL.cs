using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Model.DataAccessLayer
{
    public class ChatGroupDAL
    {
        private volatile static ChatGroupDAL instance;

        private ChatGroupDAL()
        {

        }
        public static ChatGroupDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChatGroupDAL();
                return instance;
            }
            set => instance = value;
        }

        public object AddGroup(string groupName)
        {
            string query = $"USP_AddGroupChat @GroupName";

            return DataProvider.Instance.ExcuteScalar(query, new object[] {groupName});
        }

        public DataTable GetGroupList()
        {
            string query = "SELECT * FROM dbo.ChatGroup"; 
            return DataProvider.Instance.ExcuteQuery(query);
        }
    }
}
