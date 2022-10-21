namespace dotNet_Chat_App.Common
{
	// Nhận thông tin log
	public delegate void GetLog(string log);
	public delegate void ClientListChanged(Client client = null);
	public delegate void ClearClientListContainer();
}