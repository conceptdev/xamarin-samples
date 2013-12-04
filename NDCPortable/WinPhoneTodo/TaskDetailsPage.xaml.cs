using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using NDCPortable;

namespace WinPhoneTodo {
    public partial class TaskDetailsPage : PhoneApplicationPage {
        public TaskDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new TodoItemViewModel();
                var task = default(TodoItem);

                if (NavigationContext.QueryString.ContainsKey("id")) {
                    var id = int.Parse(NavigationContext.QueryString["id"]);
                    task = (App.Current as WinPhoneTodo.App).TodoMgr.GetTask(id);
                }

                if (task != null) {
                    vm.Update(task);
                }

                DataContext = vm;
            }
        }

        private void HandleSave(object sender, EventArgs e)
        {
            var taskvm = (TodoItemViewModel)DataContext;
            var task = taskvm.GetTodoItem();
            (App.Current as WinPhoneTodo.App).TodoMgr.SaveTask(task);

            NavigationService.GoBack();
        }

        private void HandleDelete(object sender, EventArgs e)
        {
            var taskvm = (TodoItemViewModel)DataContext;
            if (taskvm.ID >= 0)
                (App.Current as WinPhoneTodo.App).TodoMgr.DeleteTask(taskvm.GetTodoItem());

            NavigationService.GoBack();
        }
    }
}