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
		private HashTableManager tableManager;
		private CoreLogger logger;
		private Thread clientThread;
		private IPAddress clientIPAddress;
		private int clientSocket;

		public void Start(TcpClient Client, HashTableManager TableManager, CoreLogger Logger)
		{
			logger = Logger;
			logger.Log("Servicing new request", LoggingLevel.DEBUGGING);
			connectedClient = Client;
			clientIPAddress = ((IPEndPoint)connectedClient.Client.RemoteEndPoint).Address;
			clientSocket = ((IPEndPoint)connectedClient.Client.RemoteEndPoint).Port;
			tableManager = TableManager;
			clientThread = new Thread(HandleRequest);
			clientThread.Start();
		}

		public void Stop()
		{
			logger.Log("Tearing down client connection", LoggingLevel.DEBUGGING);
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
				if (networkStream.CanRead)
				{
					lastRequestTime = DateTime.UtcNow;
					networkStream.Read(bytesFrom, 0, bytesFrom.Length);
					clientRequest = Encoding.ASCII.GetString(bytesFrom);
					if (clientRequest.LastIndexOf("\r\n", StringComparison.Ordinal) == -1) //Invalid request or connection closed
					{
						logger.Log("Invalid request made", LoggingLevel.DEBUGGING);
						string serverResponse = "ERROR\r\n Invalid request format";
						bytesTo = Encoding.ASCII.GetBytes(serverResponse);
						networkStream.Write(bytesTo, 0, bytesTo.Length);
						networkStream.Flush();
					}
					else {
						logger.Log("Servicing client request", LoggingLevel.VERBOSE);
						clientRequest = clientRequest.Substring(0, clientRequest.LastIndexOf("\r\n", StringComparison.Ordinal));
						string serverResponse = parseRequestAndRespond(clientRequest);
						bytesTo = Encoding.ASCII.GetBytes(serverResponse);
						networkStream.Write(bytesTo, 0, bytesTo.Length);
						networkStream.Flush();
						networkStream.Close();
						logger.Log("Finished servicing client request", LoggingLevel.VERBOSE);
					}
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
							logger.Log("Servicing put request", LoggingLevel.DEBUGGING);
							requestProcessor = new PutRequestProcessor(tableManager, requestString);
							break;
						case "-":
							logger.Log("Servicing delete request", LoggingLevel.DEBUGGING);
							requestProcessor = new DeleteRequestProcessor(tableManager, requestString);
							break;
						case "*":
							logger.Log("Servicing get request", LoggingLevel.DEBUGGING);
							requestProcessor = new GetRequestProcessor(tableManager, requestString);
							break;
						case "^":
							logger.Log("Servicing join request", LoggingLevel.DEBUGGING);
							requestProcessor = new JoinRequestProcessor(tableManager, requestString);
							break;
						case "$":
							logger.Log("Servicing delete request", LoggingLevel.DEBUGGING);
							requestProcessor = new LeaveRequestProcessor(tableManager, requestString);
							break;
						case "@":
							logger.Log("Servicing ping request", LoggingLevel.DEBUGGING);
							requestProcessor = new PingRequestProcessor(tableManager, requestString, new Node(new List<Ring>(), clientIPAddress, clientSocket));
							break;
						case "#":
							logger.Log("Servicing gossip request", LoggingLevel.DEBUGGING);
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
	}
}

