using System;
namespace DHTSharp
{
	public class PutRequest : IRequest
	{
		private string key;
		private string body;
		private Node destinationNode;
		private Boolean retransmit;
		public PutRequest(string requestKey, string requestBody, Node DestinationNode, Boolean Retransmit)
		{
			key = requestKey;
			body = requestBody;
			destinationNode = DestinationNode;
			retransmit = Retransmit;
		}
		public String Process()
		{
			TcpRequest request = new TcpRequest(destinationNode.GetIPAddress(), destinationNode.GetNodeSocket());
			return request.Send(initializePutRequest());
		}

		private String initializePutRequest()
		{
			String putRequest = "+\r\n";
			putRequest = putRequest + key + "\r\n";
			putRequest = putRequest + body + "\r\n";
			putRequest = (retransmit ? putRequest : putRequest + "|\r\n");
			return putRequest;
		}
	}
}

