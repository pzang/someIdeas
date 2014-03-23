/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 3/22/2014
 * Time: 2:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Collections.Generic;

using Microsoft.Win32;

namespace IdeaTests
{
	[Serializable]
	public class ObjectContainer : ISerializable
	{
		
		private static ConcurrentDictionary<string, ConcurrentBag<MyException>> myerrs = new ConcurrentDictionary<string, ConcurrentBag<MyException>>();
		
		public static int Count { get; set; }
		public static ConcurrentDictionary<string, ConcurrentBag<MyException>> MyErrs{
			get{
				return myerrs;
			}
		}
		
		public ObjectContainer(){}
		public ObjectContainer(SerializationInfo info, StreamingContext context){
			Count = (int)info.GetValue("static.Count", typeof(int));
			myerrs = (ConcurrentDictionary<string, ConcurrentBag<MyException>>)info.GetValue("static.myerrs", typeof(ConcurrentDictionary<string, ConcurrentBag<MyException>>));
		}
		
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue("static.Count", Count, typeof(int));
			info.AddValue("static.myerrs", MyErrs, typeof(ConcurrentDictionary<string, ConcurrentBag<MyException>>));
		}
		
		
		public static  void AddMyException(Exception ex){
			myerrs.TryAdd(ex.GetType().FullName, new ConcurrentBag<MyException>());
			ConcurrentBag<MyException> bag ;
			myerrs.TryGetValue(ex.GetType().FullName, out bag);
			bag.Add(new MyException(ex));
			Count++;
		}
		
		
		public static void GetVersionFromRegistry()
		{
			using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
			                                                    RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
			{
				foreach (string versionKeyName in ndpKey.GetSubKeyNames())
				{
					if (versionKeyName.StartsWith("v"))
					{

						RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
						string name = (string)versionKey.GetValue("Version", "");
						string sp = versionKey.GetValue("SP", "").ToString();
						string install = versionKey.GetValue("Install", "").ToString();
						if (install == "") //no install info, ust be later
							Console.WriteLine(versionKeyName + "  " + name);
						else
						{
							if (sp != "" && install == "1")
							{
								Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
							}

						}
						if (name != "")
						{
							continue;
						}
						foreach (string subKeyName in versionKey.GetSubKeyNames())
						{
							RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
							name = (string)subKey.GetValue("Version", "");
							if (name != "")
								sp = subKey.GetValue("SP", "").ToString();
							install = subKey.GetValue("Install", "").ToString();
							if (install == "") //no install info, ust be later
								Console.WriteLine(versionKeyName + "  " + name);
							else
							{
								if (sp != "" && install == "1")
								{
									Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
								}
								else if (install == "1")
								{
									Console.WriteLine("  " + subKeyName + "  " + name);
								}

							}

						}

					}
				}
			}

		}
	}
}
