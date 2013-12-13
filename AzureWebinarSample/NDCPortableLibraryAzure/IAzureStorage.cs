using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzurePortable
{
	public interface IAzureStorage
	{
		Task<List<TodoItem>> GetTodoItemsAsync();

		Task<TodoItem> GetTodoItemAsync (string id);

		Task SaveTodoItemAsync (TodoItem item);

		Task DeleteTodoItemAsync (TodoItem id);
	}
}

