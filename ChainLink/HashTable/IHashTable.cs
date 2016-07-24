using System;
namespace DHTSharp
{
	public interface IHashTable
	{
		Boolean LockKey(String Key);
		Boolean UnlockKey(String Key);
		String Get(String Key);
		Boolean Put(String Key, String Value);
		Boolean Delete(String Key);
	}
}

