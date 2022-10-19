using System.Net;

namespace dotNet_Chat_App.Services
{
	public class IPServices
	{
		public static string GetIPAddress()
		{
			string hostName = Dns.GetHostName();
			return Dns.GetHostEntry(hostName).AddressList[0].ToString();
		}
	}
}
