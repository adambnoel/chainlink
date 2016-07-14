using System;
namespace DHTSharp
{
	public class KGetRequest : IRequest
	{
		private int votes;
		private string key;
		public KGetRequest(String requestKey, int kVotes)
		{
			key = requestKey;
			votes = kVotes;
		}

		public String ProcessRequest()
		{
			return String.Empty;
		}
	}
}

