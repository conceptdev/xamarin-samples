using NDCPortable;

namespace WinPhoneTodo {
    public class TodoItemViewModel : ViewModelBase {

        TodoItem internalItem;

        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    
        public TodoItemViewModel ()
        {
            internalItem = new TodoItem();
        }
        public TodoItemViewModel (TodoItem item)
        {
            internalItem = item;
            Update (item);
        }

        public void Update(TodoItem item)
        {
            internalItem = item;

            ID = item.ID;
            Name = item.Name;
            Notes = item.Notes;
            Done = item.Done;
        }

        public TodoItem GetTodoItem()
        {
            internalItem.Name = this.Name;
            internalItem.Notes = this.Notes;
            internalItem.Done = this.Done;

            return internalItem;
        }
    }
}
