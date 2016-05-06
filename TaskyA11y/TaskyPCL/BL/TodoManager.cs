using System;
using System.Collections.Generic;
using SQLite;

namespace TaskyPCL
{
	public class TodoManager
	{
        TodoRepository repository;

		public TodoManager (SQLiteConnection conn) 
        {
            repository = new TodoRepository(conn, "");
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