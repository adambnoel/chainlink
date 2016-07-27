using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DHTSharp
{
	public class JoinRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private Node sourceNode;

		public JoinRequestProcessor(HashTableManager TableManager, String JoinRequest)
		{
			tableManager = TableManager;
			String[] splitJoinRequest = JoinRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			IPAddress newNodeIP = IPAddress.Parse(splitJoinRequest[1]);
			int newNodeSocket = int.Parse(splitJoinRequest[2]);
			sourceNode = new Node(new List<Ring>(), newNodeIP, newNodeSocket);
		}

		public String ProcessAndRespond()
		{
			return tableManager.RequestJoinNetwork(sourceNode);
		}
	}
}

