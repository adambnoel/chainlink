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
		private String hashRangeStart;
		private String hashRangeEnd;
		private int hashFunctionRange;

		public Ring(String HashRangeStart, String HashRangeEnd, int HashFunctionRange)
		{
			hashRangeStart = HashRangeStart;
			hashRangeEnd = HashRangeEnd;
			hashFunctionRange = HashFunctionRange;
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


		public static String Serialize(Ring inputRing)
		{
			String serializedRing = String.Empty;

			return String.Empty;
		}

		public static Ring Deserialize(String serializedRing)
		{
			
			return new Ring("", "", 0);
		}

		public BigInteger GetHashkeyDistance(String hashKey)
		{
			BigInteger hashKeyDistance = new BigInteger(0);
			if (checkCurrentRingForKey(hashKey))
			{
				return hashKeyDistance; //Return 0 -> Key is here
			}
			hashKeyDistance = computeCurrentRingDistance(hashKey);
			return hashKeyDistance;
		}
		private Boolean checkCurrentRingForKey(String hashKey)
		{
			BigInteger requestHashKeyValue = new BigInteger((Encoding.ASCII.GetBytes(hashKey)));
			BigInteger minRingHashKeyValue = new BigInteger((Encoding.ASCII.GetBytes(hashRangeStart)));
			BigInteger maxRingHashKeyValue = new BigInteger((Encoding.ASCII.GetBytes(hashRangeEnd)));
			if (requestHashKeyValue >= minRingHashKeyValue && requestHashKeyValue <= maxRingHashKeyValue)
			{
				return true;
			}
			return false;
		}
		private BigInteger computeCurrentRingDistance(String hashKey)
		{
			BigInteger requestHashKeyValue = new BigInteger((Encoding.ASCII.GetBytes(hashKey)));
			BigInteger minRingHashKeyValue = new BigInteger((Encoding.ASCII.GetBytes(hashRangeStart)));
			BigInteger maxRingHashKeyValue = new BigInteger((Encoding.ASCII.GetBytes(hashRangeEnd)));

			//Check distance from 0 direction
			BigInteger minDistance = BigInteger.Abs(requestHashKeyValue - minRingHashKeyValue);
			if (BigInteger.Abs(requestHashKeyValue - maxRingHashKeyValue) < minDistance)
			{
				minDistance = BigInteger.Abs(requestHashKeyValue - maxRingHashKeyValue);
			}

			//Check distance from rotating ring counter clock-wise
			if (BigInteger.Abs(requestHashKeyValue - wrapHashRange(minRingHashKeyValue)) < minDistance)
			{
				minDistance = BigInteger.Abs(requestHashKeyValue - wrapHashRange(minRingHashKeyValue));
			}

			if (BigInteger.Abs(requestHashKeyValue - wrapHashRange(maxRingHashKeyValue)) < minDistance)
			{
				minDistance = BigInteger.Abs(requestHashKeyValue - wrapHashRange(maxRingHashKeyValue));
			}

			return minDistance;
		}

		private BigInteger wrapHashRange(BigInteger hashKey)
		{
			BigInteger hashFunctionMaxValue = 0;
			for (int i = 0; i < hashFunctionRange; i++)
			{
				if (i == 0)
				{
					hashFunctionMaxValue = 2;
				}
				else 
				{
					hashFunctionMaxValue = hashFunctionMaxValue * 2;
				}
			}
			hashFunctionMaxValue = hashFunctionMaxValue - 1;
			return (hashKey + hashFunctionMaxValue);
		}

		private static DataTable convertRingToDataTable(Ring inputRing)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("HashRangeStart");
			dt.Columns.Add("HashRangeEnd");
			dt.Columns.Add("hashFunctionRange");
			DataRow dr = dt.NewRow();
			dr["HashRangeStart"] = inputRing.hashRangeStart;
			dr["HashRangeEnd"] = inputRing.hashRangeEnd;
			dr["hashFunctionRange"] = inputRing.hashFunctionRange;
			dt.Rows.Add(dr);
			return dt;
		}

	}
}

