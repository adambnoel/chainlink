using System;
using System.Linq;

namespace ChainLinkCLI
{
	public class RequestFactory
	{
		public static IRequest GetRequest(String requestText)
		{
			try
			{
				String[] kRequest = requestText.Split(' ').Skip(1).ToArray();
				int numVotes = 0;
				int.TryParse(kRequest[1], out numVotes);


				switch (kRequest[0])
				{
					case "get":
						if (numVotes > 0)
						{
							return new KGetRequest(kRequest[2], numVotes);
						}
						else 
						{
							return new GetRequest(kRequest[1]);
						}
					case "Get":
						if (numVotes > 0)
						{
							return new KGetRequest(kRequest[2], numVotes);
						}
						else
						{
							return new GetRequest(kRequest[1]);
						}
					case "put":
						if (numVotes > 0)
						{
							return new KPutRequest(kRequest[2], kRequest[3], numVotes);
						}
						else 
						{
							return new PutRequest(kRequest[1], kRequest[2]);
						}
					case "Put":
						if (numVotes > 0)
						{
							return new KPutRequest(kRequest[2], kRequest[3], numVotes);
						}
						else
						{
							return new PutRequest(kRequest[1], kRequest[2]);
						}
					case "delete":
						if (numVotes > 0)
						{
							return new KDeleteRequest(kRequest[2], numVotes);
						}
						else
						{
							return new DeleteRequest(kRequest[1]);
						}
					case "Delete":
						if (numVotes > 0)
						{
							return new KDeleteRequest(kRequest[2], numVotes);
						}
						else
						{
							return new DeleteRequest(kRequest[1]);
						}
					default:
						return new ErrorRequest(requestText);
				}

			}
			catch (Exception e)
			{
				return new ErrorRequest(requestText);
			}
		}
	}
}

