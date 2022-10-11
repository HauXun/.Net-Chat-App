namespace dotNet_Chat_App
{
    using System;
    using System.Collections.Generic;
    
    public  class Message
    {
        public int ID { get; set; }
        public string DetailMessage { get; set; }
        public int ClientSent { get; set; }
        public int ClientReceiver { get; set; }
        public bool Sent { get; set; }
    }
}
