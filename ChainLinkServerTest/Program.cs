using DHTSharp;
using System;
using System.Text;


namespace ChainLinkServerTest
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			SHA1HashFunction function = new SHA1HashFunction();
			String fileHash = function.GetHash("ENGI9869Proposal.pdf");
			byte[] hashBytes = Encoding.ASCII.GetBytes(fileHash);

			Console.WriteLine("Done testing");
		}
	}
}
