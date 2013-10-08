using System;
using System.Linq;
using Tasky.BL;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.IO;

namespace Tasky.DL
{
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
	/// </summary>
	public class TaskDatabase 
	{
		static object locker = new object ();

		public SqliteConnection connection;

		public string path;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public TaskDatabase (string dbPath) 
		{
			var output = "";
			path = dbPath;
			// create the tables
			bool exists = File.Exists (dbPath);

			if (!exists) {
				output += "Creating database";
				// Need to create the database and seed it with some data.
				//Mono.Data.Sqlite.SqliteConnection.CreateFile (dbPath);
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, Name NTEXT, Notes NTEXT, Done INTEGER);"
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						var i = c.ExecuteNonQuery ();
						output += "\n\tExecuted " + command + " (rows:" + i + ")";
					}
				}
			} else {
				output += "Database already exists";
			}
			Console.WriteLine (output);
		}
		
		public IEnumerable<Task> GetItems ()
		{
			var tl = new List<Task> ();

            lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "SELECT [_id], [Name], [Notes], [Done] from [Items]";
					var r = contents.ExecuteReader ();
					while (r.Read ()) {
						var t = new Task ();
						t.ID = Convert.ToInt32 (r ["_id"]);
						t.Name = r ["Name"].ToString ();
						t.Notes = r ["Notes"].ToString ();
						t.Done = Convert.ToInt32 (r ["Done"]) == 1 ? true : false;
						tl.Add (t);
					}
				}
				connection.Close ();
            }
			return tl;
		}

		public Task GetItem (int id) 
		{
			var t = new Task ();
            lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "SELECT [_id], [Name], [Notes], [Done] from [Items]";
					var r = contents.ExecuteReader ();
					while (r.Read ()) {
						t.ID = Convert.ToInt32 (r ["_id"]);
						t.Name = r ["Name"].ToString ();
						t.Notes = r ["Notes"].ToString ();
						t.Done = Convert.ToInt32 (r ["Done"]) == 1 ? true : false;
						break;
					}
				}
				connection.Close ();
            }
			return t;
		}

		public int SaveItem (Task item) 
		{
            lock (locker) {
                if (item.ID != 0) {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var contents = connection.CreateCommand ()) {
						contents.CommandText = "UPDATE [Items] SET [Name] = '"+item.Name+"', [Notes] = '"+item.Notes+"', [Done] = "+(item.Done?1:0)+" WHERE [_id] = " + item.ID;

						var r = contents.ExecuteNonQuery ();
					}
					connection.Close ();
                    return 1;
                } else {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var contents = connection.CreateCommand ()) {
						contents.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES ('"+item.Name+"','"+item.Notes+"',"+(item.Done?1:0)+")";
						var r = contents.ExecuteNonQuery ();
					}
					connection.Close ();
                    return -1;
                }

            }
		}
		
		public int DeleteItem(int id) 
		{
            lock (locker) {
#if NETFX_CORE
				return -1; //TODO: 
#else
				int r;
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "DELETE FROM [Items] WHERE [_id] = " + id;
					r = contents.ExecuteNonQuery ();

				}
				connection.Close ();
				return r;
#endif
            }
		}
	}
}