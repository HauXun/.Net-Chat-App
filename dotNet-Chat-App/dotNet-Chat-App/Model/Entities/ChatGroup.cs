namespace dotNet_Chat_App
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Data;

    public class ChatGroup
    {
        public int ID { get; set; }
        public string GroupName { get; set; }

        public ChatGroup()
        {

        }

        public ChatGroup(DataRow row)
        {
            ID = Convert.ToInt32(row["ID"]);
            GroupName = row["GroupName"].ToString();
        }
    }
}
