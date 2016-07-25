using System;
using System.Text;
namespace DHTSharp
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
			//CLIConfigManager configManager = new CLIConfigManager();
			//TcpRequest request = new TcpRequest(configManager.GetIPAddress(), configManager.GetPortNumber(), configManager.GetMaxRequestSize());
			//String response = request.Send(initializeGetRequest());

			return "";
		}

		private String initializeGetRequest()
		{
			String getRequest = String.Empty;
			getRequest = getRequest + "*\r\n";
			getRequest = getRequest + requestKey + "\r\n";
			return getRequest;
		}
	}
}

