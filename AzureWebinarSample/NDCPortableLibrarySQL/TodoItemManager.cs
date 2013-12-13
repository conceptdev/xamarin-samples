using System;
using System.Collections.Generic;
using AzurePortable.SQLiteBase;

namespace AzurePortable
{
	public class TodoItemManager
	{
        TodoItemRepository repository;

		public TodoItemManager (SQLiteConnection conn) 
        {
            repository = new TodoItemRepository(conn, "");
        }

		public TodoItem GetTask(int id)
		{
            return repository.GetTask(id);
		}
		
		public List<TodoItem> GetTasks ()
		{
            return new List<TodoItem>(repository.GetTasks());
		}
		
		public int SaveTask (TodoItem item)
		{
            return repository.SaveTask(item);
		}
		
		public int DeleteTask(TodoItem item)
		{
			return repository.DeleteTask(item.ID);
		}
		
	}
}