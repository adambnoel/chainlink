using System;
namespace DHTSharp
{
	public class GetRequest : IRequest
	{
		private string key;
		public GetRequest(String requestKey)
		{
			key = requestKey;
		}
		public String ProcessRequest()
		{
			return String.Empty;
		}
	}
}

