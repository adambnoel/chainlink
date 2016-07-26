using System;
using System.Collections.Concurrent;

namespace DHTSharp
{
	public class HashTableWrapper
	{
		private ConcurrentDictionary<String, byte[]> hashTable = new ConcurrentDictionary<string, byte[]>();
		public byte[] Get(String Key)
		{
			byte[] getValueBytes = new byte[0];
			if (hashTable.TryGetValue(Key, out getValueBytes))
			{
				return getValueBytes;
			}
			return getValueBytes;
		}

		public Boolean Put(String Key, byte[] newValueBytes)
		{
			hashTable[Key] = newValueBytes;
			if (hashTable[Key] == newValueBytes)
			{
				return true;
			}
			return false;
		}

		public byte[] Delete(String Key)
		{
			byte[] removedValueBytes = new byte[0];
			if (hashTable.TryRemove(Key, out removedValueBytes))
			{
				return removedValueBytes;
			}
			return removedValueBytes;
		}

	}
}

