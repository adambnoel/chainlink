using System;
namespace DHTSharp
{
	public class GetRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private String requestKey;
		private int kCount = 1;

		public GetRequestProcessor(HashTableManager TableManager, String GetRequest)
		{
			tableManager = TableManager;
			String[] splitGetRequest = GetRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			requestKey = splitGetRequest[1];
			if (splitGetRequest.Length > 2)
			{
				int newKCount = 0;
				if (int.TryParse(splitGetRequest[2], out newKCount))
				{
					kCount = newKCount;
				}
			}
		}

		public String ProcessAndRespond()
		{
			String value = tableManager.GetValue(requestKey, kCount);
			return generateResponse(value);
		}

		private String generateResponse(String value)
		{
			String response = String.Empty;
			response = "*\r\n";
			if (value == String.Empty)
			{
				response = response + "ERROR - Key not found\r\n";
			}
			else {
				response = response + "OK\r\n";
				response = response + value + "\r\n";
			}
			return response;
		}
	}
}

