using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

		private Queue<Node> newNodes = new Queue<Node>();
		private Timer newNodeTimer;
		private Timer pingTimer;
		private Timer connectionCheckTimer;

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
			pingTimer = new Timer(pingTask, this, 1000 * 60, 1000 * 60);
			connectionCheckTimer = new Timer(connectionCheckTask, this, 1000 * 60, 1000 * 60);
			newNodeTimer = new Timer(newNodeTransferTask, this, 1000 * 60, 1000 * 60);
			return true;
		}

		public Boolean AddRequestHandler(ClientRequestHandler clientRequestHandler)
		{
			clientRequestHandlers.Add(clientRequestHandler);
			return true;
		}

		//This method would be used by the service to leave the network
		public void LeaveNetwork()
		{
			
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

		public String RequestLeaveNetwork(Node node, List<Ring> nodeRings)
		{
			if (currentNode.MergeRings(nodeRings))
			{
				String leaveNetworkResponse = "$\r\nOK\r\n"; //This signals to the leaving table to transmit over all keys
				networkNodes.Remove(node);
				return leaveNetworkResponse;
			}
			else {
				return "!\r\nFailed to process leave request. Cannot merge rings";
			}
		}

		public void ProcessGossipRequest(String gossipRequestString)
		{
			gossipRequestString = gossipRequestString.Substring(0, gossipRequestString.LastIndexOf("\r\n", StringComparison.Ordinal));
			String[] splitString = gossipRequestString.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			if (splitString[0] == "#" && splitString.Length >= 4)
			{
				
			}
		}

		public String DeleteKey(String key, Boolean retransmission)
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
					if (retransmission)
					{
						Thread consistencyThread = new Thread(() => deleteConsistencyTask(key));
						consistencyThread.Start();
					}
					return "Deleted key: " + key;
				}

			}
			else {
				for (int i = 0; i < networkNodes.Count; i++)
				{
					if (networkNodes[i].CheckNodeRingsForKey(key.GetHashCode()))
					{
						DeleteRequest deleteRequest = new DeleteRequest(key, networkNodes[i], true);
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

		public String PutKey(String key, String contents, Boolean retransmission)
		{
			if (currentNode.CheckNodeRingsForKey(key.GetHashCode()))
			{
				if (hashTableWrapper.Put(key, Encoding.ASCII.GetBytes(contents)))
				{
					if (retransmission)
					{
						Thread consistencyThread = new Thread(() => putConsistencyTask(key, contents));
						consistencyThread.Start();
					}
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
						PutRequest putRequest = new PutRequest(key, contents, networkNodes[i], retransmission);
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
			Boolean shouldGossip = false;
			while (newNodes.Count != 0)
			{
				shouldGossip = true; //Gossip network changes once done
				try
				{
					Node addedNode = newNodes.Dequeue();
					Node relevantNode = (from x in networkNodes
										 where x.GetIPAddress() == addedNode.GetIPAddress()
										 && x.GetNodeSocket() == addedNode.GetNodeSocket()
										 select x).FirstOrDefault();
					if (relevantNode != null)
					{
						Monitor.Enter(relevantNode);
						try
						{
							List<String> keysToMigrate = relevantNode.GetKeysWithinHashTable(hashTableWrapper);
							foreach (String keyToMigrate in keysToMigrate)
							{
								byte[] transferPayload;
								if (currentNode.CheckNodeRingsForKey(keyToMigrate.GetHashCode())) //Edge case when rings are overlapping
								{ 
									transferPayload = hashTableWrapper.Get(keyToMigrate);
								}
								else 
								{
									transferPayload = hashTableWrapper.Delete(keyToMigrate);
								}
								PutRequest putRequest = new PutRequest(keyToMigrate, Encoding.ASCII.GetString(transferPayload), relevantNode, false);
								Thread transferThread = new Thread(() => keyTransferTask(putRequest));
								transferThread.Start();

							}
						}
						finally
						{
							Monitor.Exit(relevantNode);
						}
					}
				}
				catch (Exception e) //Just in case multiple of these threads spawn
				{

				}
			}
			if (shouldGossip)
			{
				Thread gossipThread = new Thread(gossipTask);
			}
		}

		private void pingTask(Object state)
		{
			foreach (Node node in networkNodes)
			{
				Monitor.Enter(node);
				try
				{
					if (!node.RecentlyPinged())
					{
						logger.Log("Pinging network node: " + node.GetIPAddress().ToString() + "," + node.GetNodeSocket(), LoggingLevel.DEBUGGING);
						PingRequest newPingRequest = new PingRequest(node);
						String response = newPingRequest.Process();
						if (response != String.Empty)
						{
							response = response.Substring(0, response.LastIndexOf("\r\n", StringComparison.Ordinal));
							String[] splitPingResponse = response.Split(new String[] { "\r\n" }, StringSplitOptions.None);
							if (splitPingResponse[0] == "@" && splitPingResponse.Length <= 2)
							{
								DateTime lastResponseTime = DateTime.UtcNow;
								if (DateTime.TryParse(splitPingResponse[1], out lastResponseTime))
								{
									node.SetLastPingTimeUtc(lastResponseTime);
								}
								logger.Log("Got response from network node: " + node.GetIPAddress().ToString() + "," + node.GetNodeSocket(), LoggingLevel.DEBUGGING);
							}
						}
						else
						{
							if (!node.CheckHeartbeat())
							{
								if (currentNode.MergeRings(node.GetNodeRings()))
								{
									logger.Log("Other network node gone offline - merged rings into node", LoggingLevel.VERBOSE);
									RemoveNetworkNode(node);
									Thread gossipThread = new Thread(gossipTask);
								}
								else 
								{
									logger.Log("Other network node gone offline. Cannot merge rings. Will leave to other network node", LoggingLevel.VERBOSE);
									RemoveNetworkNode(node);
								}
							}
						}
					}
				}
				finally
				{
					Monitor.Exit(node);
				}
			}
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

		private void putConsistencyTask(String key, String contents)
		{
			foreach (Node node in networkNodes)
			{
				if (node.CheckNodeRingsForKey(key.GetHashCode())) {
					PutRequest putRequest = new PutRequest(key, contents, node, false);
					Thread newThread = new Thread(() => keyTransferTask(putRequest));
					newThread.Start();
				}
			}
		}

		private void deleteConsistencyTask(String key)
		{
			foreach (Node node in networkNodes)
			{
				if (node.CheckNodeRingsForKey(key.GetHashCode()))
				{
					DeleteRequest deleteRequest = new DeleteRequest(key, node, false);

				}
			}
		}

		private void keyTransferTask(PutRequest request)
		{
			try
			{
				request.Process();
			}
			catch (Exception e)
			{

			}
		}

		private void deleteTransferTask(DeleteRequest request)
		{
			try
			{
				request.Process();
			}
			catch (Exception e)
			{

			}
		}

		private void gossipTask()
		{
			foreach (Node node in networkNodes)
			{
				try
				{
					GossipRequest gossipRequest = new GossipRequest(node, networkNodes);
				}
				catch (Exception e) //If exception -> Node didn't respond
				{
					
				}
			}
		}

	}
}

