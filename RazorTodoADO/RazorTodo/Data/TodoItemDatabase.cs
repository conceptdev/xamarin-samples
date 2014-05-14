using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorTodo
{
	public class TodoItemDatabase 
	{
		IADODatabase database;

		public TodoItemDatabase(IADODatabase database) 
		{
			this.database = database;
		}

		public TodoItem GetItem(int id)
		{
			return database.GetItem(id);
		}

		public List<TodoItem> GetItems ()
		{
			return new List<TodoItem>(database.GetItems());
		}

		public int SaveItem (TodoItem item)
		{
			return database.SaveItem(item);
		}

		public int DeleteItem(TodoItem item)
		{
			return database.DeleteItem(item.ID);
		}

		public List<TodoItem> GetItemsNotDone() {
			return new List<TodoItem>(database.GetItemsNotDone());
		}
	}
}

