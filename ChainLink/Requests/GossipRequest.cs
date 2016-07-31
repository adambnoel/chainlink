using System;
using System.Collections;
using System.Collections.Generic;

namespace DHTSharp
{
	public class GossipRequest : IRequest
	{
		private Node targetNode;
		private String gossipRequest;
		public GossipRequest(Node TargetNode, List<Node> ConnectedNodes)
		{
			targetNode = TargetNode;
			formatGossipRequest(ConnectedNodes);
		}

		public String Process()
		{
			TcpRequest request = new TcpRequest(targetNode.GetIPAddress(), targetNode.GetNodeSocket());
			return request.Send(gossipRequest);
		}

		private void formatGossipRequest(List<Node> ConnectedNodes)
		{
			gossipRequest = "#\r\n";
			foreach (Node n in ConnectedNodes)
			{
				gossipRequest = gossipRequest + n.GetIPAddress().ToString() + "\r\n";
				gossipRequest = gossipRequest + n.GetNodeSocket().ToString() + "\r\n";
				List<String> ringDetails = n.GetNodeRingDetails();
				foreach (String ringDetail in ringDetails)
				{
					gossipRequest = ringDetail + "\r\n";
				}
			}
		}
	}
}

