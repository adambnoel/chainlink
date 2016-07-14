using System;
using System.Text;

namespace ChainLinkCLI
{
	public class KDeleteRequest : IRequest
	{
		private String requestKey;
		private int kVotes;
		public KDeleteRequest(String key, int votes)
		{
			requestKey = key;
			kVotes = votes;
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
			sb.Append(kVotes);
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

