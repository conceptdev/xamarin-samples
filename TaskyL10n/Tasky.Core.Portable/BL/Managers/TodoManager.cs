using System;
using System.Collections.Generic;
using Tasky.BL;
using Tasky.DL.SQLiteBase;

namespace Tasky.BL.Managers
{
	public class TodoManager
	{
        DAL.TodoRepository repository;

		public TodoManager (SQLiteConnection conn) 
        {
            repository = new DAL.TodoRepository(conn, "");
        }

		public TodoItem GetTodo(int id)
		{
            return repository.GetTodo(id);
		}
		
		public IList<TodoItem> GetTodos ()
		{
            return new List<TodoItem>(repository.GetTodos());
		}
		
		public int SaveTodo (TodoItem item)
		{
            return repository.SaveTodo(item);
		}
		
		public int DeleteTodo (int id)
		{
            return repository.DeleteTodo(id);
		}
		
	}
}