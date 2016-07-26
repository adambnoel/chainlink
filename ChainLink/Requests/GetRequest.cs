using System;
using System.Text;
namespace DHTSharp
{
	public class GetRequest : IRequest
	{
		private String requestKey;
		private Node destinationNode;
		public GetRequest(String key, Node DestinationNode)
		{
			requestKey = key;
			destinationNode = DestinationNode;
		}

		public String Process()
		{
			TcpRequest request = new TcpRequest(destinationNode.GetIPAddress(), destinationNode.GetNodeSocket());
			return request.Send(initializeGetRequest());
		}

		private String initializeGetRequest()
		{
			String getRequest = String.Empty;
			getRequest = getRequest + "*\r\n";
			getRequest = getRequest + requestKey + "\r\n";
			return getRequest;
		}
	}
}

