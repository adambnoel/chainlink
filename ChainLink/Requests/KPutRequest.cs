using System;
namespace DHTSharp
{
	public class KPutRequest : IRequest
	{
		private string key;
		private int votes;
		private string body;

		public KPutRequest(string requestKey, int kVotes, string requestBody)
		{
			key = requestKey;
			votes = kVotes;
			body = requestBody;
		}

		public String ProcessRequest()
		{
			return String.Empty;
		}
	}
}

