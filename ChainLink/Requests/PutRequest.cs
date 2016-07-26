using System;
namespace DHTSharp
{
	public class PutRequest : IRequest
	{
		private string key;
		private string body;

		public PutRequest(string requestKey, string requestBody, Node destinationNode)
		{
			key = requestKey;
			body = requestBody;
		}
		public String Process()
		{
			return String.Empty;
		}
	}
}

