using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DHTSharp
{
	//This class
	public class HashTableManager
	{
		private Node currentNode;
		private volatile bool higherPriorityTaskLock = false;
		private Semaphore networkNodeLock = new Semaphore(1, 1);
		private List<Node> networkNodes = new List<Node>();

		private Queue<Node> recentNodePings = new Queue<Node>();
		private Timer updateNodeTimer;
		private Queue<Node> newNodes = new Queue<Node>();
		private Timer newNodeTimer;

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

		public void PingNetworkNode(Node sourceNode)
		{
			Node relevantNode = (from x in networkNodes
								 where x.GetIPAddress().Equals(sourceNode.GetIPAddress())
								 && x.GetHashCode().Equals(sourceNode.GetHashCode())
								 select x).FirstOrDefault();
			recentNodePings.Enqueue(sourceNode);
		}

		public String RequestJoinNetwork(Node node)
		{
			
			String joinRequestResponse = "^\r\n";
			List<Ring> newRings = currentNode.SplitNodeRings();
			if (newRings.Count == 0)
			{
				logger.Log("Failed to split rings - may indicate underlying issue", LoggingLevel.WARNING);
				return "!\r\nFailed to join network. Rings could not be split";
			}
			foreach (Ring newRing in newRings)
			{
				logger.Log("Responding to join request with details of one ring", LoggingLevel.DEBUGGING);
				node.AddRing(newRing);
				joinRequestResponse = joinRequestResponse + newRing.GetHashRangeStart() + "," + newRing.GetHashRangeEnd() + "\r\n";
			}
			AddNetworkNode(node); //Add new node to network -> 
			newNodes.Enqueue(node); //Don't want to block the DHT while transferring files. Do that later
			logger.Log("Sending join response", LoggingLevel.DEBUGGING);
			return joinRequestResponse;
		}

		public String RequestLeaveNetwork(Node node)
		{
			String leaveNetworkResponse = "$\r\n";


			return leaveNetworkResponse;
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
				if (valueBytes == null || valueBytes.Length == 0)
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

		public void AddNetworkNode(Node node)
		{
			networkNodeLock.WaitOne();
			try
			{
				networkNodes.Add(node);
			}
			finally
			{
				networkNodeLock.Release();
			}
		}

		public Boolean RemoveNetworkNode(Node node)
		{
			networkNodeLock.WaitOne();
			try
			{
				networkNodes.Remove(node);
			}
			finally
			{
				networkNodeLock.Release();
			}
			return true;
		}

		//Transfer keys to new node
		private void newNodeTransferTask(Object state)
		{
			higherPriorityTaskLock = true;
			networkNodeLock.WaitOne();
			try
			{
				while (newNodes.Count != 0)
				{
					try
					{
						Node newNode = newNodes.Dequeue();
						List<String> keysToMigrate = new List<String>();

						//List<String> keysToMigrate = hashTableWrapper.GetKeysWithinHashrange(newNode.
					}
					finally
					{

					}
				}
			}
			finally
			{
				higherPriorityTaskLock = false;
				networkNodeLock.Release();
			}
		}

		//This task deals with nodes that joined the network from sending a join request to this node
		private void newNodeTask(Object state)
		{
			//Attain a lock on the network node object until all new nodes are added
			networkNodeLock.WaitOne();
			try
			{
				while (newNodes.Count != 0)
				{
					try
					{
						Node newNode = newNodes.Dequeue();
						Boolean NodeIsNew = ((from x in networkNodes
													  where x.GetIPAddress().Equals(newNode.GetIPAddress())
													  && x.GetHashCode().Equals(newNode.GetHashCode())
													  select x).FirstOrDefault() == null);
						if (NodeIsNew)
						{

						}
					}
					finally
					{

					}
				}
			}
			finally
			{
				networkNodeLock.Release();
			}

		}

		private void updateNodeTask(Object state)
		{
			
			while (recentNodePings.Count != 0)
			{
				networkNodeLock.WaitOne();
				try
				{
					Node pingSourceNode = recentNodePings.Dequeue();
					var relevantNode = (from x in networkNodes
										where x.GetIPAddress().Equals(pingSourceNode.GetIPAddress())
										&& x.GetHashCode().Equals(pingSourceNode.GetHashCode())
										select x).FirstOrDefault();
					if (relevantNode != null) //Node was removed -> move on
					{
						relevantNode.SetLastPingTimeUtc(pingSourceNode.GetLastPingTimeUtc());
					}
				}
				finally
				{
					networkNodeLock.Release(); //Only hold for servicing one ping request
				}
			}
		}

		private void removeNodeTask(Object state)
		{

		}

		private void pingTask(Object state)
		{
			logger.Log("Running ping task", LoggingLevel.DEBUGGING);
			foreach (Node node in networkNodes)
			{
				if (!node.RecentlyPinged())
				{
					PingRequest newPingRequest = new PingRequest(node);
					String response = newPingRequest.Process();
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
			finally
			{
				clientRequestHandlerLock.Release();
			}
		}
	}
}

