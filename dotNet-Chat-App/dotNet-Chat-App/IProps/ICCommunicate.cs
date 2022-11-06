using dotNet_Chat_App.TAP_Socket_Model;
using System.Net.Sockets;

namespace dotNet_Chat_App.IProps
{
	public interface ICCommunicate
	{
		Socket Client { get; }
		Client MyClient { get; set; }
		string P2PMsg { get; set; }


		TAPResultPattern ConnectResult { get; }
	}
}
