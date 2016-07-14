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

		/**
		 * 
		 * First, contact local node to see if requestKey is present
		 * If key not present -> 
		 * 
		 * */
		public String ProcessRequest()
		{
			
			return String.Empty;
		}
	}
}

