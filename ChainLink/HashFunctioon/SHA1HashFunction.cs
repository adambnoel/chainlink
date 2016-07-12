using System;
using System.Security.Cryptography;
using System.Text;


namespace DHTSharp
{
	public class SHA1HashFunction : IHashFunction
	{
		public String GetHash(String key) {
			var sha1 = SHA1.Create();
			return ConvertHashToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(key)));
		}

		private String ConvertHashToString(byte[] hashResult)
		{
			String hashString = String.Empty;

			for (int i = 0; i < hashResult.Length; i++)
			{
				hashString = hashString + hashResult.ToString();
			}

			return hashString;
		}

		public int GetRange() {
			return 160;
		}
	}
}

