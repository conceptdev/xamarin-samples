using System;
using Newtonsoft.Json;

namespace NDCPortable {

	public class TodoItem {
		public TodoItem () 
		{
			ID = "";
		}

		public string ID {get;set;}
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Task: Title={0}, Description={1}, IsDone={2}]", Name, Notes, Done);
		}
	}
}