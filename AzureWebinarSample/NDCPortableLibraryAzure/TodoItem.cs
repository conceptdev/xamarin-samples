using System;
using Newtonsoft.Json;

namespace AzurePortable {

	public class TodoItem {
		public TodoItem ()
		{
		}

		public string ID { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "notes")]
		public string Notes { get; set; }

		[JsonProperty(PropertyName = "complete")]
		public bool Done { get; set; }
	}
}