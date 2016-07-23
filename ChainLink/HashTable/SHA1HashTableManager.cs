using System;
using System.Collections;
using System.Collections.Generic;

namespace DHTSharp
{
	//This class
	public class SHA1HashTableManager
	{
		private Node currentNode;
		private List<Node> networkNodes;

		public SHA1HashTableManager(Node CurrentNode, List<Node> NetworkNodes)
		{
			currentNode = CurrentNode;
			networkNodes = NetworkNodes;
		}

		public Boolean RequestJoinNetwork(Node node)
		{
			//A node

			return true;
		}

		public Boolean RequestLeaveNetwork(Node node)
		{
			return true;
		}

		public Boolean AddNetworkNode(Node node)
		{
			networkNodes.Add(node);
			return true;
		}

		public Boolean RemoveNetworkNode(Node node)
		{
			networkNodes.Add(node);
			return true;
		}
	}
}

