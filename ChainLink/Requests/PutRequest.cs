using System;
namespace DHTSharp
{
	public class PutRequest : IRequest
	{
		private string key;
		private string body;
		private Node destinationNode;
		public PutRequest(string requestKey, string requestBody, Node DestinationNode)
		{
			key = requestKey;
			body = requestBody;
			destinationNode = DestinationNode;
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
			return putRequest;
		}
	}
}

