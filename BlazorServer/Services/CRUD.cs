using System.Collections.Generic;
using BlazorServer.Models;

namespace BlazorServer.Services
{
    public class TaskService
    {
        private List<BlazorServer.Models.Task> tasks = new List<BlazorServer.Models.Task>();

        public List<BlazorServer.Models.Task> GetTasks()
        {
            return tasks;
        }

        public void AddTask(BlazorServer.Models.Task task)
        {
            task.ID = tasks.Count + 1;
            tasks.Add(task);
        }

        public void DeleteTask(int id)
        {
            BlazorServer.Models.Task task = tasks.Find(t => t.ID == id);
            if (task != null)
                tasks.Remove(task);
        }

        public void EditTask(BlazorServer.Models.Task task)
        {
            BlazorServer.Models.Task existingTask = tasks.Find(t => t.ID == task.ID);
            if (existingTask != null)
            {
                existingTask.Name = task.Name;
                existingTask.Description = task.Description;
                existingTask.IsDone = task.IsDone;
            }
        }
    }
}