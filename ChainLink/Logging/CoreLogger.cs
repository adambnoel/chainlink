using System;
namespace DHTSharp
{
	public class CoreLogger
	{
		private LoggingLevel currentLevel;
		public CoreLogger(String LogFileName)
		{
			currentLevel = LoggingLevel.CRITICAL; //Critical by default
		}
		public void Log(String LogText, LoggingLevel TextLoggingLevel)
		{
			if (TextLoggingLevel < currentLevel)
			{

			}
		}

		public void SetLoggingLevel(LoggingLevel TargetLoggingLevel)
		{
			currentLevel = TargetLoggingLevel;
		}


	}
}

