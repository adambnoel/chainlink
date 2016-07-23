using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DHTSharp
{
	//This class
	public class SHA1HashTableManager
	{
		private Node currentNode;
		private Semaphore networkNodeLock = new Semaphore(1, 1);
		private List<Node> networkNodes = new List<Node>();

		private Timer pingTimer;
		private Timer gossipTimer;
		private Timer connectionCheckTimer;
		private Timer maintenanceTimer;


		private Semaphore clientRequestHandlerLock = new Semaphore(1, 1);
		private List<ClientRequestHandler> clientRequestHandlers = new List<ClientRequestHandler>();

		public SHA1HashTableManager(Node CurrentNode, List<Node> NetworkNodes)
		{
			currentNode = CurrentNode;
			networkNodes = NetworkNodes;
		}

		public Boolean Run()
		{
			pingTimer = new Timer(pingTask, this, 1000 * 30, 1000 * 60);
			gossipTimer = new Timer(gossipTask, this, 1000 * 60, 1000 * 30);
			connectionCheckTimer = new Timer(connectionCheckTask, this, 1000 * 30, 1000 * 30);
			maintenanceTimer = new Timer(hashTableMaintenanceTask, this, 1000 * 300, 1000 * 300);
			return true;
		}

		public Boolean AddRequestHandler(ClientRequestHandler clientRequestHandler)
		{
			clientRequestHandlers.Add(clientRequestHandler);
			return true;
		}

		public Boolean RequestJoinNetwork(Node node)
		{
			//A node

			return true;
		}

		public Boolean RequestLeaveNetwork(Node node)
		{
			return true;
		}

		public Boolean AddNetworkNode(Node node)
		{
			networkNodes.Add(node);
			return true;
		}

		public Boolean RemoveNetworkNode(Node node)
		{
			networkNodes.Add(node);
			return true;
		}

		private void pingTask(Object state)
		{
			
			foreach (Node node in networkNodes)
			{
				PingRequest newPingRequest = new PingRequest(node);
				if (newPingRequest.ProcessRequest() != "OK\r\n")
				{
					if (node.FailedNodePulse())
					{
						//Assume the node has dropped
					}
				}
				else {
					node.ResetFailedPingCount();
				}
			}
		}

		private void gossipTask(Object state)
		{
			foreach (Node node in networkNodes)
			{
				/**	
				GossipRequest newGossipRequest = new GossipRequest(node);
				if (newGossipRequest.ProcessRequest() != "OK\r\n")
				{

				}
				**/
			}
		}

		private void hashTableMaintenanceTask(Object state)
		{
			
		}

		private void connectionCheckTask(Object state)
		{
			clientRequestHandlerLock.WaitOne();
			try
			{
				List<ClientRequestHandler> inactiveHandlers = new List<ClientRequestHandler>();
				foreach (ClientRequestHandler handler in clientRequestHandlers)
				{
					if (!handler.IsActive())
					{
						handler.Stop();
						inactiveHandlers.Add(handler);
					}
				}
				foreach (ClientRequestHandler handler in inactiveHandlers)
				{
					clientRequestHandlers.Remove(handler);
				}
			}
			catch (Exception e)
			{
				clientRequestHandlerLock.Release();
			}
		}
	}
}

