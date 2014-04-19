/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 4/18/2014
 * Time: 6:03 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using FluentNHibernate.Mapping;

namespace NHibernateTests.model
{
	public class StandAlone
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		
	}
	
	public class StandAloneMap : ClassMap<StandAlone>
	{
		public StandAloneMap(){
			//base.Cache.NonStrictReadWrite().Region("FiveSecondTimeInterval");
			Cache.NonStrictReadWrite().Region("ExpireAfterCertainTime");
			Id(x => x.Id);
			Map(x => x.Name);
			Table("StandAlone");
		}
	}
}
