namespace BlazorServer.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
    }
}