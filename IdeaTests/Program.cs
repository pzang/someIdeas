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
using HtmlAgilityPack;

namespace IdeaTests
{
	public class Program
	{
		public static void Main(string[] args)
		{
			
			CheckHtml();
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
//		
//				<add key="AppNamePath" value="//div[@class='details']//a[@class='title']" />
//		<add key="PricePath" value="//div[@class='reason-set']//button[@class='price buy']/span" />
//		<add key="BundleIDPath" value="//div[@class='details']//a[@class='title']" />
//		<add key="BundleIDAttr" value="href" />
//		<add key="BundleIDRegex" value="(?:id=([^&amp;]+))" />
//		<add key="AppIDPath" value="//div[@class='details']//a[@class='title']" />
//		<add key="AppIDAttr" value="href" />
//		<add key="AppIDRegex" value="(?:id=([^&amp;]+))" />

		public static void CheckHtml(){
			HtmlDocument doc = new HtmlDocument();
			string path = @"C:\Users\Pat\Documents\SharpDevelop Projects\IdeaTests\googletest.htm";
			doc.DetectEncodingAndLoad(path);
			HtmlNodeCollection containers =  doc.DocumentNode.SelectNodes(@"//*[@class='cluster-container']/*/div[@class='card-list']");
			Console.WriteLine(containers.Count );
			foreach (var node in containers) {
				HtmlNodeCollection appNames = node.SelectNodes(@"//div[@class='details']//a[@class='title']");
				Console.WriteLine("appname: " + appNames.Count);
			}
		}
		
		
	}
}