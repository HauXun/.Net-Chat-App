using dotNet_Chat_App.Common;
using System.Collections;

namespace dotNet_Chat_App.IAction
{
	public interface ILogger
	{
		void WriteLogEntry(ArrayList entry, ref string systemMsg);
		void WriteLogEntry(string entry, ref string systemMsg);
	}
}
