using System;

namespace DHTSharp
{
	public interface IHashFunction
	{
		String GetHash(String key);
		int GetRange();
	}
}

