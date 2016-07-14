using System;
using System.Configuration;
using System.Configuration.Install;
using System.Net;
using System.Net.Sockets;

namespace ChainLinkCLI
{
	public class CLIConfigManager
	{
		
		public CLIConfigManager()
		{
		}

		public Boolean SetIPAddress(String newIPAddress)
		{
			ConfigurationManager.AppSettings.Set("IPAddress", newIPAddress);
			if (ConfigurationManager.AppSettings.Get("IPAddress") == newIPAddress)
			{
				return true;
			}
			return false;
		}

		public Boolean SetPortNumber(int portNumber)
		{
			ConfigurationManager.AppSettings.Set("PortNumber", portNumber.ToString());
			if (int.Parse(ConfigurationManager.AppSettings.Get("PortNumber")) == portNumber)
			{
				return true;
			}
			return false;
		}

		public Boolean SetMaxRequestSize(String maxRequestSize)
		{
			return true;
		}

		public String GetIPAddressString()
		{
			return ConfigurationManager.AppSettings.Get("IPAddress");
		}

		public IPAddress GetIPAddress()
		{
			return IPAddress.Parse(ConfigurationManager.AppSettings.Get("IPAddress"));
		}

		public int GetPortNumber()
		{
			return int.Parse(ConfigurationManager.AppSettings.Get("PortNumber"));
		}

		public int GetMaxRequestSize()
		{
			return parseRequestSize(ConfigurationManager.AppSettings.Get("MaxRequestSize"));
		}

		private int parseRequestSize(String requestString)
		{
			return 0;
		}
	}
}

