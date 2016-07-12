using System;
namespace ChainLinkCLI
{
	public class RequestCommand : ICommand
	{
		private String text;
		public RequestCommand(String commandText)
		{
			text = commandText;
		}

		public void ExecuteCommand()
		{
			
		}
	}
}

