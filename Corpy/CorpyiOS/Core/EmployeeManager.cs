using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace Corpy {
	public class EmployeeManager {
		public EmployeeManager ()
		{
		}
		
		public static bool HasDataAlready {
			get {
				return EmployeeDatabase.CountTable() > 0;
			}
		}
		public static void UpdateFromFile(string xml)
		{
			var employees = LoadFromXmlFile (xml);
			SaveToSQLite (employees);
		}
	
		static List<Employee> LoadFromXmlFile (string xmlFilename)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(List<Employee>));
			System.IO.Stream stream = new System.IO.FileStream (xmlFilename, System.IO.FileMode.Open);
			object o = serializer.Deserialize (stream);
			stream.Close ();
			return (List<Employee>)o;	
		}
		static void SaveToSQLite (List<Employee> employees) {
			EmployeeDatabase.SaveItems(employees);
		}

		public static List<Employee> GetAll()
		{
			return EmployeeDatabase.GetItems ().ToList();
		}
	}
}

