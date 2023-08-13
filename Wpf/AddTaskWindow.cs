using System.Windows;

namespace Wpf
{
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Task newTask = new Task()
            {
                TaskTitle = txtName.Text , 
                TaskDescription = txtWork.Text , 
                TaskID = int.Parse(txtID.Text) 
            };

            ((MainWindow)Application.Current.MainWindow).AddTask(newTask);

            Close();
        }
    }
}