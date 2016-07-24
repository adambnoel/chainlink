using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace DHTSharp
{
	public class ClientRequestHandler
	{
		private TcpClient connectedClient;
		private DateTime lastRequestTime = DateTime.UtcNow;
		private Boolean isActive = true;
		private IHashTableManager tableManager;
		private Thread clientThread;

		public void Start(TcpClient Client, IHashTableManager TableManager)
		{
			connectedClient = Client;
			tableManager = TableManager;
			clientThread = new Thread(HandleRequest);
			clientThread.Start();
		}

		public void Stop()
		{
			clientThread.Abort();
		}

		public Boolean IsActive()
		{
			if (lastRequestTime.AddMinutes(5) <= DateTime.UtcNow)
			{
				isActive = false;
				return false;
			}
			else {
				return true;
			}
		}

		private void HandleRequest()
		{
			byte[] bytesFrom = new byte[4000000];
			byte[] bytesTo = null;
			String clientRequest = null;

			while (isActive)
			{
				NetworkStream networkStream = connectedClient.GetStream();
				lastRequestTime = DateTime.UtcNow;
				networkStream.Read(bytesFrom, 0, bytesFrom.Length);
				clientRequest = Encoding.ASCII.GetString(bytesFrom);
				if (clientRequest.LastIndexOf("\r\n", StringComparison.Ordinal) == -1) //Invalid request or connection closed
				{
					string serverResponse = "ERROR\r\n Invalid request format";
					bytesTo = Encoding.ASCII.GetBytes(serverResponse);
					networkStream.Write(bytesTo, 0, bytesTo.Length);
					networkStream.Flush();
				}
				else {
					clientRequest = clientRequest.Substring(0, clientRequest.LastIndexOf("\r\n", StringComparison.Ordinal));
					string serverResponse = parseRequestAndRespond(clientRequest);
					bytesTo = Encoding.ASCII.GetBytes(serverResponse);
					networkStream.Write(bytesTo, 0, bytesTo.Length);
					networkStream.Flush();
				}
			}
		}

		private String parseRequestAndRespond(String requestString)
		{
			IRequestProcessor requestProcessor = null;
			String[] splitRequest = requestString.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			if (splitRequest.Length <= 0)
			{
				requestProcessor = new ErrorRequestProcessor(requestString);
			}
			else 
			{
				//Try and parse the request -> If error then respond with 
				try
				{
					switch (splitRequest[0])
					{
						case "+":
							requestProcessor = new PutRequestProcessor(tableManager, requestString);
							break;
						case "-":
							requestProcessor = new DeleteRequestProcessor(tableManager, requestString);
							break;
						case "*":
							requestProcessor = new GetRequestProcessor(tableManager, requestString);
							break;
						case "^":
							requestProcessor = new JoinRequestProcessor(tableManager, requestString);
							break;
						case "$":
							requestProcessor = new LeaveRequestProcessor(tableManager, requestString);
							break;
						case "@":
							requestProcessor = new PingRequestProcessor(tableManager, requestString);
							break;
						case "#":
							requestProcessor = new GossipRequestProcessor(tableManager, requestString);
							break;
						default:
							requestProcessor = new ErrorRequestProcessor(requestString);
							break;
							
					}
				}
				catch (Exception e)
				{
					requestProcessor = new ErrorRequestProcessor(requestString);
				}
			}


			return (requestProcessor != null ? requestProcessor.ProcessAndRespond() : "");
		}

		/**
		private String parseRequestAndRespond(String requestString)
		{
			IRequest request = null;
			String[] splitRequest = requestString.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			if (splitRequest.Length <= 0)
			{
				request = new ErrorRequest(requestString);
				return request.ProcessRequest();
			}
			switch (splitRequest[0])
			{
				case "+":
					//request = new PutRequest(requestString);
					break;
				case "-":
					break;
				case "*":
					break;
				case "$":
					
					break;
				case "!":
					break;
				case "@":
					break;
				case "#":
					break;
				default:
					request = new ErrorRequest(requestString);
					break;
			}
			return request.ProcessRequest();
		}**/
	}
}

