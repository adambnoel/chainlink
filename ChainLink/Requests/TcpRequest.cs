using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DHTSharp
{
	public class TcpRequest
	{
		private IPAddress address;
		private int port;
		public TcpRequest(IPAddress targetAddress, int targetPort)
		{
			address = targetAddress;
			port = targetPort;
		}
		public String Send(String requestContents)
		{
			TcpClient clientSocket = new TcpClient();
			clientSocket.Connect(address, port);
			NetworkStream serverStream = clientSocket.GetStream();
			byte[] requestBytes = Encoding.ASCII.GetBytes(requestContents);
			serverStream.Write(requestBytes, 0, requestBytes.Length);
			serverStream.Flush();

			byte[] inputStream = new byte[4000000];
			serverStream.Read(inputStream, 0, 4000000);
			clientSocket.Close();
			return Encoding.ASCII.GetString(inputStream);
		}
	}
}

