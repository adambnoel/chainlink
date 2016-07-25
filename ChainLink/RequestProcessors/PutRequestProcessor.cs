﻿using System;
namespace DHTSharp
{
	public class PutRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private String requestKey;
		private String requestBody;

		public PutRequestProcessor(HashTableManager TableManager, String PutRequest)
		{
			tableManager = TableManager;
			String[] splitPutRequest = PutRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			requestKey = splitPutRequest[1];
			requestBody = splitPutRequest[2];
		}

		public String ProcessAndRespond()
		{
			return generateResponse(tableManager.PutKey(requestKey, requestBody));
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

