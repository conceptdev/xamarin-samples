using System;

namespace TaskyA11y {
	/// <summary>
	/// Represents a Todo item.
	/// </summary>
	public class TodoItem {
		public TodoItem ()
		{
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }
	}
}

