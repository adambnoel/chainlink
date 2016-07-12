using System;
namespace ChainLinkCLI
{
	public class ConfigCommand : ICommand
	{
		private String context;
		public ConfigCommand(String commandContext)
		{
			context = commandContext;
		}

		public void ExecuteCommand()
		{
			
		}
	}
}

