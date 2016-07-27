using System;
using System.Text;

namespace DHTSharp
{
	public class JoinRequest : IRequest
	{
		Node sourceNode;
		Node destinationNode;
		public JoinRequest(Node SourceNode, Node DestinationNode)
		{
			sourceNode = SourceNode;
			destinationNode = DestinationNode;
		}

		public String Process()
		{
			TcpRequest request = new TcpRequest(destinationNode.GetIPAddress(), destinationNode.GetHashCode());
			return request.Send(initializeJoinRequest());
		}
		private String initializeJoinRequest()
		{
			String joinRequest = "$\r\n";
			joinRequest = joinRequest + sourceNode.GetIPAddress().ToString() + "\r\n";
			joinRequest = joinRequest + sourceNode.GetNodeSocket().ToString() + "\r\n";
			return joinRequest;
		}
	}
}

