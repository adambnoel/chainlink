﻿using System;
namespace DHTSharp
{
	public class DeleteRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private String requestKey;

		public DeleteRequestProcessor(HashTableManager TableManager, String DeleteRequest)
		{
			tableManager = TableManager;
			String[] splitDeleteRequest = DeleteRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			requestKey = splitDeleteRequest[1];

		}

		public String ProcessAndRespond()
		{
			return generateResponse(tableManager.DeleteKey(requestKey));
		}

		private String generateResponse(String requestResult)
		{
			String response = "-\r\n";
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

