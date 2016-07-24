using System;
namespace DHTSharp
{
	public class ErrorRequestProcessor : IRequestProcessor
	{
		private string errorRequest;
		public ErrorRequestProcessor(String ErrorRequest)
		{
			errorRequest = ErrorRequest;
		}

		public String ProcessAndRespond()
		{
			return generateResponse(errorRequest);
		}

		private String generateResponse(String errorRequest)
		{
			String response = "!\r\n";
			response = response + "Incorrect request format\r\n";
			response = response + errorRequest + "\r\n";
			return response;
		}
	}
}

