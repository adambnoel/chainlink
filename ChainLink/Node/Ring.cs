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
	public class Ring
	{
		private int hashRangeStart;
		private int hashRangeEnd;

		public Ring(int HashRangeStart, int HashRangeEnd)
		{
			hashRangeStart = HashRangeStart;
			hashRangeEnd = HashRangeEnd;
		}

		public Boolean CheckRingForKey(int hashKey)
		{
			if (hashKey >= hashRangeStart && hashKey <= hashRangeEnd)
			{
				return true;
			}
			return false;
		}

		public static String Serialize(Ring inputRing)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
			DataSet ringData = new DataSet();
			Stream dataStream = new MemoryStream();

			ringData.Tables.Add(Ring.convertRingToDataTable(inputRing));
			serializer.Serialize(dataStream, ringData);
			String serializedNode = dataStream.ToString();
			dataStream.Close();
			return serializedNode;
		}

		public static Ring Deserialize(String serializedRing)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
			DataSet ringData = new DataSet();
			Stream dataStream = new MemoryStream();
			byte[] serializedRingBytes = Encoding.ASCII.GetBytes(serializedRing);
			dataStream.Read(serializedRingBytes, 0, serializedRingBytes.Length);
			ringData = (DataSet)serializer.Deserialize(dataStream);
			return convertDataTableToRing(ringData.Tables[0]);
		}

		private static DataTable convertRingToDataTable(Ring inputRing)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("HashRangeStart");
			dt.Columns.Add("HashRangeEnd");
			DataRow dr = dt.NewRow();
			dr["HashRangeStart"] = inputRing.hashRangeStart;
			dr["HashRangeEnd"] = inputRing.hashRangeEnd;
			dt.Rows.Add(dr);
			return dt;
		}

		private static Ring convertDataTableToRing(DataTable inputDataTable)
		{
			DataRow dr = inputDataTable.Rows[0];
			int hashRangeStart = int.Parse((dr["HashRangeStart"].ToString()));
			int hashRangeEnd = int.Parse((dr["HashRangeEnd"].ToString()));
			return new Ring(hashRangeStart, hashRangeEnd);
		}

	}
}

