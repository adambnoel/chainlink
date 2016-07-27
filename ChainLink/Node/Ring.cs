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

		public Boolean CheckAdjacent(Ring inputRing)
		{
			int inputHashRangeStart = inputRing.GetHashRangeStart();
			int inputHashRangeEnd = inputRing.GetHashRangeEnd();
			//Rings may be above or below each other
			if (inputHashRangeStart < hashRangeStart) //Ring portion is below
			{
				return ((hashRangeStart - inputHashRangeEnd) == 1);
			}
			else { //Ring is above
				return (inputHashRangeStart - hashRangeEnd == 1);
			}
		}

		public void Merge(Ring inputRing)
		{
			hashRangeStart = Math.Min(hashRangeStart, inputRing.hashRangeEnd);
			hashRangeEnd = Math.Max(hashRangeEnd, inputRing.hashRangeEnd);
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
			JoinRequest = JoinRequest.Substring(0, JoinRequest.LastIndexOf("\r\n", StringComparison.Ordinal));
			String[] splitJoinRequest = JoinRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			for (int i = 1; i < splitJoinRequest.Length; i++)
			{
				String[] ringDetails = splitJoinRequest[i].Split(',');
				int ringHashrangeStart = int.Parse(ringDetails[0]);
				int ringHashrangEnd = int.Parse(ringDetails[1]);
				Ring newRing = new Ring(ringHashrangeStart, ringHashrangEnd);
				assignedRings.Add(newRing);
			}
			return assignedRings;
		}

		public static List<Ring> ParseLeaveRequest(string LeaveRequest, out Node newNode)
		{
			List<Ring> parsedRings = new List<Ring>();
			LeaveRequest = LeaveRequest.Substring(0, LeaveRequest.LastIndexOf("\r\n", StringComparison.Ordinal));
			String[] splitLeaveRequest = LeaveRequest.Split(new String[] { "\r\n" }, StringSplitOptions.None);

			if (splitLeaveRequest.Length < 3) //Cannot be valid
			{
				newNode = null;
				return null;
			}
			IPAddress nodeAddress;
			if (!IPAddress.TryParse(splitLeaveRequest[1], out nodeAddress))
			{
				newNode = null;
				return null;
			}
			int nodeSocket;
			if (!int.TryParse(splitLeaveRequest[2], out nodeSocket)) {
				newNode = null;
				return null;
			}
			try
			{
				for (int i = 3; i < splitLeaveRequest.Length; i++)
				{
					String[] splitRingRanges = splitLeaveRequest[i].Split(',');
					Ring r = new Ring(int.Parse(splitRingRanges[0]), int.Parse(splitRingRanges[1]));
					parsedRings.Add(r);
				}
			}
			catch (Exception e)
			{
				newNode = null;
				return null;
			}

			newNode = new Node(parsedRings, nodeAddress, nodeSocket);
			return parsedRings;
		}
	}
}

