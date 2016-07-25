using System;
using System.Collections;
using System.Collections.Generic;

namespace DHTSharp
{
	public class GossipRequest : IRequest
	{
		private Node targetNode;

		public GossipRequest(Node TargetNode, List<Node> ConnectedNodes)
		{
			targetNode = TargetNode;
		}

		public String Process()
		{
			return "";
		}

		private String FormatGossipRequest(List<Node> ConnectedNodes)
		{
			String gossipRequest = String.Empty;
			foreach (Node n in ConnectedNodes)
			{
				
			}
			return gossipRequest;
		}
	}
}

