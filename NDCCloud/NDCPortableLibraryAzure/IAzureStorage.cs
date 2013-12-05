using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDCPortable
{
	public interface IAzureStorage
	{
		Task<List<TodoItem>> RefreshDataAsync();

		Task<TodoItem> GetTodoItemAsync (int id);

		Task SaveTodoItemAsync (TodoItem item);

		Task DeleteTodoItemAsync (TodoItem id);
	}
}

