using System;
using System.Collections.Generic;

namespace NDCPortable
{
	public interface IXmlStorage
	{
		List<TodoItem> ReadXml (string filename);

		void WriteXml (List<TodoItem> tasks, string filename);

	}
}

