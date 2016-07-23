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
		 * If key not present -> send request to network component
		 * If key present in one-hop node -> request from node
		 * If not present -> calculate closest node distance
		 * */
		public String ProcessRequest()
		{
			
			return String.Empty;
		}
	}
}

