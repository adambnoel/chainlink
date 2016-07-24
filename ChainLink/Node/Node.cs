﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DHTSharp
{
	public class Node
	{
		private List<Ring> nodeDHTRings;
		private IPAddress nodeAddress;
		private int nodeSocket;
		private int failedPingCount = 0;
		private int failedPingThreshold = 5;

		public Node (List<Ring> DHTRings, IPAddress NodeAddress, int Socket) 
		{
			nodeDHTRings = DHTRings;
			nodeAddress = NodeAddress;
			nodeSocket = Socket;
		}

		public Boolean checkNodeRingsForKey(String hashKey)
		{
			foreach (Ring r in nodeDHTRings)
			{
				BigInteger distance = r.GetHashkeyDistance(hashKey);
				if (distance == 0) //Distance is 0 -> key on this ring
				{
					return true;
				}
			}
			return false;
		}

		public Boolean FailedNodePulse()
		{
			failedPingCount++;
			if (failedPingCount >= failedPingThreshold)
			{
				return true;
			}
			return false;
		}

		public void ResetFailedPingCount()
		{
			failedPingCount = 0;
		}

		public static String Serialize(Node inputNode)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
			DataSet nodeData = new DataSet();
			Stream dataStream = new MemoryStream();

			serializer.Serialize(dataStream, nodeData);
			String serializedNode = dataStream.ToString();
			dataStream.Close();
			return serializedNode;
		}

		public static Node Deserialize(String nodeString)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
			DataSet nodeData = new DataSet();
			Stream dataStream = new MemoryStream();
			byte[] nodeStringBytes = Encoding.ASCII.GetBytes(nodeString);
			dataStream.Read(nodeStringBytes, 0, nodeStringBytes.Length);

			nodeData = (DataSet)serializer.Deserialize(dataStream);
			List<Ring> nodeRings = new List<Ring>();

			return new Node(nodeRings, IPAddress.Parse(nodeData.Tables["NodeData"].Columns[""].ToString()), int.Parse(nodeData.Tables["NodeData"].Columns[""].ToString()));
		}
	}
}

