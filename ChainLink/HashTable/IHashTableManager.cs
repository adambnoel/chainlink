using System;
using System.Collections;
using System.Collections.Generic;


namespace DHTSharp
{
	public interface IHashTableManager
	{
		Boolean Run();
		Boolean RequestJoinNetwork(Node node);
		Boolean RequestLeaveNetwork(Node node);
		Boolean AddNetworkNode(Node node);
		Boolean RemoveNetworkNode(Node node);
		Boolean PutKey(String key, String contents);
		String GetValue(String key);
		Boolean DeleteKey(String key);
		List<String> GetKeySpace(String minKey, String maxKey);
		Boolean DeleteKeySpace(String minKey, String maxKey);
	}
}

