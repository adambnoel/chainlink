using System;
namespace DHTSharp
{
	public class PutRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private String requestKey;
		private String requestBody;
		private int kCount = 1;

		public PutRequestProcessor(HashTableManager TableManager, String PutRequest)
		{
			tableManager = TableManager;
			String[] splitPutRequest = PutRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			requestKey = splitPutRequest[1];
			if (splitPutRequest.Length == 3)
			{
				requestBody = splitPutRequest[2];
			}
			else 
			{
				int newKCount = 0;
				if (int.TryParse(splitPutRequest[2], out newKCount))
				{
					kCount = newKCount;
				}
				requestBody = splitPutRequest[3];
			}
		}

		public String ProcessAndRespond()
		{
			return generateResponse(tableManager.PutKey(requestKey, requestBody, kCount));
		}

		private String generateResponse(String requestResult)
		{
			String response = "+\r\n";
			if (requestResult == "OK")
			{
				response = response + "OK\r\n";
			}
			else 
			{
				response = response + requestResult + "\r\n";
			}
			return response;
		}
	}
}

