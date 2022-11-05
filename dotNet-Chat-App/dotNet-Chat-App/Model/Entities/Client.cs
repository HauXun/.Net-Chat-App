namespace dotNet_Chat_App
{
    using System;
    using System.Collections.Generic;
    using System.Data;
	using System.IO;
    using System.Net;
    using System.Net.Sockets;
	using System.Xml.Serialization;
	[Serializable]
    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Online { get; set; }
        public Socket M_Client { get; set; }
        public EndPoint M_EndPoint { get; set; }

        public Client()
        {
        }

        public Client(DataRow row)
        {
            this.ID = int.Parse(row["ID"].ToString());
            this.Name = row["Name"].ToString();
            this.Password = row["Password"].ToString();
            this.Online = (bool)row["Online"];
        }
	}
}
