using System;
using Newtonsoft.Json;

namespace NDCPortable {
	/// <summary>
	/// Task business object, stored as XML
	/// </summary>
	public class TodoItem {
		public TodoItem ()
		{
		}

		//public int ID { get; set; }
		public string ID { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "notes")]
		public string Notes { get; set; }

		[JsonProperty(PropertyName = "complete")]
		public bool Done { get; set; }
	}
}