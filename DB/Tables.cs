using Microsoft.EntityFrameworkCore;

public class Job
{
    public Job(int Id , string Name , string Work , bool Done){
        this.Id = Id ;
        this.Name = Name ; 
        this.Work = Work ; 
        this.Done = Done ;  
    }
    public int Id {get;set;}
    public string Name {get ; set ;}
    public string Work {get ; set ; }
    public bool Done {get; set;}
}

public class JobContext : DbContext
{
    public DbSet<Job> Jobs {get ; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Tables.db");
    }
}