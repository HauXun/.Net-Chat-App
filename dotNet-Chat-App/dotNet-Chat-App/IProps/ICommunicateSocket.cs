using dotNet_Chat_App.Common;
using System.Collections.Generic;

namespace dotNet_Chat_App.IProps
{
	public interface ICommunicateSocket
	{
		string IpAddress { get; set; }
		int Port { get; set; }
		string SystemMsg { get; set; }
		string UserMsg { get; set; }
		int OnlineClientCount { get; set; }
		List<Client> Clients { get; set; }
		GetLog GetLog { set; }

		bool Closing { get; set; }
	}
}
