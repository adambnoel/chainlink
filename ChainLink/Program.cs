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
			Console.WriteLine("Derp?");
			IPAddress address = IPAddress.Parse(ConfigurationManager.AppSettings.Get("IPAddress"));
			TcpListener serverSocket = new TcpListener(address, 8386);
			TcpClient clientSocket = default(TcpClient);
			serverSocket.Start();

			while (true)
			{
				clientSocket = serverSocket.AcceptTcpClient();
				NetworkStream networkStream = clientSocket.GetStream();
				byte[] bytesFrom = new byte[10025];
				networkStream.Read(bytesFrom, 0, bytesFrom.Length);

				string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
				dataFromClient = dataFromClient.Substring(0, dataFromClient.LastIndexOf("\r\n", System.StringComparison.Ordinal));
				Console.WriteLine(" >> Data from client - " + dataFromClient);
				string serverResponse = "Last Message from client" + dataFromClient;
				Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
				networkStream.Write(sendBytes, 0, sendBytes.Length);
				networkStream.Flush();
				Console.WriteLine(" >> " + serverResponse);
			}

			serverSocket.Stop();
			clientSocket.Close();
		}

		private static void HandleRequest()
		{
			
		}


	}
}

