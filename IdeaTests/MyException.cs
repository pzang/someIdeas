/*
 * Created by SharpDevelop.
 * User: Pat
 * Date: 3/22/2014
 * Time: 2:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.Serialization;

namespace IdeaTests
{
	/// <summary>
	/// Description of MyException.
	/// </summary>
	[Serializable]
	public class MyException : Exception, ISerializable
	{
		public static int Count { get; set;}

		public MyException()
		{
		}
		
		public MyException(Exception ex) : base(ex.Message, ex.InnerException){
		}
		public MyException(SerializationInfo info, StreamingContext context) : base(info, context){
			
			Count = (int)info.GetValue("static.Count", typeof(int));
		}
		
		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
			info.AddValue("static.Count", Count, typeof(int));
		}
	}
}
