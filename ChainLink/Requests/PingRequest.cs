using System;
namespace DHTSharp
{
	public class PingRequest : IRequest
	{
		private Node targetNode;
		public PingRequest(Node TargetNode)
		{
			targetNode = TargetNode;
		}

		public String Process()
		{
			TcpRequest request = new TcpRequest(targetNode.GetIPAddress(), targetNode.GetNodeSocket());
			return request.Send(initializePingRequest());
		}

		private String initializePingRequest()
		{
			String pingRequest = String.Empty;
			pingRequest = pingRequest + "@\r\n";
			pingRequest = pingRequest + DateTime.UtcNow + "\r\n";
			return pingRequest;
		}

	}
}

