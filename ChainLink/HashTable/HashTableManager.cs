using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DHTSharp
{
	//This class
	public class HashTableManager
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
		private CoreLogger logger;

		HashTableWrapper hashTableWrapper = new HashTableWrapper();
		public HashTableManager(Node CurrentNode, List<Node> NetworkNodes, HashTableWrapper hashTableImplementation, CoreLogger Logger)
		{
			currentNode = CurrentNode;
			networkNodes = NetworkNodes;
			hashTableWrapper = hashTableImplementation;
			logger = Logger;
		}

		public Boolean Run()
		{
			logger.Log("Scheduling all Hash Table manager tasks", LoggingLevel.VERBOSE);
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

		public String RequestJoinNetwork(Node node)
		{
			//A node

			return "";
		}

		public String RequestLeaveNetwork(Node node)
		{
			return "";
		}

		public String DeleteKey(String key)
		{
			byte[] valueBytes = new byte[0];
			if (currentNode.CheckNodeRingsForKey(key.GetHashCode()))
			{
				valueBytes = hashTableWrapper.Delete(key);
				if (valueBytes.Length == 0)
				{
					return "Failed to delete key - either key already deleted or resource locked.";
				}
				else {
					return "Deleted key: " + key;
				}
			}
			else {
				for (int i = 0; i < networkNodes.Count; i++)
				{
					if (networkNodes[i].CheckNodeRingsForKey(key.GetHashCode()))
					{
						DeleteRequest deleteRequest = new DeleteRequest(key, networkNodes[i]);
						return deleteRequest.Process();
					}
				}
				return "Failed to delete key - node unaware of key's hashspace";
			}
		}

		public String GetValue(String key)
		{
			byte[] valueBytes = new byte[0];
			if (currentNode.CheckNodeRingsForKey(key.GetHashCode()))
			{
				valueBytes = hashTableWrapper.Get(key);
				if (valueBytes.Length == 0)
				{
					return "";
				}
				else {
					return Encoding.ASCII.GetString(valueBytes);
				}
			}
			else {
				
				for (int i = 0; i < networkNodes.Count; i++)
				{
					if (networkNodes[i].CheckNodeRingsForKey(key.GetHashCode()))
					{
						GetRequest getRequest = new GetRequest(key, networkNodes[i]);
						return getRequest.Process();
					}
				}
				return "";
			}
		}

		public String PutKey(String key, String contents)
		{
			
			if (currentNode.CheckNodeRingsForKey(key.GetHashCode()))
			{
				if (hashTableWrapper.Put(key, Encoding.ASCII.GetBytes(contents)))
				{
					return "OK";
				}
				else
				{
					return "ERROR - Failed to put key";
				}
			}
			else 
			{
				for (int i = 0; i < networkNodes.Count; i++)
				{
					if (networkNodes[i].CheckNodeRingsForKey(key.GetHashCode()))
					{
						PutRequest putRequest = new PutRequest(key, contents, networkNodes[i]);
						return putRequest.Process();
					}
				}
				return "ERROR - Failed to find key";
			}
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
			logger.Log("Running ping task", LoggingLevel.DEBUGGING);
			foreach (Node node in networkNodes)
			{
				PingRequest newPingRequest = new PingRequest(node);
				if (newPingRequest.Process() != "OK\r\n")
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
			logger.Log("Finished ping task", LoggingLevel.DEBUGGING);
		}

		private void gossipTask(Object state)
		{
			logger.Log("Running gossip task", LoggingLevel.DEBUGGING);
			foreach (Node node in networkNodes)
			{
				/**	
				GossipRequest newGossipRequest = new GossipRequest(node);
				if (newGossipRequest.ProcessRequest() != "OK\r\n")
				{

				}
				**/
			}
			logger.Log("Finished gossip task", LoggingLevel.DEBUGGING);
		}

		private void hashTableMaintenanceTask(Object state)
		{
			logger.Log("Running maintenance task", LoggingLevel.DEBUGGING);


			logger.Log("Finished maintenance task", LoggingLevel.DEBUGGING);
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

