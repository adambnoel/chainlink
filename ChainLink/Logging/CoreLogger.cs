using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DHTSharp
{
	public class CoreLogger
	{
		private LoggingLevel currentLevel;
		private StreamWriter f;
		private Timer flushTimer;
		private Queue<String> LogQueue = new Queue<string>();

		public CoreLogger(String FilePath)
		{
			f = new StreamWriter(FilePath);
			currentLevel = LoggingLevel.CRITICAL; //Critical by default
			flushTimer = new Timer(emptyQueueTask, this, 10 * 1000, 10 * 1000);

			f.WriteLine("New logging session started at: " + DateTime.UtcNow);
		}

		public void Stop()
		{
			flushTimer.Dispose();
			emptyQueueTask(this);
			f.Close();
		}

		public void Log(String LogText, LoggingLevel TextLoggingLevel)
		{
			if (TextLoggingLevel < currentLevel)
			{
				LogQueue.Enqueue(LogText);
			}
		}

		public void SetLoggingLevel(LoggingLevel TargetLoggingLevel)
		{
			currentLevel = TargetLoggingLevel;
			LogQueue.Enqueue("Changing logging level to: " + TargetLoggingLevel);
		}

		private void emptyQueueTask(object state)
		{
			
			while (LogQueue.Count != 0)
			{
				try
				{
					String queuedText = LogQueue.Dequeue();
					f.WriteLine(queuedText);
				}
				catch
				{

				}
			}
			f.Flush();
		}

	}
}

