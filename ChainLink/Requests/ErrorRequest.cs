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

		public String ProcessRequest()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("!");
			sb.Append("\r\n");
			sb.Append("Failed to process request: ");
			sb.Append(requestText);
			sb.Append(". Invalid request header.");
			sb.Append("\r\n");
			return sb.ToString();
		}
	}
}

