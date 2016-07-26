using System;
namespace DHTSharp
{
	public class GetRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private String requestKey;

		public GetRequestProcessor(HashTableManager TableManager, String GetRequest)
		{
			tableManager = TableManager;
			String[] splitGetRequest = GetRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			requestKey = splitGetRequest[1];
		}

		public String ProcessAndRespond()
		{
			String value = tableManager.GetValue(requestKey);
			return generateResponse(value);
		}

		private String generateResponse(String value)
		{
			String response = String.Empty;
			if (value == String.Empty)
			{
				response = "!\r\n";
				response = response + "ERROR - Key not found\r\n";
			}
			else {
				response = "*\r\n";
				response = response + "OK\r\n";
				response = response + value + "\r\n";
			}
			return response;
		}
	}
}

