namespace dotNet_Chat_App
{
    using Newtonsoft.Json;
    using System;
    using System.Data;

    public class Message
    {
        [JsonIgnore]
        [JsonProperty("ID")]
        public int? ID { get; set; }
        [JsonProperty("DetailMessage")]
        public byte[] DetailMessage { get; set; }
        [JsonProperty("ClientSent")]
        public int? ClientSent { get; set; }
        [JsonProperty("ClientReceiver")]
        public int? ClientReceiver { get; set; }
        [JsonProperty("Sent")]
        public bool Sent { get; set; }
        [JsonProperty("MessageType")]
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
