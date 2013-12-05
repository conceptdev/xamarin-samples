using System;
using System.Linq;
using System.Collections.Generic;

namespace NDCPortable {
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public class TodoItemManager {
		List<TodoItem> items;

		public TodoItemManager () 
		{
			items = new List<TodoItem> ();
		}

		public TodoItem GetTask(int id)
		{
			return (from i in items
				where i.ID == id
				select i).FirstOrDefault ();
		}
		
		public List<TodoItem> GetTasks ()
		{
			return items;
		}
		int max;
		public int SaveTask (TodoItem item)
		{
			if (item.ID <= 0) {
				item.ID = ++max;
				items.Add (item);
			}
			return 1;
		}
		
		public int DeleteTask(TodoItem item)
		{
			return items.Remove (item)?1:0;
		}
	}
}