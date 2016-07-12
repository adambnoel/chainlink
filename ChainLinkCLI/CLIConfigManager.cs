using System;
using System.Configuration;
using System.Configuration.Install;

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
	}
}

