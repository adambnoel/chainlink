using System;
using System.Security.Cryptography;
using System.Text;



namespace DHTSharp
{
	public class SHA1HashFunction : IHashFunction
	{
		public String GetHash(String key) {
			var sha1 = SHA1.Create();
			String hashString = Encoding.ASCII.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(key)));
			return hashString;
		}

		public int GetRange() {
			return 160;
		}
	}
}

