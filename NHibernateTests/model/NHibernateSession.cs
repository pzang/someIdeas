/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 4/18/2014
 * Time: 12:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

using NHibernate.Caches.SysCache2;

namespace NHibernateTests.model
{
public class NHibernateSession
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)

                    InitializeSessionFactory();
                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
        	_sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                  .ConnectionString(
                  @"Data Source=.\SQLEXPRESS;Initial Catalog=testHibernate;Integrated Security=True;") // Modify your ConnectionString
        		          .ShowSql()
                )
                .Mappings(m =>
                          m.FluentMappings
                          .AddFromAssemblyOf<Program>())
        		.Cache( c => c.ProviderClass<SysCacheProvider>().UseSecondLevelCache().UseQueryCache())
        		.ExposeConfiguration(cfg => new SchemaExport(cfg)
                                                .Create(false, false))
                .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
