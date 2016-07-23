using System;
namespace DHTSharp
{
	public interface IDiskManager
	{
		Boolean FlushFileQueue();
		Boolean QueueWriteFile(String FileName, byte[] fileContents);
		Boolean ReadFile(String FileName);
		Boolean DeleteFile(String FileName);
	}
}

