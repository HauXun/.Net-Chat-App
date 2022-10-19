using dotNet_Chat_App.TAP_Socket_Model;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IProps
{
	public interface ITranspotDefine
	{
		Task<TAPResultPattern<int>> ReceivePacketSizeAsync(Socket socket);
		Task<TAPResultPattern> ReceivePacketAsync(Socket socket);
		Task<TAPResultPattern<int>> SendPacketAsync(byte[] data);
		Task<TAPResultPattern<int>> SendPacketAsync(object args);
	}
}
