using System;
namespace DHTSharp
{
	public class PingRequestProcessor : IRequestProcessor
	{
		private String requestString;
		private IHashTableManager tableManager;
		public PingRequestProcessor(IHashTableManager TableManager, String RequestString)
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

