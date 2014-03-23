/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 3/22/2014
 * Time: 10:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;

namespace SerializerTests
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			IdeaTests.ObjectContainer.AddMyException(new Exception("i add an exception here"));
			IdeaTests.ObjectContainer.AddMyException(new EntryPointNotFoundException("add an entrypointNotFoundException"));
			IdeaTests.Program.Serialize();
			Thread.Sleep(1000);
			IdeaTests.Program.Deserialize();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}