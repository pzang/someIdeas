/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 4/18/2014
 * Time: 10:46 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using NHibernateTests.model;

namespace NHibernateTests
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			using (var session = NHibernateSession.OpenSession()){
				using (var tran = session.BeginTransaction()){
					var parent = new Parent();
					parent.Name= "p1";
					
					var child = new Child();
					child.Name = "c1";
					child.Weight = 2.134;
					child.Parent = parent;
					
					//parent.Children.Add(child);
					
					session.SaveOrUpdate(parent);
					//session.SaveOrUpdate(parent);
					tran.Commit();
				}
			}
						
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}