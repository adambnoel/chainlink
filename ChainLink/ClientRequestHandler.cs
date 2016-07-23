using System;
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
		private IHashTableManager tableManager;
		Thread clientThread;
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
				return false;
			}
			else {
				return true;
			}
		}

		private void HandleRequest()
		{
			NetworkStream incomingStream = connectedClient.GetStream();
			byte[] bytesFrom = new byte[2000000];
			incomingStream.Read(bytesFrom, 0, bytesFrom.Length);
			String clientRequest = Encoding.ASCII.GetString(bytesFrom);
			if (clientRequest.LastIndexOf("\r\n", StringComparison.Ordinal) == -1) //Invalid request or connection closed
			{

			}
		}
	}
}

