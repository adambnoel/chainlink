using System;
namespace ChainLinkCLI
{
	public class CLIParser
	{
		public ICommand ParseCommand(String commandText)
		{
			ICommand command = null;
			CommandFactory.GetCommand(parseCommandType(commandText), commandText);
			return command;
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
				default:
					return CommandType.Error;
			}
		}
	}
}

