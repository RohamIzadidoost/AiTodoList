using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel; 
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;
using Swashbuckle.AspNetCore.Swagger;
using Serverlib; 
namespace Wpf
{
    
    public partial class MainWindow : Window, INotifyPropertyChanged
    {         
        private ObservableCollection<Task> taskList;
        private Task selectedTask;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Task> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskList)));
            }
        }

        public Task SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTask)));
            }
        }
        public static HttpClient httpClient = new HttpClient();

        public Serverlib.swaggerClient Client = new Serverlib.swaggerClient("http://localhost:5000/",httpClient);

        public HubConnection connection = new HubConnectionBuilder()
                        .WithUrl("http://localhost:5000/SR")
                        .Build();
        public HubConnection ConnInt = new HubConnectionBuilder()
                        .WithUrl("http://localhost:5000/SRInt")
                        .Build();  
        public MainWindow()
        {
            InitializeComponent();
            connection.StartAsync().Wait() ; 
            ConnInt.StartAsync().Wait() ; 
            var Jobs = Client.GetAllAsync().Result;
            helper.JobList = new List<Serverlib.Job>() ; 
            foreach(var j in Jobs)
            {
                helper.JobList.Add(j) ;    
            }
            // JobList = new List<Serverlib.Job>() ; 
            helper.SC = new SynchronizedCollection<Serverlib.Job>("http://localhost:5000/" , helper.JobList);

            connection.On("CreateSignal" , (Serverlib.Job j) => {
                Application.Current.Dispatcher.Invoke(() => 
                {
                    TaskList.Add(new Task() {TaskID = j.Id , TaskTitle = j.Name , TaskDescription = j.Work  , done = j.Done }) ; 
                });
            });

            ConnInt.On("DeleteSignal" , (int id) => {
               Application.Current.Dispatcher.Invoke(() => 
                {
                    foreach(var v in TaskList){
                        if(v.TaskID == id){
                            TaskList.Remove(v) ; 
                            break;
                        }
                    } 
                }); ; 
            });

            connection.On("UpdateSignalwpf" , (Serverlib.Job j) => {
               Application.Current.Dispatcher.Invoke(() => 
                {
                    TaskList.Clear() ;
                    var jobs = Client.GetAllAsync().Result ;
                    TaskList = new ObservableCollection<Task>() ; 
                    foreach (var j in jobs)
                    {
                        TaskList.Add(new Task() {TaskID = j.Id , TaskTitle = j.Name , TaskDescription = j.Work  , done = j.Done }) ;  
                    }
                }); ; 
            });

            DataContext = this;
            var jobs = Client.GetAllAsync().Result ;
            TaskList = new ObservableCollection<Task>() ; 
            foreach (var j in jobs)
            {
                TaskList.Add(new Task() {TaskID = j.Id , TaskTitle = j.Name , TaskDescription = j.Work  , done = j.Done }) ;  
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                UpdateTaskWindow updateTaskWindow = new UpdateTaskWindow(SelectedTask.TaskID);
                updateTaskWindow.ShowDialog();
            }
        }

        public void UpdateTask(Task newTask){
            var jobs = Client.GetAllAsync().Result ;
            foreach(var j in jobs){
                if(newTask.TaskID == j.Id){
                    Serverlib.Job job = new Serverlib.Job() ; 
                    job.Done = newTask.done ; 
                    job.Work = newTask.TaskDescription ; 
                    job.Name = newTask.TaskTitle; 
                    job.Id = j.Id ; 
                    connection.SendAsync("UpdateSRwpf" , job) ; 
                    break ; 
                }
            }
        }
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();
        }
        public void AddTask(Task newTask)
        {
            Serverlib.Job job = new Serverlib.Job{Id = newTask.TaskID , Name = newTask.TaskTitle , Work = newTask.TaskDescription , Done = false} ; 
            Client.CreateAsync(job).Wait() ;
            connection.SendAsync("CreateSR" , job) ;
            TaskList.Add(newTask);
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("DELETING!\n") ; 
            if (SelectedTask != null)
            {
                int id = SelectedTask.TaskID ; 
                Client.DeleteAsync(id).Wait() ; 
                ConnInt.SendAsync("DeleteSR" , id) ;
                Console.WriteLine(SelectedTask.TaskID) ; 
                TaskList.Remove(SelectedTask);
                SelectedTask = null;
            }
        }

        public void HelpForTask_Click(object sender , RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                int id = SelectedTask.TaskID ; 
                var gptres = Client.HelpForTaskAsync(id).Result ; 
                List<string> ResStrings = new List<string>() ; 
                foreach(string s in gptres){
                    ResStrings.Add(s) ; 
                }
                HelpForTaskWindow helpForTaskWindow = new HelpForTaskWindow(ResStrings) ; 
                helpForTaskWindow.ShowDialog() ; 
            }
        }
    }

    public class Task : INotifyPropertyChanged
    {
        private int taskID;
        private string taskTitle;
        private string taskDescription;

        public bool done ; 

        public event PropertyChangedEventHandler PropertyChanged;

        public int TaskID
        {
            get { return taskID; }
            set
            {
                taskID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskID)));
            }
        }

        public string TaskTitle
        {
            get { return taskTitle; }
            set
            {
                taskTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskTitle)));
            }
        }

        public string TaskDescription
        {
            get { return taskDescription; }
            set
            {
                taskDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskDescription)));
            }
        }

        public string TaskDone
        {
            get
            {
                if(this.done) return "Done" ; 
                else return "Undone" ;
            }
        }
    }
}

