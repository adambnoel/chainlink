using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;

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

			//Configure the hash table manager and all classes used by application
			List<Ring> localNodeRings = new List<Ring>();
			Node rootNode = new Node(localNodeRings, IPAddress.Parse(ConfigurationManager.AppSettings.Get("IPAddress")), int.Parse(ConfigurationManager.AppSettings.Get("PortNumber")));
			List<Node> connectedNodes = new List<Node>();
			HashTableWrapper wrapper = new HashTableWrapper();
			HashTableManager manager = new HashTableManager(rootNode, connectedNodes, wrapper, logger);

			//Configure current node -> Given that the node was offline
			//Keyrange is unknown -> Will need to join once again
			try
			{
				XDocument networkFile = XDocument.Load(ConfigurationManager.AppSettings.Get("NetworkXmlFile"));
				var networkNodes = networkFile.Root
											  .Elements("SeedNodes")
											  .Select(x => new Node(new List<Ring>(),
															IPAddress.Parse((string)x.Attribute("ip")),
															int.Parse((string)x.Attribute("port"))))
				                              .ToList();
				logger.Log("Parsed network file. Trying to join network", LoggingLevel.VERBOSE);

				Random random = new Random();
				List<int> attemptedNodes = new List<int>();
				do
				{
					int chosenNodeID = random.Next(0, networkNodes.Count);
					Node chosenNode = networkNodes[chosenNodeID];
					PingRequest pingRequest = new PingRequest(chosenNode);
					String result = pingRequest.Process();
					if (result == "OK")
					{
						break;
					}

				} while (attemptedNodes.Count != 0);

				                                      
			}
			catch (Exception e)
			{
				logger.Log("Started new DHT", LoggingLevel.VERBOSE);
				int numRings = int.Parse(ConfigurationManager.AppSettings.Get("NumberOfRings"));
				for (int i = 0; i < numRings; i++)
				{
					Ring newRing = new Ring(int.MinValue, int.MaxValue);
					localNodeRings.Add(newRing);
				}
			}
			finally
			{

			}


			//Application configured -> Start up main loop
			TcpListener serverSocket = new TcpListener(rootNode.GetIPAddress(), rootNode.GetNodeSocket());
			TcpClient clientSocket = default(TcpClient);
			try
			{
				serverSocket.Start();
			}
			catch (Exception e)
			{
				logger.Log("Failed to start socket server. Caught exception: " + e.ToString() + ", error likely due to incorrect IP/socket", LoggingLevel.CRITICAL);
				return;
			}

			if (!manager.Run())
			{
				logger.Log("Failed to start hash table manager. Critical error. Application exiting.", LoggingLevel.CRITICAL);
				return;
			}

			while (true)
			{
				clientSocket = serverSocket.AcceptTcpClient();
				ClientRequestHandler requestHandler = new ClientRequestHandler();
				requestHandler.Start(clientSocket, manager, logger);
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

