using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDCPortable {
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public class TodoItemManager {

		IAzureStorage storage;

		public TodoItemManager (IAzureStorage storage) 
		{
			this.storage = storage;
		}

		public Task<TodoItem> GetTaskAsync(string id)
		{
			return storage.GetTodoItemAsync(id);
		}
		
		public Task<List<TodoItem>> GetTasksAsync ()
		{
			return storage.RefreshDataAsync();
		}
		
		public Task SaveTaskAsync (TodoItem item)
		{
			return storage.SaveTodoItemAsync(item);
		}
		
		public Task DeleteTaskAsync (TodoItem item)
		{
			return storage.DeleteTodoItemAsync(item);
		}
	}
}