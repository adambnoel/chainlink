using System;
using System.Text;

namespace ChainLinkCLI
{
	public class GetRequest : IRequest
	{
		private String requestKey;
		public GetRequest(String key)
		{
			requestKey = key;
		}

		public void Process()
		{
			CLIConfigManager configManager = new CLIConfigManager();
			TcpRequest request = new TcpRequest(configManager.GetIPAddress(), configManager.GetPortNumber(), configManager.GetMaxRequestSize());
			String response = request.Send(initializeGetRequest());
			handleResponse(response);
		}

		private String initializeGetRequest()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('*'); //ChainLink protocol defines * as a get request
			sb.Append("\r\n");
			sb.Append(requestKey);
			sb.Append("\r\n");
			return sb.ToString();
		}

		private void handleResponse(String response)
		{
			Console.WriteLine("Response is: " + response);
		}
	}
}

