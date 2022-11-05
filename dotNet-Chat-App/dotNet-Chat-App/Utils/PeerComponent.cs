using System.Net;

namespace dotNet_Chat_App.Utils
{
    public class PeerComponent
    {
        public int ID { get; set; }
        public EndPoint M_EndPoint { get; set; }

        public PeerComponent(int iD, EndPoint endPoint)
        {
            ID = iD;
            M_EndPoint = endPoint;
        }
    }
}
