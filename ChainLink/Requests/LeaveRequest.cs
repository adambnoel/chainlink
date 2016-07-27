using System;
namespace DHTSharp
{
	public class LeaveRequest : IRequest
	{
		private Node leavingNode;
		private Node destinationNode;

		public LeaveRequest(Node LeavingNode, Node DestinationNode)
		{
			leavingNode = LeavingNode;
			destinationNode = DestinationNode;
		}

		public String Process()
		{
			TcpRequest request = new TcpRequest(destinationNode.GetIPAddress(), destinationNode.GetNodeSocket());
			return request.Send(initializeLeaveRequest());
		}

		private String initializeLeaveRequest()
		{
			String leaveRequest = String.Empty;
			leaveRequest = leaveRequest + "$\r\n";
			leaveRequest = leaveRequest + leavingNode.GetIPAddress().ToString() + "\r\n";
			leaveRequest = leaveRequest + leavingNode.GetNodeSocket().ToString() + "\r\n";
			leaveRequest = leaveRequest + leavingNode.GetSerializedRings();
			return leaveRequest;
		}
	}
}

