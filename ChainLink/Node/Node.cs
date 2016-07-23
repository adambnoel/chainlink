using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace DHTSharp
{
	public class Node
	{
		private List<Ring> nodeDHTRings;
		private IPAddress nodeAddress;
		private int nodeSocket;

		public Node (List<Ring> DHTRings, IPAddress NodeAddress, int Socket) 
		{
			nodeDHTRings = DHTRings;
			nodeAddress = NodeAddress;
			nodeSocket = Socket;
		}
		public Boolean checkNodeRingsForKey(String hashKey)
		{
			foreach (Ring r in nodeDHTRings)
			{
				BigInteger distance = r.GetHashkeyDistance(hashKey);
				if (distance == 0) //Distance is 0 -> key on this ring
				{
					return true;
				}
			}
			return false;
		}
	}
}

