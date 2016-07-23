using System;
using System.Text;

namespace DHTSharp
{
	public class JoinRequest : IRequest
	{
		Node sourceNode;
		Node targetNode;
		public JoinRequest(Node SourceNode, Node TargetNode)
		{
			sourceNode = SourceNode;
			targetNode = TargetNode;
		}

		public String ProcessRequest()
		{
			return "";
		}
		private String initializeJoinRequest()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('$');
			sb.Append("\r\n");
			sb.Append(sourceNode.Serialize()); //Send details about the node that wants to join the network
			sb.Append("\r\n");
			return sb.ToString();
		}
	}
}

