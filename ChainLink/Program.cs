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
			String logFilePath = Path.Combine(ConfigurationManager.AppSettings.Get("FilePath"), ConfigurationManager.AppSettings.Get("FileName"));
			CoreLogger logger = new CoreLogger(ConfigurationManager.AppSettings.Get(logFilePath));
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



			//Application configured -> Start up main loop
			IPAddress address = IPAddress.Parse(ConfigurationManager.AppSettings.Get("IPAddress"));
			int portNumber = 0;
			int.TryParse(ConfigurationManager.AppSettings.Get("PortNumber"), out portNumber);
			if (portNumber == 0)
			{
				logger.Log("Invalid port number provided in app.config, defaulting to 8386.", LoggingLevel.WARNING);
				return;
			}

			TcpListener serverSocket = new TcpListener(address, portNumber);
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

