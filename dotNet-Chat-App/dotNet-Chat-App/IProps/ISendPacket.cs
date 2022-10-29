using dotNet_Chat_App.TAP_Socket_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IProps
{
    public interface ISendPacket
    {
        Task<TAPResultPattern<int>> HandleSendTask { get; }
        TAPResultPattern<int> HandleSendTaskResult { get; }
	}
}
