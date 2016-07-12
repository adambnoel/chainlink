using System;
namespace ChainLinkCLI
{
	public class ConfigCommand : ICommand
	{
		private String text;
		public ConfigCommand(String commandText)
		{
			text = commandText;
		}

		public void ExecuteCommand()
		{
			
		}
	}
}

