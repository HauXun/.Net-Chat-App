using dotNet_Chat_App.Model.DataAccessLayer;
using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotNet_Chat_App.Model.BusinessLogicLayer
{
    public class ChatGroupBLL
    {
        private volatile static ChatGroupBLL instance;

        private ChatGroupBLL()
        {

        }
        public static ChatGroupBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChatGroupBLL();
                return instance;
            }
            set => instance = value;
        }

        public int AddGroup(string groupName)
        {
            return Convert.ToInt32(ChatGroupDAL.Instance.AddGroup(groupName));
        }

        public List<ChatGroup> GetGroupList()
        {
            DataTable data = ChatGroupDAL.Instance.GetGroupList();

            if (data != null && data.Rows.Count > 0)
            {
                List<ChatGroup> listGroup = new List<ChatGroup>();
                foreach (DataRow row in data.Rows)
                {
                    listGroup.Add(new ChatGroup(row));
                }
                return listGroup;
            }
            return null;
        }
    }
}
