using System;
namespace DHTSharp
{
	public class OSxDiskManager : IDiskManager
	{
		public Boolean FlushFileQueue()
		{
			return true;
		}

		public Boolean QueueWriteFile(String FileName, byte[] fileContents)
		{
			return true;
		}

		public Boolean ReadFile(String FileName)
		{
			return true;
		}

		public Boolean DeleteFile(String FileName)
		{
			return true;
		}
	}
}

