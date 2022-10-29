namespace dotNet_Chat_App
{
    using System;
    using System.Data;

    public class Message
    {
        public int ID { get; set; }
        public byte[] DetailMessage { get; set; }
        public int? ClientSent { get; set; }
        public int ClientReceiver { get; set; }
        public bool Sent { get; set; }
        public int MessageType { get; set; }

        public Message()
        {

        }

        public Message(DataRow row)
        {
            ID = Convert.ToInt32(row["ID"]);
            DetailMessage = row["DetailMessage"] as byte[];

            ClientSent = row["ClientSent"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(row["ClientSent"]);

            ClientReceiver = Convert.ToInt32(row["ClientReceiver"]);
            Sent = Convert.ToBoolean(row["Sent"]);
            MessageType = Convert.ToInt32(row["MessageType"]); 
        }
    }
}
