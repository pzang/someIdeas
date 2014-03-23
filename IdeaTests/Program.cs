/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 3/22/2014
 * Time: 2:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace IdeaTests
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Deserialize();
			// TODO: Implement Functionality Here
			//ObjectContainer.GetVersionFromRegistry();
			for(int i=0;i <1000; i++){
			ObjectContainer.AddMyException(new Exception("nest level 1 message", new Exception("new exception added")));
			ObjectContainer.AddMyException(new IOException("my IO exception added"));
			ObjectContainer.AddMyException(new Exception("new exception added"));
			ObjectContainer.AddMyException(new Exception("new exception added"));
			}

			Thread.Sleep(3000);
			
			ICollection<ConcurrentBag<MyException>> col =  ObjectContainer.MyErrs.Values;
			Serialize();
			
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		
		public static void Serialize(){
			FileStream fs = new FileStream("systemState", FileMode.Create);
			BinaryFormatter sp = new BinaryFormatter();
			sp.Serialize(fs, new ObjectContainer());
			fs.Dispose();
		}
		
		public static void Deserialize(){
			if(File.Exists("systemState")){
				FileStream fs = new FileStream("systemState", FileMode.Open);
				BinaryFormatter bf = new BinaryFormatter();
				ObjectContainer oc = new ObjectContainer();
				oc = (ObjectContainer)bf.Deserialize(fs);
				Console.WriteLine("Count : " + ObjectContainer.Count);
				foreach (var element in ObjectContainer.MyErrs) {
					foreach(var bag in element.Value){
						Console.WriteLine("myerrs: "+ element.Key + ", value: "+ bag.Message);
					}
					
				}
				fs.Dispose();
			}
		}
		
	}
}