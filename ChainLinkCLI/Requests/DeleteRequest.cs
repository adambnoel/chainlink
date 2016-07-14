using System;
using System.Text;

namespace ChainLinkCLI
{
	public class DeleteRequest : IRequest
	{
		private String requestKey;
		public DeleteRequest(String key)
		{
			requestKey = key;
		}

		public void Process()
		{
			CLIConfigManager configManager = new CLIConfigManager();
			TcpRequest request = new TcpRequest(configManager.GetIPAddress(), configManager.GetPortNumber(), configManager.GetMaxRequestSize());
			String response = request.Send(initializeDeleteRequest());
			handleResponse(response);
		}

		private String initializeDeleteRequest()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('-');
			sb.Append("\r\n");
			sb.Append(requestKey);
			sb.Append("\r\n");
			return sb.ToString();
		}

		private void handleResponse(String response)
		{

		}
	}
}

