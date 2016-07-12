using System;
using System.Collections;
using System.Collections.Generic;

namespace ChainLinkCLI
{
	public class ErrorCommand : ICommand
	{
		private String incorrectCommand = String.Empty;
		private List<String> validCommands = new List<String>(new String[] {"config", "request"});
		public ErrorCommand(String commandText)
		{
			incorrectCommand = commandText.Split()[0];
		}
		public void ExecuteCommand()
		{
			Console.WriteLine(incorrectCommand + " is not a valid command");
			Console.WriteLine("");
			Console.WriteLine("Did you mean this?");
			Console.WriteLine("\t\t " + suggestCommand());
		}
		private String suggestCommand()
		{
			return String.Empty;
		}
	}
}

