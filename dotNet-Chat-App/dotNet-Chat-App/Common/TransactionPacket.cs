using System;

namespace dotNet_Chat_App.Common
{
	[Serializable]
	public class TransactionPacket
	{
		public int Todo { get; }
		public object Value { get; }

		public TransactionPacket(int todo, object value)
		{
			Todo = todo;
			Value = value;
		}
	}
}
