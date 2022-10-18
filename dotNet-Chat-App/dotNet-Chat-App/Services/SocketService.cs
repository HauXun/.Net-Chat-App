using System.Net.Sockets;

namespace dotNet_Chat_App.Services
{
	public class SocketService
	{
		public static bool SocketConnected(Socket s)
		{
			bool part1 = s.Poll(1000, SelectMode.SelectRead);
			bool part2 = (s.Available == 0);
			if (part1 & part2)
			{
				//connection is closed
				return false;
			}
			return true;
		}
	}
}
