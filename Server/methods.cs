using Microsoft.AspNetCore.SignalR; 
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR() ; 
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
using JobContext DbJobs = new JobContext();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

Methods methods = new Methods(DbJobs) ; 
app.MapPost("/SuggestSourceForTask" , (int id) => helper.methods.SuggestSourceForTask(id)) ; 
app.MapPost("/HelpForTask" , (int id) => helper.methods.HelpForTask(id)) ; 
app.MapPost("/Bored" , (int Hours) => helper.methods.Bored(Hours)) ;
app.MapPost("/MoiveAndOrder" , (string problem) => helper.methods.MoiveAndOrder(problem)) ; 
app.MapPost("/Query" , (string s) => helper.methods.GptQuery(s)) ;
app.MapGet("/Hello", () => "Hello World!");
app.MapPost("/Create" , (Job j) => helper.methods.Create(j)) ; 
app.MapPost("/SetProfile", (Profile p) => helper.methods.SetProfile(p)) ; 
app.MapDelete("/Delete" , (int id) => helper.methods.Delete(id)) ; 
app.MapPut("/Update" , (int id , bool isdone) => helper.methods.Update(id , isdone)) ; 
app.MapPut("/UpdateWork" , (int id , string work) => helper.methods.UpdateWork(id , work)) ; 
app.MapPut("/UpdateName" , (int id , string name) => helper.methods.UpdateName(id , name)) ; 
app.MapGet("/GetAll" , () => helper.methods.GetAll()) ; 
app.MapHub<SR<Job>>("/SR") ; 
app.MapHub<SR<int>>("/SRInt") ; 
app.MapHub<SR<(int , Job)>>("/SRSC") ; 
app.MapHub<SRSC>("/SRHub") ; 
app.Run() ; 

public class SRSC : Hub
{
    public async Task UpdateWithIndex(int index , Job t){
        Console.WriteLine("UPDATING WITH INDEX") ; 
        await Clients.Others.SendAsync("UpdateWithIndexSR" , (index , t )) ; 
    }
}
public class SR<T> : Hub
{   
    public async Task CreateSR(T j){
        Console.WriteLine(">>>>>>>>>>>>>CREATING in signal R") ;
        Console.WriteLine( (j as Job).Id) ;  
        helper.methods.Create(j as Job) ; 
        Console.WriteLine("dd\n") ; 
        await Clients.Others.SendAsync("CreateSignal", j);
    }
    public async Task DeleteSR(T id){
        Console.WriteLine(">>>>>>>>>>>>>>>DELete\n");
        Console.WriteLine(id) ;  
        int intid = (id is Int32) ? int.Parse(id as string ?? "0") :  0 ; // chon int nullable nist
        helper.methods.Delete(intid) ; 
        await Clients.All.SendAsync("DeleteSignal", id);
    }
    public async Task DeleteJobSR(T job){
        helper.methods.Delete((job as Job).Id) ; 
        await Clients.Others.SendAsync("DeleteSignalSR" , job) ; 
    }
    public async Task DeleteWithIndex(T index){
        await Clients.Others.SendAsync("DeleteWithIndexSR" , index) ; 
    }
    // public async Task UpdateSR(T j)
    // {
    //     helper.methods.Upd(j as Job).Wait() ;
    //     await Clients.All.SendAsync("UpdateSignal", j);
    // }
    public async Task UpdateSR(int index , T j)
    {
        helper.methods.Upd(j as Job).Wait() ;
        //await Clients.All.SendAsync("UpdateSignal", j);
        await Clients.Others.SendAsync("UpdateSignal" , index , j ) ; 
    }
    public async Task UpdateSRwpf(T j)
    {
        helper.methods.Upd(j as Job).Wait() ;
        await Clients.All.SendAsync("UpdateSignalwpf", j);
    }
    
    public async Task Error(){
        await Clients.Others.SendAsync("ErrorSignal") ; 
    }

    public async Task Success(){
        await Clients.Others.SendAsync("SuccessSignal") ; 
    }
}
