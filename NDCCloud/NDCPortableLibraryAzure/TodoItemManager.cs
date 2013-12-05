using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDCPortable {
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public class TodoItemManager {
		//TodoItemRepository repository;
		IAzureStorage storage;

		public TodoItemManager (IAzureStorage storage) 
		{
			//repository = new TodoItemRepository(filename, storage);
			this.storage = storage;
		}

		public Task<TodoItem> GetTodoItemAsync(int id)
		{
			return storage.GetTodoItemAsync(id);
		}
		
		public Task<List<TodoItem>> GetTodoItemsAsync ()
		{
			return storage.RefreshDataAsync();
		}
		
		public Task SaveTodoItemAsync (TodoItem item)
		{
			return storage.SaveTodoItemAsync(item);
		}
		
		public Task DeleteTodoItemAsync (TodoItem item)
		{
			return storage.DeleteTodoItemAsync(item);
		}
	}
}