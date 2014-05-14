using System;
using System.Collections.Generic;

namespace RazorTodo
{
	public interface IADODatabase
	{
		IEnumerable<TodoItem> GetItems();

		TodoItem GetItem(int id);

		int SaveItem(TodoItem item);

		int DeleteItem(int id);

		IEnumerable<TodoItem> GetItemsNotDone();
	}
}

