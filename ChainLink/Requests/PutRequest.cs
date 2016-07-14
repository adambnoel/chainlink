using System;
namespace DHTSharp
{
	public class PutRequest : IRequest
	{
		private string key;
		private string body;

		public PutRequest(string requestKey, string requestBody)
		{
			key = requestKey;
			body = requestBody;
		}
		public String ProcessRequest()
		{
			return String.Empty;
		}
	}
}

