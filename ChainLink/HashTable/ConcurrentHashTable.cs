using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DHTSharp
{
	public class ConcurrentHashTable : IHashTable
	{
		private Semaphore newKeySemaphore = new Semaphore(1, 1);
		private Dictionary<string, string> concurrentHashTable = new Dictionary<string, string>();
		private Dictionary<string, Semaphore> concurrentHashTableLocks = new Dictionary<string, Semaphore>();

		public Boolean LockKey(String Key)
		{
			return true;
		}

		public Boolean UnlockKey(String Key)
		{
			return true;
		}

		public String Get(String Key)
		{
			return "";
		}

		public Boolean Put(String Key, String Value)
		{
			/**
			if (concurrentHashTableLocks.ContainsKey(Key))
			{

			}
			else 
			{
				newKeySemaphore.WaitOne();
				try
				{
					if (concurrentHashTableLocks.ContainsKey(Key))
					{
						newKeySemaphore.Release();
					}
					else {

					}
				}
				finally
				{
					newKeySemaphore.Release();
				}
			}
			**/
			return true;
		}

		public Boolean Delete(String Key)
		{
			return true;
		}
	}
}

