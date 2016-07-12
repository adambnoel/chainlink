using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChainLinkCLI
{
	public class TcpRequest
	{
		private IPAddress address;
		private int port;
		private int maxRequestSize;
		public TcpRequest(IPAddress targetAddress, int targetPort, int targetMaxRequestSize)
		{
			address = targetAddress;
			port = targetPort;
			maxRequestSize = targetMaxRequestSize;
		}
		public String Send(String requestContents)
		{
			TcpClient clientSocket = new TcpClient();
			NetworkStream serverStream = clientSocket.GetStream();
			byte[] requestBytes = Encoding.ASCII.GetBytes(requestContents);
			serverStream.Write(requestBytes, 0, requestBytes.Length);
			serverStream.Flush();

			byte[] inputStream = new byte[maxRequestSize];
			serverStream.Read(inputStream, 0, maxRequestSize);
			return Encoding.ASCII.GetString(inputStream);
		}
	}
}

