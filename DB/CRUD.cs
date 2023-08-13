var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
using JobContext DbJobs = new JobContext();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

void Create(Job job)
{
    DbJobs.Jobs.Add(job) ; 
    DbJobs.SaveChanges() ; 
}
void Delete(int id){
    var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ; 
    if(job != null){
        DbJobs.Jobs.Remove(job[0]) ; 
        DbJobs.SaveChanges() ; 
    }
}
void Update(int id , bool isdone){
    var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
    if(job != null){
        job[0].Done = isdone ; 
        DbJobs.SaveChanges();
    }
}
void UpdateWork(int id , string work){
    var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
    if(job != null){
        job[0].Work = work ; 
        DbJobs.SaveChanges();
    }
}
void UpdateName(int id , string name){
    var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
    if(job != null){
        job[0].Name = name; 
        DbJobs.SaveChanges();
    }
}
Job[] GetAll(){
    return DbJobs.Jobs.ToArray() ; 
}
app.MapGet("/Hello", () => "Hello World!");
app.MapPost("/Create" , (Job j) => Create(j)) ; 
app.MapDelete("/Delete" , (int id) => Delete(id)) ; 
app.MapPut("/Update" , (int id , bool isdone) => Update(id , isdone)) ; 
app.MapPut("/UpdateWork" , (int id , string work) => UpdateWork(id , work)) ; 
app.MapPut("/UpdateName" , (int id , string name) => UpdateName(id , name)) ; 
app.MapGet("/GetAll" , () => GetAll()) ; 
app.Run() ; 