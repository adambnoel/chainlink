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

		public int GetHashRangeStart()
		{
			return hashRangeStart;
		}

		public int GetHashRangeEnd()
		{
			return hashRangeEnd;
		}

		public Boolean CheckRingForKey(int hashKey)
		{
			if (hashKey >= hashRangeStart && hashKey <= hashRangeEnd)
			{
				return true;
			}
			return false;
		}

		public Tuple<int, int> GetSplitRingTuple()
		{
			Int64 powerOfTwo = Convert.ToInt64(Math.Log(((double)hashRangeEnd - (double)hashRangeStart) + 1) / Math.Log(2));
			Int64 scalingFactor = (Int64)((Math.Pow(2, powerOfTwo - 1)));

			int estimatedHashRangeEnd = (int)((Int64)hashRangeEnd - scalingFactor); //Int 64 to prevent overflow
			return new Tuple<int, int>(hashRangeStart, estimatedHashRangeEnd);
		}

		public Ring Split(bool SplitFromTop)
		{
			Int64 powerOfTwo = Convert.ToInt64(Math.Log(((double)hashRangeEnd - (double)hashRangeStart) + 1) / Math.Log(2));
			Int64 scalingFactor = (Int64)((Math.Pow(2, powerOfTwo - 1)));

			if (SplitFromTop)
			{
				int newHashRangeEnd = (int)((Int64)hashRangeEnd - scalingFactor); //Int 64 to prevent overflow
				int oldHashRangeEnd = hashRangeEnd;
				hashRangeEnd = newHashRangeEnd;
				return new Ring(newHashRangeEnd+1, oldHashRangeEnd);
			}
			else 
			{
				int newHashRangeStart = (int)((Int64)hashRangeStart + scalingFactor); //Int64 to prevent overflow
				int oldHashRangeStart = hashRangeStart;
				hashRangeStart = newHashRangeStart;
				return new Ring(oldHashRangeStart, newHashRangeStart-1);
			}
		}

		public static List<Ring> ParseJoinRequest(string JoinRequest)
		{
			List<Ring> assignedRings = new List<Ring>();
			String[] splitJoinRequest = JoinRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			for (int i = 1; i < splitJoinRequest.Length; i++)
			{
				String[] ringDetails = splitJoinRequest[i].Split('-');
				int ringHashrangeStart = int.Parse(ringDetails[0]);
				int ringHashrangEnd = int.Parse(ringDetails[1]);
				Ring newRing = new Ring(ringHashrangeStart, ringHashrangEnd);
				assignedRings.Add(newRing);
			}
			return assignedRings;
		}
	}
}

