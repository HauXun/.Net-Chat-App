using dotNet_Chat_App.Common;
using dotNet_Chat_App.TAP_Socket_Model;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IProps
{
	public interface IReceivePacket
	{
		// Receive struct
		byte[] LenBuffer { get; }
		ReceiveBuffer Buffer { get; }

		Task<TAPResultPattern<int>> ReceivePacketSizeTask { get; }
		TAPResultPattern<int> ReceivePacketSizeTaskResult { get; }

		Task<TAPResultPattern> ReceivePacketTask { get; }
	}
}
