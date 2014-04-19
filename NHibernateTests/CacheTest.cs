/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 4/18/2014
 * Time: 3:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using NUnit.Framework;

using NHibernateTests.model;

namespace NHibernateTests
{
	[TestFixture]
	public class CacheTest
	{
		
		//	[SetUp]
		public void setup(){
			using (var session = NHibernateSession.OpenSession()){
				using (var tran = session.BeginTransaction()){
					for(int i=0;i <100000; i++){
						var parent = new Parent();
						parent.Name = CommonUtils.RandomString(10);
						
						var child = new Child();
						child.Name = CommonUtils.RandomString(5);
						child.Weight = new Random().NextDouble();
						
						child.Parent = parent;
						
						
						var stand = new StandAlone();
						stand.Name = CommonUtils.RandomString(5);
						//parent.Children.Add(child);
						//	session.SaveOrUpdate(parent);
						session.SaveOrUpdate(stand);
					}
					tran.Commit();
				}
				session.Close();
			}
		}

		public long TestFirstLoad()
		{
			Console.WriteLine("********************************* FIRST LOAD STARTED ***********************");
			long start = DateTime.Now.Ticks;
			
			using (var session = NHibernateSession.OpenSession()){
				using (var tran = session.BeginTransaction()){
					var allParent = session.CreateCriteria<Parent>().SetCacheable(true).List<Parent>();
					foreach (var e in allParent) {
						//Console.Write(e.Id);
					}
					tran.Commit();
				}
			}
			long end = DateTime.Now.Ticks;
			Console.WriteLine("********************************* FIRST LOAD ENDED ***********************");
			return end-start;
			
		}
		
		public long TestSQLLoad()
		{
			Console.WriteLine("********************************* SQL LOAD STARTED ***********************");
			long start = DateTime.Now.Ticks;
			using (var session = NHibernateSession.OpenSession()){
				using (var tran = session.BeginTransaction()){
					var all = session.CreateSQLQuery(@"select {l.*} from [TestDB].[dbo].[Parent] as l");
					all.AddEntity("l", typeof(Parent));
					foreach (var p in all.List<Parent>()) {
						//Console.Write(p.ToString());
					}
					tran.Commit();
				}
			}
			long end = DateTime.Now.Ticks;
			Console.WriteLine("********************************* SQL LOAD ENDED ***********************");
			return end-start;

		}
		
		public long TestSQL(){
			string connectionString = @"Data Source=.\SQL2008EXPRESS;Initial Catalog=TestDB;Integrated Security=True;";
			long start = DateTime.Now.Ticks;
			using (SqlConnection conn = new SqlConnection(connectionString)){
				conn.Open();
				SqlCommand command = new SqlCommand("select * from [TestDB].[dbo].[Parent]", conn);
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read()) {
					Console.WriteLine();
					for(int i = 0; i < reader.FieldCount; i++){
						Console.Write(reader[i] + " , ");
					}
				}
			}
			long end = DateTime.Now.Ticks;
			return end-start;
		}
		
		public long IndepHibernate(){
			Console.WriteLine("********************************* Hibernate LOAD STARTED ***********************");
			long start = DateTime.Now.Ticks;
			
			using (var session = NHibernateSession.OpenSession()){
				using (var tran = session.BeginTransaction()){
					var allParent = session.CreateCriteria<StandAlone>()
						.SetCacheable(true).List<StandAlone>();
					foreach (var e in allParent) {
						
						//Console.Write(e.Id);
					}
					tran.Commit();
				}
			}
			long end = DateTime.Now.Ticks;
			Console.WriteLine("********************************* Hibernate LOAD ENDED ***********************");
			return end-start;
			
		}
		
		public long IndepSQL(){
			Console.WriteLine("********************************* SQL LOAD STARTED ***********************");
			string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testHibernate;Integrated Security=True;";
			long start = DateTime.Now.Ticks;
			using (SqlConnection conn = new SqlConnection(connectionString)){
				conn.Open();
				SqlCommand command = new SqlCommand("select * from [testHibernate].[dbo].[StandAlone]", conn);
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read()) {
					//	Console.WriteLine();
					StandAlone sa = new StandAlone();
					sa.Id = (int)reader[0];
					sa.Name = (string)reader[1];
					//Console.Write(reader[i] + " , ");
				}
			}
			long end = DateTime.Now.Ticks;
			Console.WriteLine("********************************* SQL LOAD ENDED ***********************");
			return end-start;
		}
		
		public long RandomQuerySQL (){
			Console.WriteLine("********************************* SQL LOAD STARTED ***********************");
			string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testHibernate;Integrated Security=True;";
			long start = DateTime.Now.Ticks;
			using (SqlConnection conn = new SqlConnection(connectionString)){
				conn.Open();
				for(int i=0; i<1000000; i++){
					SqlCommand command = new SqlCommand("select * from [testHibernate].[dbo].[StandAlone] WHERE Id="
					                                    + new Random().Next(1, 9), conn);
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read()) {
						//	Console.WriteLine();
						StandAlone sa = new StandAlone();
						sa.Id = (int)reader[0];
						sa.Name = (string)reader[1];
						//Console.Write(reader[i] + " , ");
					}
					reader.Close();
				}
			}
			long end = DateTime.Now.Ticks;
			Console.WriteLine("********************************* SQL LOAD ENDED ***********************");
			return end-start;
		}
		
		public long RandomHibernateQuery(){
			Console.WriteLine("********************************* Hibernate Random STARTED ***********************");
			long start = DateTime.Now.Ticks;
			
			using (var session = NHibernateSession.OpenSession()){
//				using (var tran = session.BeginTransaction()){
				for(int i=0; i<1000000; i++){
					var allParent = session.CreateCriteria<StandAlone>()
						.SetCacheable(true).SetCacheMode(NHibernate.CacheMode.Normal)
						.Add(
							NHibernate.Criterion.Restrictions.Eq("Id", new Random().Next(1, 9))
						)
						.List<StandAlone>();

//					}
//					tran.Commit();
				}
			}
			long end = DateTime.Now.Ticks;
			Console.WriteLine("********************************* Hibernate Random ENDED ***********************");
			return end-start;
			
		}

		[Test]
		public void TestTime(){
			long time1 = IndepHibernate();
			Thread.Sleep(1000);
			//long time2 = IndepHQL();
			long time3 = IndepSQL();
			Console.WriteLine(" Hibernate load : " + time1);
			//Console.WriteLine(" HQL Load: " + time2);
			Console.WriteLine(" SQL Load: " + time3);
			
			long cachetime = IndepHibernate();
			Console.WriteLine(" Cached hibernate load: " + cachetime);
			
			long randSQL = RandomQuerySQL();
			long randHibernate = RandomHibernateQuery();
			
			Console.WriteLine(" Random SQL : " + randSQL);
			Console.WriteLine(" Random Hibernate: " + randHibernate);
		}
	}
}
