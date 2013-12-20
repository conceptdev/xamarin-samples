using System;
using System.Collections.Generic;

namespace NDCPortable
{
	public class TodoItemManager
	{
        IADODatabase database;

        public TodoItemManager(IADODatabase database) 
        {
            this.database = database;
        }

		public TodoItem GetTask(int id)
		{
            return database.GetItem(id);
		}
		
		public List<TodoItem> GetTasks ()
		{
            return new List<TodoItem>(database.GetItems());
		}
		
		public int SaveTask (TodoItem item)
		{
            return database.SaveItem(item);
		}
		
		public int DeleteTask(TodoItem item)
		{
            return database.DeleteItem(item.ID);
		}
		
	}
}