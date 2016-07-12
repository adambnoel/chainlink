using System;
namespace ChainLinkCLI
{
	public class RequestCommand : ICommand
	{
		IRequest request = null;
		public RequestCommand(String commandText)
		{
			request = RequestFactory.GetRequest(commandText);
		}

		public void ExecuteCommand()
		{
			
		}
	}
}

