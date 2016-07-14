using System;
namespace ChainLinkCLI
{
	public class CLIParser
	{
		public ICommand ParseCommand(String commandText)
		{
			return CommandFactory.GetCommand(parseCommandType(commandText), commandText);
		}

		private CommandType parseCommandType(String commandText)
		{
			String[] splitText = commandText.Split(' ');
			if (splitText.Length <= 1)
			{
				return CommandType.Error;
			}
			switch (splitText[0])
			{
				case "Request":
					return CommandType.Request;
				case "request":
					return CommandType.Request;
				case "Config":
					return CommandType.Config;
				case "config":
					return CommandType.Config;
				case "Exit":
					return CommandType.Exit;
				case "exit":
					return CommandType.Exit;
				case "q":
					return CommandType.Exit;
				case "Quit":
					return CommandType.Exit;
				case "quit":
					return CommandType.Exit;
				default:
					return CommandType.Error;
			}
		}
	}
}

