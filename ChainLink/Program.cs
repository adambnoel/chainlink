using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DHTSharp
{
	class MainClass
	{
		public static void Main (string[] args)
		{

			//HashTableManager manager = new HashTableManager();

			//Initialize the logger
			String logFilePath = Path.Combine(ConfigurationManager.AppSettings.Get("FilePath"), ConfigurationManager.AppSettings.Get("FileName"));
			CoreLogger logger = new CoreLogger(logFilePath);
			String logLevelString = ConfigurationManager.AppSettings.Get("LoggingLevel");
			LoggingLevel loggingLevel = LoggingLevel.CRITICAL;
			switch (logLevelString)
			{
				case "1":
					loggingLevel = LoggingLevel.ERROR;
					break;
				case "2":
					loggingLevel = LoggingLevel.WARNING;
					break;
				case "3":
					loggingLevel = LoggingLevel.VERBOSE;
					break;
				case "4":
					loggingLevel = LoggingLevel.DEBUGGING;
					break;
				default:
					loggingLevel = LoggingLevel.CRITICAL; //Critical indicates that the application should crash
					break;
			}
			logger.SetLoggingLevel(loggingLevel);
			logger.Log("ChainLink node started at: " + DateTime.UtcNow, LoggingLevel.VERBOSE);

			//Configure current node -> Given that the node was offline
			//You do not know what your keyrange is
			List<Ring> localNodeRings = new List<Ring>();
			Node currentNode = new Node(localNodeRings, IPAddress.Parse(ConfigurationManager.AppSettings.Get("IPAddress")), int.Parse(ConfigurationManager.AppSettings.Get("PortNumber")));


			//Check for configuration xml -> If found try and connect to old network
			//If not -> Start as a new DHT network
			try
			{
				String xmlFile;
				using (StreamReader r = new StreamReader(ConfigurationManager.AppSettings.Get("ConfigurationXmlFile")))
				{
					xmlFile = r.ReadToEnd();
				}

				logger.Log("Loaded network configuration", LoggingLevel.VERBOSE);
			}
			catch (Exception e)
			{
				logger.Log("Couldn't find log file", LoggingLevel.VERBOSE);
				logger.Log("Started new DHT", LoggingLevel.VERBOSE);
				int numRings = int.Parse(ConfigurationManager.AppSettings.Get("NumberOfRings"));
				for (int i = 0; i < numRings; i++)
				{
					
				}

			}
			finally
			{

			}


			//Application configured -> Start up main loop
			TcpListener serverSocket = new TcpListener(currentNode.GetIPAddress(), currentNode.GetNodeSocket());
			TcpClient clientSocket = default(TcpClient);
			serverSocket.Start();

			while (true)
			{
				clientSocket = serverSocket.AcceptTcpClient();
				ClientRequestHandler requestHandler = new ClientRequestHandler();
				//requestHandler.Start(clientSocket);


				/**
				NetworkStream networkStream = clientSocket.GetStream();
				byte[] bytesFrom = new byte[2000000];
				networkStream.Read(bytesFrom, 0, bytesFrom.Length);

				String clientData = System.Text.Encoding.ASCII.GetString(bytesFrom);
				clientData = clientData.Substring(0, clientData.LastIndexOf("\r\n", StringComparison.Ordinal));
				Console.WriteLine("Received request from client");
				**/

				//Console.WriteLine(" >> Data from client - " + dataFromClient);
				//string serverResponse = "Last Message from client" + dataFromClient;
				//Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
				//networkStream.Write(sendBytes, 0, sendBytes.Length);
				//networkStream.Flush();
				//Console.WriteLine(" >> " + serverResponse);
			}

			serverSocket.Stop();
			clientSocket.Close();
		}
	}
}

