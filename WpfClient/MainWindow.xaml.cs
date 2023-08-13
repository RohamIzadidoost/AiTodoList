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

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Task> tasks = new List<Task>
            {
                new Task { Name = "Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(1) },
                new Task { Name = "Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(2) },
                new Task { Name = "Task 3", Description = "Description 3", DueDate = DateTime.Now.AddDays(3) }
            };

            taskDataGrid.ItemsSource = tasks;
        }
    }
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
