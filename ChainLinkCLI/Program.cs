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
			while (commandLineArgument != "Exit")
			{
				commandLineArgument = Console.ReadLine();
			}
			Console.WriteLine("Shutting down ChainLink CLI");
		}
	}
}
