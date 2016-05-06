using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace TaskyPCL
{
	public class TodoRepository 
	{
		TodoDatabase db = null;
		protected static string dbLocation;		

        public TodoRepository(SQLiteConnection conn, string dbLocation)
		{
			// instantiate the database	
			db = new TodoDatabase(conn, dbLocation);
		}


		public TodoItem GetTodo(int id)
		{
            return db.GetItem<TodoItem>(id);
		}
		
		public IEnumerable<TodoItem> GetTodos ()
		{
			return db.GetItems<TodoItem>();
		}
		
		public int SaveTodo (TodoItem item)
		{
			return db.SaveItem<TodoItem>(item);
		}

		public int DeleteTodo(int id)
		{
			return db.DeleteItem<TodoItem>(id);
		}
	}
}

