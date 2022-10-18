namespace dotNet_Chat_App
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Online { get; set; }
        public Socket M_Client { get; set; }
    }
}
