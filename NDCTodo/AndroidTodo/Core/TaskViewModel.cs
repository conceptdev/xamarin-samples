using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace NDCTodo
{
	public class TaskViewModel
	{
		SQLiteConnection conn;

		public TaskViewModel () 
		{
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			conn = new SQLiteConnection (System.IO.Path.Combine (folder, "tasks.db"));
			conn.CreateTable<Task> ();
		}

		public IList<Task> GetAll() {
			return (from i in conn.Table<Task> () orderby i.Id select i).ToList ();
		}

		public Task Get(int id) {
			return conn.Table<Task>().FirstOrDefault(x => x.Id == id);
		}

		public int Save(Task t) {
			if (t.Id != 0) {
				conn.Update (t);
				return t.Id;
			} else {
				return conn.Insert (t);
			}
		}

		public int Delete(Task t) {
			return conn.Delete<Task> (t.Id); //primary key, not object itself
		}
	}
}
