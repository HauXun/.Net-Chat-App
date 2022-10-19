using dotNet_Chat_App.IAction;
using System.Collections;
using System.Text;
using System.Threading;

namespace dotNet_Chat_App.Config
{
	public class GUILogger : ILogger
	{
		private static Mutex mutex = new Mutex(false, @"Global\mutex2203");
		private static StringBuilder stringLog = new StringBuilder();

		public void WriteLogEntry(ArrayList entry, ref string systemMsg)
		{
			try
			{
				mutex.WaitOne();
				IEnumerator line = entry.GetEnumerator();
				while (line.MoveNext())
					stringLog.Append(line.Current).AppendLine();
				systemMsg = stringLog.ToString();
				stringLog.Clear();
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}

		public void WriteLogEntry(string entry, ref string systemMsg)
		{
			try
			{
				mutex.WaitOne();
				stringLog.Append(entry).AppendLine();
				systemMsg = stringLog.ToString();
				stringLog.Clear();
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}
	}
}
