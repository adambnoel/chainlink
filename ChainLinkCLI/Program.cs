using System;

namespace ChainLinkCLI
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			String commandLineArgument = String.Empty;
			CLIParser parser = new CLIParser();
			Console.WriteLine("Starting ChainLink CLI");
			Console.WriteLine("Enter command");
			while (commandLineArgument != "Exit")
			{
				commandLineArgument = Console.ReadLine();
				ICommand parsedCommand = parser.ParseCommand(commandLineArgument);
				parsedCommand.ExecuteCommand();
			}
			Console.WriteLine("Exiting ChainLink CLI");
		}
	}
}
