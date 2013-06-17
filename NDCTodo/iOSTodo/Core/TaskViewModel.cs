using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace NDCTodo
{
	/// <summary>
	/// Okay, this would probably be better named a repository, but for a larger app
	/// it makes sense to to bind to a 'ViewModel' class from the UI and then access
	/// the repository from within the ViewModel... so here I've just merged them together
	/// </summary>
	/// <remarks>
	/// This code includes locks around the database access which was not discussed in the
	/// NDC talks, but which is good practice for any sort of concurrent access.
	/// </remarks>
	public class TaskViewModel
	{
		static object locker = new object ();

		SQLiteConnection conn;

		/// <summary>
		/// Ensure the instance has a valid 'database connection'
		/// </summary>
		public TaskViewModel () 
		{
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			conn = new SQLiteConnection (System.IO.Path.Combine (folder, "tasks.db"));
			conn.CreateTable<Task> ();
		}

		public IList<Task> GetAll() {
			lock (locker) {
				return (from i in conn.Table<Task> () orderby i.Id select i).ToList ();
			}
		}

		public Task Get(int id) {
			lock (locker) {
				return conn.Table<Task>().FirstOrDefault(x => x.Id == id);
			}
		}

		public int Save(Task t) {
			lock (locker) {
				if (t.Id != 0) {
					conn.Update (t);
					return t.Id;
				} else {
					return conn.Insert (t);
				}
			}
		}

		public int Delete(Task t) {
			lock (locker) {
				return conn.Delete<Task> (t.Id); //primary key, not object itself
			}
		}
	}
}
