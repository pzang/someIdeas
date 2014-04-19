/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 4/18/2014
 * Time: 3:09 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;

namespace NHibernateTests
{
	public class CommonUtils
	{
		public static string RandomString(int n){
			Random random = new Random(unchecked((int)DateTime.Now.Ticks));//thanks to McAden
			StringBuilder builder = new StringBuilder();
			char ch;
			for (int i = 0; i < n; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}

			return builder.ToString();

		}
	}
}
