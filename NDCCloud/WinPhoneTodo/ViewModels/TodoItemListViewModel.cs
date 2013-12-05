using NDCPortable;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WinPhoneTodo {
    public class TodoItemListViewModel : ViewModelBase {

        public ObservableCollection<TodoItemViewModel> Items { get; private set; }

        public bool IsUpdating { get; set; }
        public Visibility ListVisibility { get; set; }
        public Visibility NoDataVisibility { get; set; }

        public Visibility UpdatingVisibility
        {
            get
            {
                return (IsUpdating || Items == null || Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        Dispatcher dispatcher;

        public void BeginUpdate(Dispatcher dispatcher) {
            this.dispatcher = dispatcher;

            IsUpdating = true;

            ThreadPool.QueueUserWorkItem(delegate {
                var entries = (App.Current as WinPhoneTodo.App).TodoMgr.GetTasks();
                PopulateData(entries);
            });
        }

        void PopulateData(IEnumerable<TodoItem> entries)
        {
            dispatcher.BeginInvoke(delegate {
                //
                // Set all the news items
                //
                Items = new ObservableCollection<TodoItemViewModel>(
                    from e in entries
                    select new TodoItemViewModel(e));

                //
                // Update the properties
                //
                OnPropertyChanged("Items");

                ListVisibility = Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                NoDataVisibility = Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                OnPropertyChanged("ListVisibility");
                OnPropertyChanged("NoDataVisibility");
                OnPropertyChanged("IsUpdating");
                OnPropertyChanged("UpdatingVisibility");
            });
        }



    }
}
