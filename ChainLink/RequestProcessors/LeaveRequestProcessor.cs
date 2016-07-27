using System;
using System.Collections.Generic;

namespace DHTSharp
{
	public class LeaveRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private Node leavingNode;
		private List<Ring> nodeRings;

		public LeaveRequestProcessor(HashTableManager TableManager, String LeaveRequest)
		{
			tableManager = TableManager;
			//Serialize node from request
		}

		public String ProcessAndRespond()
		{
			return generateResponse(tableManager.RequestLeaveNetwork(leavingNode, nodeRings));
		}

		private String generateResponse(String requestResult)
		{
			String response = "$\r\n";
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

