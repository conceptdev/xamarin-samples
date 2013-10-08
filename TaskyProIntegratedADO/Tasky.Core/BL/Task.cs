using System;
using Tasky.BL.Contracts;
using Tasky.DL.SQLite;

namespace Tasky.BL
{
	/// <summary>
	/// Represents a Task.
	/// </summary>
	public class Task : IBusinessEntity
	{
		public Task ()
		{
		}
		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		// new property
		public bool Done { get; set; }
	}
}

// keep for compatibility of the Task class with the ORM examples (note: HACK)
namespace Tasky.DL.SQLite {
	public class PrimaryKeyAttribute : Attribute {
	}

	public class AutoIncrementAttribute : Attribute {
	}
}
