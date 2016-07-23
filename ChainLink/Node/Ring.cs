using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DHTSharp
{
	public class Ring
	{
		private String hashRangeStart;
		private String hashRangeEnd;
		private int hashFunctionRange;
		private IHashTableManager hashTableManager;
		public Ring(String HashRangeStart, String HashRangeEnd, int HashFunctionRange, IHashTableManager HashTableManager)
		{
			hashRangeStart = HashRangeStart;
			hashRangeEnd = HashRangeEnd;
			hashFunctionRange = HashFunctionRange;
			hashTableManager = HashTableManager;
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
	}
}

