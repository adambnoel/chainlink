using System;
namespace ChainLinkCLI
{
	public class ErrorRequest : IRequest
	{
		private String requestText;
		public ErrorRequest(String errorRequestText)
		{
			requestText = errorRequestText;
		}

		public void Process()
		{
			Console.WriteLine("Failed to parse command.");
			Console.WriteLine(requestText + " is invalid.");
		}
	}
}

