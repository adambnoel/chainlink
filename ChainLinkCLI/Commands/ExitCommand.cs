using System;
namespace ChainLinkCLI
{
	public class ExitCommand : ICommand
	{
		public void ExecuteCommand()
		{
			Console.WriteLine("Preparing to exit...");
		}
	}
}

