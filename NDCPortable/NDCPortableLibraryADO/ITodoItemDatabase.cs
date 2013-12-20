using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDCPortable
{
    public interface IADODatabase
    {
        IEnumerable<TodoItem> GetItems();

        TodoItem GetItem(int id);

        int SaveItem(TodoItem item);

        int DeleteItem(int id);
    }
}