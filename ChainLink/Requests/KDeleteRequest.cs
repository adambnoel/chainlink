using System;
namespace DHTSharp
{
	public class KDeleteRequest : IRequest
	{
		private int votes;
		private String key;
		public KDeleteRequest(String requestKey, int kVotes)
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

