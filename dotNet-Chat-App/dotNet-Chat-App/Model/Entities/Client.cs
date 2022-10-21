namespace dotNet_Chat_App
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net.Sockets;
    using System.Xml.Serialization;

    [Serializable]
    public class Client
    {
        [XmlElement("id")]
        public int ID { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("pass")]
        public string Password { get; set; }

        [XmlElement("online")]
        public bool Online { get; set; }

        [XmlElement("m_client")]
        public object M_Client { get; set; }

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
