using System;
namespace DHTSharp
{
	public class GossipRequestProcessor : IRequestProcessor
	{
		private String requestString;
		private IHashTableManager tableManager;
		public GossipRequestProcessor(IHashTableManager TableManager, String RequestString)
		{
			requestString = RequestString;
			tableManager = TableManager;
		}

		public String ProcessAndRespond()
		{
			return String.Empty;
		}
	}
}

