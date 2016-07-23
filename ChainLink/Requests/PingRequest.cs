using System;
namespace DHTSharp
{
	public class PingRequest : IRequest
	{
		private Node targetNode;
		public PingRequest(Node TargetNode)
		{
			targetNode = TargetNode;
		}

		public String ProcessRequest()
		{
			return "";
		}
	}
}

