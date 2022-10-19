using System.IO;

namespace dotNet_Chat_App.Common
{
	public enum TransportType : int
	{
		String,
		Image
	}

	public struct ReceiveBuffer
	{
		public const int BUFFER_SIZE = 1024;
		public byte[] Buffer;
		public int ToReceive;
		public MemoryStream BufferStream;

		public ReceiveBuffer(int toRec)
		{
			Buffer = new byte[BUFFER_SIZE];
			ToReceive = toRec;
			BufferStream = new MemoryStream(toRec);
		}

		public void Dispose()
		{
			Buffer = null;
			ToReceive = 0;
			Close();

			if (BufferStream != null)
			{
				BufferStream.Flush();
				BufferStream.Dispose();
			}
		}

		public void Close()
		{
			if (BufferStream != null && BufferStream.CanWrite)
			{
				BufferStream.Close();
			}
		}
	}
}