using System;
using SQLite;

namespace NDCTodo {
	/// <summary>
	/// A to-do task item
	/// </summary>
	/// <remarks>
	/// Requires the SQLite.NET component to support the [PrimaryKey,AutoIncrement] attributes
	/// </remarks>
	public class Task {
		public Task ()
		{
		}
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }
		public string Title { get; set; }
		public bool Done { get; set; }
	}
}