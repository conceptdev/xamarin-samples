using System;
using System.Collections.Generic;
using System.Linq;
using DrinkUp.Core.DataLayer.SQLite;
using DrinkUp.Core.BusinessLayer;
using System.IO;

namespace DrinkUp.Core.DataLayer {
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the MWC DB.
	/// It contains methods for retreival and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class DrinkUpDatabase : SQLiteConnection {
		protected static DrinkUpDatabase me = null;
		protected static string dbLocation;

        static object locker = new object ();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="DrinkUp.Core.DataLayer.DrinkUpDatabase"/> MwcDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		protected DrinkUpDatabase (string path) : base (path)
		{
			// create the tables
			CreateTable<Event> ();


			// these are really for caches
			CreateTable<Tweet> ();
		}

		static DrinkUpDatabase ()
		{
			// set the db location
			dbLocation = DatabaseFilePath;
			
			// instantiate a new db
			me = new DrinkUpDatabase(dbLocation);
		}
		
		public static string DatabaseFilePath {
			get { 
#if SILVERLIGHT
            var path = "DrinkUp.db3";
#else

#if __ANDROID__
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
			// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
			// (they don't want non-user-generated data in Documents)
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "../Library/");
#endif
			var path = Path.Combine (libraryPath, "DrinkUp.db3");
#endif		
			return path;	
}
		}

		public static IEnumerable<T> GetItems<T> () where T : DrinkUp.Core.BusinessLayer.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return (from i in me.Table<T> () select i).ToList ();
            }
		}
		
		public static T GetItem<T> (int id) where T : DrinkUp.Core.BusinessLayer.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                
                // ---
                //return (from i in me.Table<T> ()
                //        where i.ID == id
                //        select i).FirstOrDefault ();

                // +++ To properly use Generic version and eliminate NotSupportedException
                // ("Cannot compile: " + expr.NodeType.ToString ()); in SQLite.cs
                return me.Table<T>().FirstOrDefault(x => x.ID == id);
            }
		}
		
		public static int SaveItem<T> (T item) where T : DrinkUp.Core.BusinessLayer.Contracts.IBusinessEntity
		{
            lock (locker) {
                if (item.ID != 0) {
                    me.Update (item);
                    return item.ID;
                } else {
                    return me.Insert (item);
                }
            }
		}
		
		public static void SaveItems<T> (IEnumerable<T> items) where T : DrinkUp.Core.BusinessLayer.Contracts.IBusinessEntity
		{
            lock (locker) {
                me.BeginTransaction ();

                foreach (T item in items) {
                    SaveItem<T> (item);
                }

                me.Commit ();
            }
		}
		
		public static int DeleteItem<T>(int id) where T : DrinkUp.Core.BusinessLayer.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return me.Delete<T> (new T () { ID = id });
            }
		}
		
		public static void ClearTable<T>() where T : DrinkUp.Core.BusinessLayer.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                me.Execute (string.Format ("delete from \"{0}\"", typeof (T).Name));
            }
		}
		
		// helper for checking if database has been populated
		public static int CountTable<T>() where T : DrinkUp.Core.BusinessLayer.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
				string sql = string.Format ("select count (*) from \"{0}\"", typeof (T).Name);
				var c = me.CreateCommand (sql, new object[0]);
				return c.ExecuteScalar<int>();
            }
		}
		
//		public static IEnumerable<T> Query<T>(string query)
//		{
//			return _me.Query.
//		}
		
		public static IEnumerable<Event> GetSessionsByStartDate(DateTime dateMin, DateTime dateMax)
		{
            lock (locker) {
                return (from i in me.Table<Event> ()
                        where i.Starts >= dateMin && i.Starts <= dateMax
                        select i).ToList ();
            }
		}


      
        public static Tweet GetTweet(int id)
        {
            lock (locker) {
                //return DL.MwcDatabase.GetItem<Tweet> (id);
                return (from s in me.Table<Tweet> ()
                        where s.ID == id
                        select s).FirstOrDefault ();
            }
        }
 	}
}