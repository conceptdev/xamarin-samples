//using System;
//using NDCPortable;
//using System.Xml.Serialization;
//using System.Collections.Generic;
//using System.IO;
//
//namespace AndroidTodo
//{
//	public class XmlStorageImplementation : IXmlStorage
//	{
//		public XmlStorageImplementation ()
//		{
//		}
//
//		public List<TodoItem> ReadXml (string filename)
//		{
//			if (File.Exists (filename)) {
//				var serializer = new XmlSerializer (typeof(List<TodoItem>));
//				using (var stream = new FileStream (filename, FileMode.Open)) {
//					return (List<TodoItem>)serializer.Deserialize (stream);
//				}
//			}
//			return new List<TodoItem> ();
//		}
//
//		public void WriteXml (List<TodoItem> tasks, string filename)
//		{
//			var serializer = new XmlSerializer (typeof(List<TodoItem>));
//			using (var writer = new StreamWriter (filename)) {
//				serializer.Serialize (writer, tasks);
//			}
//		}
//	}
//}
//
