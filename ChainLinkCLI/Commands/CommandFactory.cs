using System;
namespace ChainLinkCLI
{
	public class CommandFactory
	{
		public static ICommand GetCommand(CommandType commandType, String commandText)
		{
			ICommand command = null;
			switch (commandType)
			{
				case CommandType.Config:
					command = new ConfigCommand(commandText);
					break;
				case CommandType.Request:
					command = new ConfigCommand(commandText);
					break;
				default:
					command = new ErrorCommand(commandText);
					break;
			}
			return command;
		}
	}
}

