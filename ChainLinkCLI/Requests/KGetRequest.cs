﻿using System;
using System.Text;

namespace ChainLinkCLI
{
	public class KGetRequest : IRequest
	{
		private String requestKey;
		private int kVotes;
		public KGetRequest(String key, int votes) 
		{
			requestKey = key;
			kVotes = votes;
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

