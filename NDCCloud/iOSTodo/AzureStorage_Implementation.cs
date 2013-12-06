using System;
using NDCPortable;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using MonoTouch.UIKit;

namespace iOSTodo
{
	public class AzureStorageImplementation : DelegatingHandler, IAzureStorage
	{
		static AzureStorageImplementation todoServiceInstance = new AzureStorageImplementation();
		public static AzureStorageImplementation DefaultService { get { return todoServiceInstance; } }
		public List<TodoItem> Items { get; private set;}
		public MobileServiceClient Client;
		IMobileServiceTable<TodoItem> todoTable;

		public MobileServiceUser User;

		// Constructor
		protected AzureStorageImplementation()
		{
			CurrentPlatform.Init ();

			Items = new List<TodoItem>();

			Client = new MobileServiceClient ("https://xamarin-todo.azure-mobile.net/", "", this);	
			//Client = new MobileServiceClient ("https://xamarin-todo.azure-mobile.net/", "555", this);	

			todoTable = Client.GetTable<TodoItem>(); // Create an MSTable instance to allow us to work with the TodoItem table
		}

		#region Auth
		/// <summary>
		/// NOT PART OF INTERFACE INTO PCL
		/// </summary>
		public async Task<bool> Authenticate (UIViewController viewController) {
			try
			{
				User = await Client.LoginAsync(viewController, MobileServiceAuthenticationProvider.Google);
				if (User != null) return true;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine (@"ERROR - AUTHENTICATION FAILED {0}", ex.Message);
			}
			return false;
		}
		#endregion 

		async public Task<List<TodoItem>> RefreshDataAsync()
		{
			try 
			{
				// This code refreshes the entries in the list view by querying the TodoItems table.
				Items = await todoTable.ToListAsync();
				//.Where (todoItem => todoItem.Complete == false).ToListAsync();
			} 
			catch (MobileServiceInvalidOperationException msioe) 
			{
				Console.Error.WriteLine (@"ERROR {0}", msioe.Message);
				return null;
			}

			return Items;
		}

		public async Task SaveTodoItemAsync(TodoItem todoItem)
		{
			if (String.IsNullOrEmpty(todoItem.ID))
				await todoTable.InsertAsync(todoItem);
			else
				await todoTable.UpdateAsync(todoItem);
		}

		public async Task<TodoItem> GetTodoItemAsync(string id)
		{
			try 
			{
				return await todoTable.LookupAsync(id);
			} 
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.Error.WriteLine(@"INVALID {0}", msioe.Message);
			}
			catch (Exception e) 
			{
				Console.Error.WriteLine(@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task DeleteTodoItemAsync(TodoItem item)
		{
			try 
			{
				await todoTable.DeleteAsync(item);
			} 
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.Error.WriteLine(@"INVALID {0}", msioe.Message);
			}
			catch (Exception e) 
			{
				Console.Error.WriteLine(@"ERROR {0}", e.Message);
			}
		}


		private int busyCount = 0;

		// Public events
		public event Action<bool> BusyUpdate;
		void Busy(bool busy)
		{
			// assumes always executes on UI thread
			if (busy) 
			{
				if (busyCount++ == 0 && BusyUpdate != null)
					BusyUpdate(true);
			} 
			else 
			{
				if (--busyCount == 0 && BusyUpdate != null)
					BusyUpdate(false);
			}
		}

		// TODO:: Uncomment this code when using Mobile Services and inheriting from IServiceFilter
		#region implemented abstract members of HttpMessageHandler

		protected override async Task<System.Net.Http.HttpResponseMessage> SendAsync (System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			Busy (true);
			var response = await base.SendAsync (request, cancellationToken);

			Busy (false);
			return response;
		}

		#endregion

		public List<TodoItem> ReadXml (string filename)
		{
			if (File.Exists (filename)) {
				var serializer = new XmlSerializer (typeof(List<TodoItem>));
				using (var stream = new FileStream (filename, FileMode.Open)) {
					return (List<TodoItem>)serializer.Deserialize (stream);
				}
			}
			return new List<TodoItem> ();
		}

		public void WriteXml (List<TodoItem> tasks, string filename)
		{
			var serializer = new XmlSerializer (typeof(List<TodoItem>));
			using (var writer = new StreamWriter (filename)) {
				serializer.Serialize (writer, tasks);
			}
		}
	}
}

