using dotNet_Chat_App.TAP_Socket_Model;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IProps
{
	public interface ISCommunicate
	{
		Socket Listener { get; }
		Thread Thread { get; }
		Socket LastClient { get; }
		List<Socket> ClientSockets { get; }

		Task<TAPResultPattern<Socket>> AcceptTask { get; }
		TAPResultPattern<Socket> AcceptResult { get; }
	}
}
