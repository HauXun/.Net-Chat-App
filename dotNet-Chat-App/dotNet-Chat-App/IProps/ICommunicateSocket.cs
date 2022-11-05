﻿using dotNet_Chat_App.Common;
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
		List<ChatGroup> GroupList { get; set; }

		bool P2P { get; set; }
		bool Closing { get; set; }
	}
}
