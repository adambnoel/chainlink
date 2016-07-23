using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DHTSharp
{
	//This class
	public class SHA1HashTableManager
	{
		private Node currentNode;
		private object networkNodeListLock = new object();
		private List<Node> networkNodes;
		private Timer pingTimer;
		private Timer gossipTimer;

		public SHA1HashTableManager(Node CurrentNode, List<Node> NetworkNodes)
		{
			currentNode = CurrentNode;
			networkNodes = NetworkNodes;

		}

		public Boolean Run()
		{
			
			return true;
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

		private void pingTask()
		{
			
			foreach (Node node in networkNodes)
			{
				PingRequest newPingRequest = new PingRequest(node);
				if (newPingRequest.ProcessRequest() != "OK\r\n")
				{
					if (node.FailedNodePulse())
					{
						//Assume the node has dropped
					}
				}
				else {
					node.ResetFailedPingCount();
				}
			}
		}

		private void gossipTask()
		{
			foreach (Node node in networkNodes)
			{

			}
		}

		private void tearDownTask()
		{
			
		}
	}
}

