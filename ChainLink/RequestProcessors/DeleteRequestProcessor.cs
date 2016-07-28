using System;
namespace DHTSharp
{
	public class DeleteRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private String requestKey;
		private Boolean retransmitRequest = true;

		public DeleteRequestProcessor(HashTableManager TableManager, String DeleteRequest)
		{
			tableManager = TableManager;
			String[] splitDeleteRequest = DeleteRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			requestKey = splitDeleteRequest[1];
			if (splitDeleteRequest.Length == 3)
			{
				retransmitRequest = false;
			}
		}

		public String ProcessAndRespond()
		{
			return generateResponse(tableManager.DeleteKey(requestKey, retransmitRequest));
		}

		private String generateResponse(String requestResult)
		{
			String response = "";
			if (requestResult == "OK")
			{
				response = response + "-\r\n";
				response = response + "OK\r\n";
				response = response + "Deleted key: " + requestKey + "\r\n";
			}
			else 
			{
				response = "!\r\n";
				response = response + requestResult + "\r\n";
			}
			return response;
		}
	}
}

