using System;
using System.Text;

namespace ChainLinkCLI
{
	public class PutRequest : IRequest
	{
		private String requestKey;
		private String requestContents;
		public PutRequest(String key, String contents)
		{
			requestKey = key;
			requestContents = contents;
		}

		public void Process()
		{
			CLIConfigManager configManager = new CLIConfigManager();
			TcpRequest request = new TcpRequest(configManager.GetIPAddress(), configManager.GetPortNumber(), configManager.GetMaxRequestSize());
			String response = request.Send(initializePutRequest());
			handleResponse(response);
		}

		private String initializePutRequest()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('+');
			sb.Append("\r\n");
			sb.Append(requestKey);
			sb.Append("\r\n");
			sb.Append(requestContents);
			sb.Append("\r\n");
			return sb.ToString();
		}

		private void handleResponse(String response)
		{

		}
	}
}

