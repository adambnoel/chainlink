using System;
using System.Text;

namespace DHTSharp
{
	public class ErrorRequest : IRequest
	{
		private string requestText;
		public ErrorRequest(String inputRequestText)
		{
			requestText = inputRequestText;
		}

		public String Process()
		{
			String responseString = String.Empty;
			responseString = responseString + "!\r\n";
			responseString = responseString + "Failed to process request: ";
			responseString = responseString + requestText;
			responseString = responseString + ". Invalid request format. \r\n";
			return responseString;
		}
	}
}

