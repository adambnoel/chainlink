using System;
namespace ChainLinkCLI
{
	public class CommandFactory
	{
		public static ICommand GetCommand(CommandType commandType, String commandText)
		{
			switch (commandType)
			{
				case CommandType.Config:
					return new ConfigCommand(commandText);
				case CommandType.Request:
					return new RequestCommand(commandText);
				case CommandType.Exit:
					return new ExitCommand();
				default:
					return new ErrorCommand(commandText);

			}
		}
	}
}

