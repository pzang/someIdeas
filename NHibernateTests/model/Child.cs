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
	/// Description of Child.
	/// </summary>
	public class Child
	{
		
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual double Weight { get; set; }
		public virtual Parent Parent {
			get{
				return parent;
			}
			set{
				if(Equals(parent, value))
					return ;
				if(parent != null)
					parent.Children.Remove(this);
				parent = value;
				if(!parent.Children.Contains(this))
					parent.Children.Add(this);

			}
		}
		protected Parent parent;
		
		public override string ToString()
		{
			return string.Format("[Child Parent={0}, Id={1}, Name={2}, Weight={3}]", parent.Name, Id, Name, Weight);
		}

		
	}
	
	public class ChildMap : ClassMap<Child>
	{
		public ChildMap(){
			Id(x => x.Id);
			Map(x => x.Name);
			Map(x => x.Weight);
			//References( x => x.Parent).Cascade.All();
			References( x => x.Parent).Cascade.All().Access.CamelCaseField();
			Table("Child");
		}
	}
}
