using System;
namespace DHTSharp
{
	public static class Time
	{
		public static long GetUnixTime()
		{
			var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
			return (long)timeSpan.TotalSeconds;
		}
	}
}

