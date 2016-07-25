using System;
namespace DHTSharp
{
	public class PingRequestProcessor : IRequestProcessor
	{
		private String requestString;
		private HashTableManager tableManager;
		public PingRequestProcessor(HashTableManager TableManager, String RequestString)
		{
			requestString = RequestString;
			tableManager = TableManager;
		}

		public String ProcessAndRespond()
		{
			return "";
		}
	}
}

