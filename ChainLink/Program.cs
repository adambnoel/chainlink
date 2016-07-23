using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
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
			

			IPAddress address = IPAddress.Parse(ConfigurationManager.AppSettings.Get("IPAddress"));
			TcpListener serverSocket = new TcpListener(address, 8386);
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

