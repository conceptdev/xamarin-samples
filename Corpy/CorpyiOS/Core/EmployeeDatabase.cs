using System;
using SQLiteClient;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Corpy {
	public class EmployeeDatabase : SQLiteCrudDatabase {
		
		protected static string DatabaseFileName {
			get { return "Corpy.db3"; }
		}

			static EmployeeDatabase () 
			{
				me = new EmployeeDatabase(DatabaseFilePath);	
			}

		protected EmployeeDatabase (string path) : base (path)
		{
			CreateTable<Employee>();
		}

		public static bool HasData {
			get {return CountTable<Employee>() > 0;}
		}
	}
}