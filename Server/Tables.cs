using Microsoft.EntityFrameworkCore;

public class Job
{

    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        if((obj as Job).Id == this.Id) return true ; 
        else return false ; 
    }
    
    public override int GetHashCode()
    {
        return this.Id ; 
    }
    public Job(){
        
    }
    public Job(int Id , string Name , string Work , bool Done){
        this.Id = Id ;
        this.Name = Name ; 
        this.Work = Work ; 
        this.Done = Done ;  
        cnt ++ ; 
    }
    public Job(string Name , string Work , bool Done){
        this.Id = cnt ; 
        this.Name = Name ; 
        this.Work = Work ; 
        this.Done = Done ;  
        cnt ++ ; 
    }
    public int Id {get;set;}
    public string Name {get ; set ;}
    public string Work {get ; set ; }
    public bool Done {get; set;}

    static int cnt = 6 ; 
}

public class JobContext : DbContext
{
    public DbSet<Job> Jobs {get ; set;}
    public DbSet<Profile> Profiles {get ; set ; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DBTables.db");
    }
}

public class Profile
{
    public Profile(){

    }
    public int Id {get ; set ; } 
    public string Name {get ; set ;} 
    public string Interests {get ; set;}
    public int Age {get ; set ;} // default 
}

