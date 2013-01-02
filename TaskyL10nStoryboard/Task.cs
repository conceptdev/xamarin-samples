using System;

namespace StoryboardTables {
	/// <summary>
	/// Represents a Task.
	/// </summary>
	public class Task {
		public Task ()
		{
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }
	}
}

