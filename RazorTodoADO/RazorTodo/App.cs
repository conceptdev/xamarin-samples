using System;

namespace RazorTodo
{
	public static class App
	{
		static TodoItemDatabase database;
		public static void SetDatabaseConnection (TodoItemDatabase db)
		{
			database = db;
		}
		public static TodoItemDatabase Database {
			get { return database; }
		}
	}
}

