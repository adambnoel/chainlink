using System;
using System.Security.Cryptography;

namespace DHTSharp
{
	public class SHA1HashFunction : IHashFunction
	{
		public String GetHash(String key) {
			return SHA1 (key);
		}

		public int GetRange() {
			return 160;
		}
	}
}

