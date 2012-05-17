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
				return EmployeeDatabase.HasData;
			}
		}
        public static void UpdateFromString(string xml) 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
            System.IO.TextReader reader = new System.IO.StringReader(xml);
            object o = serializer.Deserialize(reader);
            reader.Close();
            var employees = (List<Employee>)o;
            SaveToSQLite(employees);
        }
		public static void UpdateFromFile(string xmlFilename)
		{
            var employees = LoadFromXmlFile(xmlFilename);
			SaveToSQLite (employees);
		}
		static List<Employee> LoadFromXmlFile (string xmlFilename)
		{
			var xmlString = System.IO.File.ReadAllText(xmlFilename);
			XmlSerializer serializer = new XmlSerializer (typeof(List<Employee>));
			System.IO.TextReader reader = new System.IO.StringReader(xmlString);
			object o = serializer.Deserialize(reader);
			reader.Close();
			return (List<Employee>)o;
		}
		static void SaveToSQLite (List<Employee> employees) {
			EmployeeDatabase.SaveItems<Employee>(employees);
		}
		public static List<Employee> GetAll()
		{
			return EmployeeDatabase.GetItems<Employee> ().ToList();
		}

        public static Employee Get(int id)
        {
            return EmployeeDatabase.Get(id);
        }
	}
}

