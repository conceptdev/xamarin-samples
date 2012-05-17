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

        public static Employee Get(int id) {
            // HACK: having a problem with SQLite and the generic GetItem<Employee>() query
            var tempList = GetItems<Employee>();
            return (from e in tempList
                    where e.Id == id
                    select e).FirstOrDefault();

        }
	}
}