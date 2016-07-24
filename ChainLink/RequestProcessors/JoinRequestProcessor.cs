using System;
namespace DHTSharp
{
	public class JoinRequestProcessor : IRequestProcessor
	{
		private IHashTableManager tableManager;
		private Node sourceNode;

		public JoinRequestProcessor(IHashTableManager TableManager, String JoinRequest)
		{
			tableManager = TableManager;
			String[] splitJoinRequest = JoinRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			//sourceNode = Node.Deserialize(splitJoinRequest[1]);
		}

		public String ProcessAndRespond()
		{
			return String.Empty;
		}


	}
}

