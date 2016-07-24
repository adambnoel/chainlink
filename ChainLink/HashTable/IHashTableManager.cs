using System;
using System.Collections;
using System.Collections.Generic;


namespace DHTSharp
{
	public interface IHashTableManager
	{
		Boolean Run();
		Boolean AddRequestHandler(ClientRequestHandler requestHandler);
		String RequestJoinNetwork(Node node);
		String RequestLeaveNetwork(Node node);
		Boolean AddNetworkNode(Node node);
		Boolean RemoveNetworkNode(Node node);
		String PutKey(String key, String contents, int kCount);
		String GetValue(String key, int kCount);
		String DeleteKey(String key, int kCount);
		//List<String> GetKeySpace(String minKey, String maxKey);
		//Boolean DeleteKeySpace(String minKey, String maxKey);
	}
}

