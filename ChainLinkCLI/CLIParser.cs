using System;
namespace ChainLinkCLI
{
	public class CLIParser
	{
		public ICommand ParseCommand(String commandText)
		{
			ICommand command = null;
			CommandFactory.GetCommand(parseCommandType(commandText), parseCommandDetails(commandText));
			return command;
		}

		private CommandType parseCommandType(String commandText)
		{
			return CommandType.Config;
		}

		private String parseCommandDetails(String commandText)
		{
			String details = String.Empty;

			return details;
		}
	}
}

