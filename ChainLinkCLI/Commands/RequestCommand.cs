using System;
namespace ChainLinkCLI
{
	public class RequestCommand : ICommand
	{
		private String context;
		public RequestCommand(String commandContext)
		{
			context = commandContext;
		}

		public void ExecuteCommand()
		{
			
		}
	}
}

