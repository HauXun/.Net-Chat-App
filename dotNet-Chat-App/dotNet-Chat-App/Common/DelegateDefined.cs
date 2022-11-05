namespace dotNet_Chat_App.Common
{
	// Nhận thông tin log
	public delegate void GetLog(string log);
	public delegate void ListChanged();
	public delegate void ClearListContainer();
	public delegate void ReceiveRequestPacket(object data);
}