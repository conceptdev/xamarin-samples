using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SQLiteClient {

	public interface IBusinessEntity {
		int Id { get; set; }
	}

	public class SQLiteCrudDatabase : SQLiteConnection {

		static object locker = new object ();
		protected static SQLiteCrudDatabase me = null;

		protected SQLiteCrudDatabase (string path) : base (path)
		{
		}

		protected static string DatabaseFileName {
			get { return "Database.db3"; }
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
				var path = Path.Combine (libraryPath, DatabaseFileName);	
				return path;	
			}
		}

		
		public static IEnumerable<T> GetItems<T> () where T : IBusinessEntity, new ()
		{
            lock (locker) {
                return (from i in me.Table<T> () select i).ToList ();
            }
		}
		
		public static T GetItem<T> (int id) where T : IBusinessEntity, new ()
		{
            lock (locker) {
                return (from i in me.Table<T> ()
                        where i.Id == id
                        select i).FirstOrDefault ();
            }
		}
		
		public static int SaveItem<T> (T item) where T : IBusinessEntity
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
		
		public static void SaveItems<T> (IEnumerable<T> items) where T : IBusinessEntity
		{
            lock (locker) {
                me.BeginTransaction ();

                foreach (T item in items) {
                    SaveItem<T> (item);
                }

                me.Commit ();
            }
		}
		
		public static int DeleteItem<T>(int id) where T : IBusinessEntity, new ()
		{
            lock (locker) {
                return me.Delete<T> (new T () { Id = id });
            }
		}
		
		public static void ClearTable<T>() where T : IBusinessEntity, new ()
		{
            lock (locker) {
                me.Execute (string.Format ("delete from \"{0}\"", typeof (T).Name));
            }
		}
		
		// helper for checking if database has been populated
		public static int CountTable<T>() where T : IBusinessEntity, new ()
		{
            lock (locker) {
				string sql = string.Format ("select count (*) from \"{0}\"", typeof (T).Name);
				var c = me.CreateCommand (sql, new object[0]);
				return c.ExecuteScalar<int>();
            }
		}
	}
}