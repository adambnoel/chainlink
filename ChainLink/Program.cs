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
				XDocument networkFile = XDocument.Load(ConfigurationManager.AppSettings.Get("SeedNodeFile"));
				var networkNodes = networkFile.Root
											  .Elements("Node")
											  .Select(x => new Node(new List<Ring>(),
															IPAddress.Parse((string)x.Attribute("ip")),
															int.Parse((string)x.Attribute("port"))))
											  .ToList();
				if (networkNodes.Count == 0)
				{
					throw new Exception("Failed to seed network file. Defaulting to acting as root node");
				}
				logger.Log("Parsed network file. Trying to join network", LoggingLevel.VERBOSE);
				Random random = new Random();
				List<Ring> assignedRings = new List<Ring>();
				do
				{
					int chosenNodeID = random.Next(0, networkNodes.Count);
					Node chosenNode = networkNodes[chosenNodeID];
					JoinRequest joinRequest = new JoinRequest(rootNode, chosenNode);
					String result = joinRequest.Process();
					if (result != String.Empty)
					{
						assignedRings = Ring.ParseJoinRequest(result);
						break;
					}
					networkNodes.Remove(chosenNode); //Didn't work -> don't try and contact again

				} while (networkNodes.Count != 0);
				if (assignedRings.Count == 0)
				{
					logger.Log("Failed to contact any network nodes. Starting as root node", LoggingLevel.WARNING);
					throw new Exception("Failed to join network during startup. Will boot as root node.");
				}
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

			if (!manager.Run())
			{
				logger.Log("Failed to start hash table manager. Critical error. Application exiting.", LoggingLevel.CRITICAL);
				return;
			}
			else
			{
				logger.Log("Started hash table manager.", LoggingLevel.VERBOSE);
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

			while (true)
			{
				clientSocket = serverSocket.AcceptTcpClient();
				ClientRequestHandler requestHandler = new ClientRequestHandler();
				requestHandler.Start(clientSocket, manager, logger);
			}

			serverSocket.Stop();
			clientSocket.Close();
		}


	}
}

