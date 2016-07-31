using System;
namespace DHTSharp
{
	public class GossipRequestProcessor : IRequestProcessor
	{
		private String requestString;
		private HashTableManager tableManager;
		public GossipRequestProcessor(HashTableManager TableManager, String RequestString)
		{
			requestString = RequestString;
			tableManager = TableManager;
		}

		public String ProcessAndRespond()
		{
			tableManager.ProcessGossipRequest(requestString);
			return "#\r\n";
		}
	}
}

