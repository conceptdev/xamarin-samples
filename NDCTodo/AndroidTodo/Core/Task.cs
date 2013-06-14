using System;
using SQLite;

namespace NDCTodo {
	/// <summary>
	/// Represents a Task.
	/// </summary>
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