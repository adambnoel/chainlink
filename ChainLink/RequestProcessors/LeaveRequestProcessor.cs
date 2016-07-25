using System;
namespace DHTSharp
{
	public class LeaveRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private Node leavingNode;

		public LeaveRequestProcessor(HashTableManager TableManager, String LeaveRequest)
		{
			tableManager = TableManager;
			//Serialize node from request
		}

		public String ProcessAndRespond()
		{
			return generateResponse(tableManager.RequestLeaveNetwork(leavingNode));
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

