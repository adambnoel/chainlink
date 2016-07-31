using System;
namespace DHTSharp
{
	public class PingRequestProcessor : IRequestProcessor
	{
		private HashTableManager tableManager;
		private Node sourceNode;

		public PingRequestProcessor(HashTableManager TableManager, String RequestString, Node SourceNode)
		{
			String[] splitRequestString = RequestString.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			tableManager = TableManager;
			sourceNode = SourceNode;
			sourceNode.SetLastPingTimeUtc(DateTime.Parse(splitRequestString[1]));
		}

		public String ProcessAndRespond()
		{
			return generateResponse();
		}

		private String generateResponse()
		{
			String response = String.Empty;
			response = "@\r\n";
			response = response + DateTime.UtcNow + "\r\n";
			return response;
		}
	}
}

