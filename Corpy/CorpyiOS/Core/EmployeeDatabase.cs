using System;
using SQLiteClient;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Corpy {
	public class EmployeeDatabase : SQLiteConnection {
		protected static EmployeeDatabase me = null;
		static object locker = new object ();

		protected EmployeeDatabase (string path) : base (path)
		{
			CreateTable<Employee>();
		}
		
		public static string DatabaseFilePath {
			get { 
#if __ANDROID__
            	string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
				// we need to put in /Library/ 
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library");
#endif
				var path = Path.Combine (libraryPath, "CorpyData.db3");	
				return path;	
			}
		}

		static EmployeeDatabase () 
		{
			// instantiate a new db
			me = new EmployeeDatabase(DatabaseFilePath);
		}
		public static int CountTable() 
		{
            lock (locker) {
				string sql = string.Format ("select count (*) from \"{0}\"", typeof (Employee).Name);
				var c = me.CreateCommand (sql, new object[0]);
				return c.ExecuteScalar<int>();
            }
		}
		public static IEnumerable<Employee> GetItems () 
		{
            lock (locker) {
                return (from i in me.Table<Employee> () select i).ToList ();
            }
		}
		public static int SaveItem (Employee item) 
		{
            lock (locker) {
                if (item.Id != 0) {
                    me.Update (item);
                    return item.Id;
                } else {
                    return me.Insert (item);
                }
            }
		}
		public static void SaveItems (IEnumerable<Employee> items) 
		{
            lock (locker) {
                me.BeginTransaction ();

                foreach (Employee item in items) {
                    SaveItem (item);
                }

                me.Commit ();
            }
		}
	}
}

