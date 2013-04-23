using System;
using System.Linq;
using System.Collections.Generic;

namespace Tasky.Core {
	/// <summary>
	/// 
	/// </summary>
	public class TaskStore {
		static object locker = new object ();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public TaskDatabase (string path) : base (path)
		{
			// create the tables
			CreateTable<Task> ();
		}
		
		public IEnumerable<T> GetItems<T> () where T : Contracts.IBusinessEntity, new ()
		{
			lock (locker) {

			}
		}
		
		public T GetItem<T> (int id) where T : Contracts.IBusinessEntity, new ()
		{
			lock (locker) {

			}
		}
		
		public int SaveItem<T> (T item) where T : Contracts.IBusinessEntity
		{
			lock (locker) {

			}
		}
		
		public int DeleteItem<T>(int id) where T : Contracts.IBusinessEntity, new ()
		{
			lock (locker) {

			}
		}
	}
}