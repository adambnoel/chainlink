using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace DHTSharp
{
	public class Node
	{
		private List<Ring> nodeDHTRings;
		private IPAddress nodeAddress;
		private int nodeSocket;
		private DateTime lastPingTimeUtc;
		private int pingRefreshTimeSeconds = 300;
		private Semaphore lockRings = new Semaphore(1, 1);

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

		public Boolean RecentlyPinged()
		{
			return (lastPingTimeUtc.AddSeconds(pingRefreshTimeSeconds) < DateTime.UtcNow);
		}

		public DateTime GetLastPingTimeUtc()
		{
			return lastPingTimeUtc;
		}

		public void SetLastPingTimeUtc(DateTime newPingTime)
		{
			lastPingTimeUtc = newPingTime;
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
			lockRings.WaitOne();
			try
			{
				List<Tuple<int, int>> ringTuples = new List<Tuple<int, int>>();
				foreach (Ring ring in nodeDHTRings)
				{
					Tuple<int, int> ringTuple = ring.GetSplitRingTuple();
					if (ringTuples.Contains(ringTuple))
					{
						Ring newRing = ring.Split(false);
						newRings.Add(newRing);

						//Removing the ring tuple requires explanation
						//The idea is that if the ring tuple is present we
						//want to split the ring from the bottom
						//otherwise we want to split the ring from the top
						//We want this to alternate when k >= 2
						//So we remove it
						ringTuples.Remove(ringTuple);
					}
					else
					{
						Ring newRing = ring.Split(true);
						newRings.Add(newRing);
						ringTuples.Add(ringTuple);
					}
				}
			}
			finally
			{
				lockRings.Release();
			}
			return newRings;
		}

		public Boolean MergeRings(List<Ring> RingsToMerge)
		{
			List<Boolean> mergeCheckList = new List<Boolean>();
			lockRings.WaitOne();
			try
			{
				if (RingsToMerge.Count != nodeDHTRings.Count)
				{
					return false;
				}
				for (int i = 0; i < RingsToMerge.Count; i++)
				{
					Boolean check = (nodeDHTRings[i].CheckAdjacent(RingsToMerge[i]));
					mergeCheckList.Add(check);
				}
				if (mergeCheckList.Count == 0)
				{
					return false;
				}
				Boolean mergeSuccess = true;
				foreach (Boolean check in mergeCheckList) {
					mergeSuccess = (mergeSuccess && check);
				}
				for (int i = 0; i < RingsToMerge.Count; i++)
				{
					nodeDHTRings[i].Merge(RingsToMerge[i]);
				}
			}
			finally
			{
				lockRings.Release();
			}
			return true;
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
	}
}

