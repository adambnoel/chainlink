using System;
namespace DHTSharp
{
	public class RequestParser
	{
		//TODO: Add error-handling for integer parsing
		public static IRequest ParseRequest(String request)
		{
			String[] splitRequest = request.Split(new String[] { "\r\n" }, StringSplitOptions.None);
			if (splitRequest.Length <= 0)
			{
				return new ErrorRequest(request);
			}
			switch (splitRequest[0])
			{
				case "+":
					if (splitRequest.Length >= 4)
					{
						return new KPutRequest(splitRequest[1], int.Parse(splitRequest[2]), splitRequest[3]);
					}
					else {
						return new PutRequest(splitRequest[1], splitRequest[2]);
					}
				case "-":
					if (splitRequest.Length >= 3)
					{
						return new KDeleteRequest(splitRequest[1], int.Parse(splitRequest[2]));
					}
					else {
						return new DeleteRequest(splitRequest[1]);
					}
				case "*":
					if (splitRequest.Length >= 3)
					{
						return new KGetRequest(splitRequest[1], int.Parse(splitRequest[2]));
					}
					else 
					{
						return new GetRequest(splitRequest[1]);
					}
				default:
					return new ErrorRequest(request);
			}
		}
	}
}

