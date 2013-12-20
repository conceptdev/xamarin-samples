using System;

namespace NDCPortable
{
	/// <summary>
	/// Represents a Task.
	/// </summary>
	public class TodoItem 
	{
		public TodoItem ()
		{
		}

        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		// new property
		public bool Done { get; set; }
	}
}

