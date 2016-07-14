﻿using System;
namespace DHTSharp
{
	public class DeleteRequest : IRequest
	{
		private String key;
		public DeleteRequest(String requestKey)
		{
			key = requestKey;
		}

		public String ProcessRequest()
		{
			return String.Empty;
		}
	}
}

