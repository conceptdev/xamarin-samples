using System;
using SQLiteBase;

namespace AzurePortable
{
	/// <summary>
	/// Represents a Task.
	/// </summary>
	public class TodoItem : IBusinessEntity
	{
		public TodoItem ()
		{
		}

		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }
	}
}

