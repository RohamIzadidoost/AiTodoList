using System.Windows;

namespace Wpf
{
    public partial class UpdateTaskWindow : Window
    {
        public int Id ; 
        public UpdateTaskWindow(int id)
        {
            InitializeComponent();
            this.Id = id ; 
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Task newTask = new Task()
            {
                TaskTitle = txtName.Text , 
                TaskDescription = txtWork.Text , 
                TaskID = this.Id , 
                //done = bool.Parse(IsDonebtn.IsChecked)
            };
            bool b = IsDonebtn.IsChecked ?? false ; 
            if(b){
                newTask.done = true ; 
            }else{
                newTask.done = false ; 
            }

            ((MainWindow)Application.Current.MainWindow).UpdateTask(newTask);

            Close();
        }
    }
}