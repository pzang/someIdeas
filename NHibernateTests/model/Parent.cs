/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 4/18/2014
 * Time: 10:55 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using FluentNHibernate.Mapping;

namespace NHibernateTests.model
{
	/// <summary>
	/// Description of Parent.
	/// </summary>
	public class Parent
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IList<Child> Children { get; protected set; }
		
		public Parent()
		{
			Children = new List<Child>();
		}
		
		public override string ToString()
		{
			return string.Format("[Parent Id={0}, Name={1}, Children={2}]", Id, Name, Children);
		}

	}
	
	public class ParentMap : ClassMap<Parent> 
	{
		public ParentMap(){
			Id(x => x.Id);
			Map(x => x.Name);
			HasMany(x => x.Children).Cascade.All().Not.LazyLoad();
			Table("Parent");
		}
	}
}
