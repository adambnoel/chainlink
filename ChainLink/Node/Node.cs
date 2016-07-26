using System;
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

		public void AddRing(Ring r)
		{
			nodeDHTRings.Add(r);
		}

		public IPAddress GetIPAddress()
		{
			return nodeAddress;
		}

		public int GetNodeSocket()
		{
			return nodeSocket;
		}

		public Boolean CheckNodeRingsForKey(int hashKey)
		{
			foreach (Ring r in nodeDHTRings)
			{
				if (r.CheckRingForKey(hashKey))
				{
					return true;
				}
			}
			return false;
		}

		public List<Ring> SplitNodeRings()
		{
			List<Ring> newRings = new List<Ring>();
			List<String> ringRangePairs = new List<String>();

			for (int i = 0; i < nodeDHTRings.Count; i++)
			{

				//ringRangePairs.Add(
			}

			return newRings;
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

		private Tuple<int, int> splitHashcodeSpace(int HashcodeSpaceStart, int HashcodeSpaceEnd, Boolean TakeLowerHalf)
		{
			if (TakeLowerHalf)
			{
				return new Tuple<int, int>(HashcodeSpaceStart, (HashcodeSpaceStart + HashcodeSpaceEnd) / 2);
			}
			else 
			{
				return new Tuple<int, int>((HashcodeSpaceStart + HashcodeSpaceEnd) / 2, HashcodeSpaceEnd);
			}
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
			nodeData.Tables.Add(convertNodeToDataTable(inputNode));

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

		private static DataTable convertNodeToDataTable(Node inputNode)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("RingData");
			dt.Columns.Add("NodeAddress");
			dt.Columns.Add("NodeSocket");
			DataRow row = dt.NewRow();
			String serializedRings = String.Empty;
			foreach (Ring r in inputNode.nodeDHTRings)
			{
				serializedRings = serializedRings + Ring.Serialize(r) + ";";
			}
			row["RingData"] = serializedRings;
			row["NodeAddress"] = inputNode.nodeAddress.ToString();
			row["NodeSocket"] = inputNode.nodeSocket.ToString();
			dt.Rows.Add(row);
			return dt;
		}

		private static Node convertDataTableToNode(DataTable dt)
		{
			List<Ring> nodeRings = new List<Ring>();
			String serializedRings = dt.Rows[0]["RingData"].ToString();
			String[] splitSerializedRings = serializedRings.Split(';');
			for (int i = 0; i < splitSerializedRings.Length; i++)
			{
				Ring newRing = Ring.Deserialize(splitSerializedRings[i]);
				nodeRings.Add(newRing);
			}
			String IPAddressString = dt.Rows[0]["NodeAddress"].ToString();
			String SocketString = dt.Rows[0]["NodeSocket"].ToString();
			return new Node(nodeRings, IPAddress.Parse(IPAddressString), int.Parse(SocketString));
		}
	}
}

