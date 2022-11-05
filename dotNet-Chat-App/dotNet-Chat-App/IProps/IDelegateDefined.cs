using dotNet_Chat_App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IProps
{
	public interface IDelegateDefined
	{
		ListChanged ListChanged { get; set; }
		ClearListContainer ClearListContainer { get; set; }
        ReceiveRequestPacket ReceiveRequestPacket { get; set; }
    }
}
