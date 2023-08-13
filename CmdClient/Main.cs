using CmdClient;
using Microsoft.AspNetCore.SignalR.Client;
using HttpClient httpClient = new HttpClient();
swaggerClient Client = new swaggerClient("http://localhost:5000/", httpClient);
var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/SR")
            .Build();
var ConnInt = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/SRInt")
            .Build();
connection.StartAsync().Wait();
ConnInt.StartAsync().Wait();
connection.On("CreateSignal", (Job j) =>
{
    Console.WriteLine($"task with id: {j.Id} just added\n");
});
connection.On("ErrorSignal", () =>
{
    Console.WriteLine("an Error occured!!!!");
});
connection.On("SuccessSignal", () =>
{
    Console.WriteLine("Succeeded!!!!");
});
ConnInt.On("DeleteSignalSR", (Job j) =>
{
    Console.WriteLine($"task with id: {j.Id} just deleted\n");
});
ConnInt.On("UpdateSignal", (int id) =>
{
    Console.WriteLine($"task with id: {id} just updated\n");
});
string input = null;
Console.WriteLine("Welcome to ToDo\n options: add delete edit SeeTasks AskMe\n  type <<exit>> to finish the program\n");
var Jobs = Client.GetAllAsync().Result;
helper.JobList = new List<Serverlib.Job>();
foreach (var j in Jobs)
{
    Serverlib.Job job = new Serverlib.Job { Id = j.Id, Name = j.Name, Work = j.Work, Done = j.Done };
    helper.JobList.Add(job);
}
helper.SC = new SynchronizedCollection<Serverlib.Job>("http://localhost:5000/", helper.JobList);
while (input != "exit")
{
    input = Console.ReadLine();
    switch (input)
    {
        case "add":
            Console.WriteLine("id"); int id = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Please write name of task:\n");
            string name = Console.ReadLine();
            Console.WriteLine("Any note on task\n");
            string work = Console.ReadLine();
            Serverlib.Job job = new Serverlib.Job { Id = id, Name = name, Work = work, Done = false };
            helper.SC.Add(job);
            Thread.Sleep(1000);
            Console.WriteLine("Task Added\n");
            break;
        case "delete":
            Console.WriteLine("id");
            id = Int32.Parse(Console.ReadLine());
            Serverlib.Job job1 = new Serverlib.Job();
            foreach (var v in helper.SC.Objs())
            {
                if (v.Id == id)
                {
                    job1 = v;
                }
            }
            helper.SC.Remove(job1);
            Thread.Sleep(1000);  //will be Asynced

            Console.WriteLine("Task deleted\n");
            break;
        case "edit":
            Serverlib.Job job2 = new Serverlib.Job();
            Console.WriteLine("id");
            id = Int32.Parse(Console.ReadLine());
            foreach (var v in helper.SC.Objs())
            {
                if (v.Id == id)
                {
                    job2 = v;
                }
            };
            Console.WriteLine("To rewrite the name type 1 , to mark as done or undone type 2 , to change the note type 3 \n");
            string typ = Console.ReadLine();
            switch (typ)
            {
                case "1":
                    Console.WriteLine("write the new name: ");
                    string NewName = Console.ReadLine();
                    job2.Name = NewName;
                    helper.SC[2] = job2;
                    Thread.Sleep(1000);
                    Console.WriteLine("Name Changed\n");
                    break;
                case "2":
                    Console.WriteLine("type done or undone");
                    string state = Console.ReadLine();
                    if (state == "done")
                    {
                        job2.Done = true;
                        helper.SC[2] = job2;
                        Thread.Sleep(1000);
                    }
                    else if (state == "undone")
                    {
                        job2.Done = false;
                        helper.SC[2] = job2;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Request\n");
                    }
                    break;
                case "3":
                    Console.WriteLine("write the new note: ");
                    string NewNote = Console.ReadLine();
                    job2.Work = NewNote;
                    helper.SC[2] = job2;
                    Thread.Sleep(1000);
                    break;
                default:
                    Console.WriteLine("Invalid Request\n");
                    break;
            }
            Console.WriteLine("Task Updated\n");
            break;
        case "SeeTasks":
            var jobs = Client.GetAllAsync().Result;
            foreach(var j in jobs){
                string Result = $"{j.Id} {j.Name} {j.Work} {j.Done}" ;
                Console.WriteLine(Result) ; 
            }
            foreach (var j in helper.SC.Objs())
            {
                string Result = $"{j.Id} {j.Name} {j.Work} {j.Done}";
                Console.WriteLine(Result);
            }
            break;
        case "AskMe":
            Console.WriteLine("If you want help to do a task type 1 :");
            typ = Console.ReadLine();
            switch (typ)
            {
                case "1":
                    Console.WriteLine("Type id of the Task: ");
                    id = int.Parse(Console.ReadLine());
                    var Res = Client.HelpForTaskAsync(id).Result;
                    foreach (string s in Res)
                    {
                        Console.WriteLine(s);
                    }
                    break;
            }
            break;
        default:
            Console.WriteLine("Invalid Request\n");
            break;
    }
    Console.WriteLine("options: add delete edit SeeTasks AskMe\n");
}

