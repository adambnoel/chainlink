using System;
namespace ChainLinkCLI
{
	public class GetRequest : IRequest
	{
		private String requestKey;
		public GetRequest(String key)
		{
			requestKey = key;
		}

		public String Process()
		{
			return String.Empty;
		}

		private String initializeGetRequest()
		{
			return String.Empty;
		}

		private String handleResponse()
		{
			return String.Empty;
		}
	}
}

