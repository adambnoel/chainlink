using System;
using System.IO;
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
				switch (kRequest[0])
				{
					case "get":
						return new GetRequest(kRequest[1]);
					case "Get":
						return new GetRequest(kRequest[1]);
					case "put":
					return new PutRequest(kRequest[1], ReadFileAsString(kRequest[2]));
					case "Put":
					return new PutRequest(kRequest[1], ReadFileAsString(kRequest[2]));
					case "delete":
						return new DeleteRequest(kRequest[1]);
					case "Delete":
						return new DeleteRequest(kRequest[1]);
					default:
						return new ErrorRequest(requestText);
				}

			}
			catch (Exception e)
			{
				return new ErrorRequest(requestText);
			}
		}

		private static String ReadFileAsString(String FilePath)
		{
			StreamReader r = new StreamReader(File.Open(FilePath, FileMode.Open));
			String fileContents = r.ReadToEnd();
			return fileContents;
		}
	}
}

