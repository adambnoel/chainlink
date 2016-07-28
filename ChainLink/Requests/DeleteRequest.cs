using System;
namespace DHTSharp
{
	public class DeleteRequest : IRequest
	{
		private String key;
		private Node destinationNode;
		private Boolean retransmit;
		public DeleteRequest(String requestKey, Node DestinationNode, Boolean Retransmit)
		{
			key = requestKey;
			destinationNode = DestinationNode;
			retransmit = Retransmit;
		}

		public String Process()
		{
			TcpRequest request = new TcpRequest(destinationNode.GetIPAddress(), destinationNode.GetNodeSocket());
			return request.Send(initializeDeleteRequest());
		}

		private String initializeDeleteRequest()
		{
			String deleteRequest = "-\r\n";
			deleteRequest = deleteRequest + key + "\r\n";
			return deleteRequest;
		}

	}
}

