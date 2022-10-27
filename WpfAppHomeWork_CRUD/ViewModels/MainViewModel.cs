using MVVMTools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp_MVVM.Models;

namespace WpfApp_MVVM.ViewModels
{

    public class MainViewModel : ViewModelBase
    {
        private string taskName;
        private string taskDescription;
        private bool taskDone;
        private MyTask myTask;
        private DateTime taskDeadline = DateTime.Now;
        private ObservableCollection<MyTask> myTasks;
        public MainViewModel()
        {
            MyTasks = new ObservableCollection<MyTask>();
            
        }
        public string TaskName { get => taskName;
            set
            {
                OnChanged(out taskName, value);
                AddTaskCommand.RaiseCanExecuteChanged();
            }
            }
        public string TaskDescription { get => taskDescription;
            set
            {
                OnChanged(out taskDescription, value);
                AddTaskCommand.RaiseCanExecuteChanged();
            }
            }
        public bool TaskDone { get => taskDone; set => OnChanged(out taskDone, value); }
        public DateTime TaskDeadline { get => taskDeadline; set => OnChanged(out taskDeadline, value); }
        public ObservableCollection<MyTask> MyTasks { get => myTasks; set => OnChanged(out myTasks, value); }
        public MyTask SelectedTask { get => myTask; set => OnChanged(out myTask, value); }
        
        public void Clear()
        {
            TaskDescription = string.Empty;
            TaskName = string.Empty;
            TaskDone = false;
            TaskDeadline = DateTime.Now;
        }

        public Command addTaskCommand;
        public Command AddTaskCommand => addTaskCommand ??= new Command(
           () =>
        { 
        MyTask task = new MyTask()
        {
            Description = TaskDescription,
            Name = TaskName,
            Deadline = TaskDeadline,
            IsDone = TaskDone,
        };
        MyTasks.Add(task);
            Clear();
        }, 
            () => 
        {
            return (!string.IsNullOrWhiteSpace(TaskName) && !string.IsNullOrWhiteSpace(TaskDescription));
        });

        public Command<MyTask> removeTaskCommand;
        public Command<MyTask> RemoveTaskCommand => removeTaskCommand ??= new Command<MyTask>(myTask =>
        {
            MyTasks.Remove(myTask);
        });
        public Command editTaskCommand;
        public Command EditTaskCommand => editTaskCommand ??= new Command(
            () =>
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TaskName)) SelectedTask.Name = TaskName;
                if (!string.IsNullOrWhiteSpace(TaskDescription)) SelectedTask.Description = TaskDescription;
                SelectedTask.Deadline = TaskDeadline;
                SelectedTask.IsDone = TaskDone;
                var task = MyTasks.Where(x => x.Name == SelectedTask.Name).FirstOrDefault();
                MyTasks.Add(task);
                Clear();
                MyTasks.Remove(myTask);
            }
            catch (Exception) { }
            });
    }
}
