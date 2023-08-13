using OpenAI_API;
using OpenAI_API.Completions;
public class Gpt{
    
    public async  Task<string> UseChatGPT(string query)
    {
        completionRequest = new CompletionRequest() ; 
        string outputResult = "";
        completionRequest.Prompt = query;
        completionRequest.Model = OpenAI_API.Models.Model.DavinciText;
        completionRequest.MaxTokens = 1024;

        var completions = await openai.Completions.CreateCompletionAsync(completionRequest);

        foreach (var completion in completions.Completions)
        {
            outputResult += completion.Text;
        }

        return outputResult;
    }
    public CompletionRequest completionRequest = new CompletionRequest();
    public OpenAIAPI openai = new OpenAIAPI(Secrets.OpenAIKey);
}

public class Methods
{
    public JobContext DbJobs = new JobContext();
    public Methods(){
    }
    public Methods(JobContext _Dbjobs){
        this.DbJobs = _Dbjobs ; 
    }
    public void SetProfile(Profile pf){
        Console.WriteLine("ADDING PROFILE") ; 
        if(DbJobs.Profiles.Count() == 0){
            Console.WriteLine("ADDING PROFILE1") ; 
            Console.WriteLine(DbJobs.Profiles.Count()) ; 
            DbJobs.Profiles.Add(pf) ; 
            Console.WriteLine("ADDING PROFILE2") ; 
            DbJobs.SaveChanges() ;
        }else{
            Profile p = DbJobs.Profiles.FirstOrDefault() ; 
            p = pf ; 
            DbJobs.SaveChanges() ; 
        }
    }
    public string GetInteresets(){ // since there is only one user for now , implemented like this
        var pf = DbJobs.Profiles.FirstOrDefault() ; 
        return pf.Interests ; 
    }
    public void Create(Job job)
    {
        Console.WriteLine("CREATING") ; 
        DbJobs.Jobs.Add(job) ; 
         DbJobs.SaveChangesAsync().Wait(); 
    }
    public void Delete(int id){
        Console.WriteLine("DELETING ") ; 
        var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ; 
        if(job != null){
            DbJobs.Jobs.Remove(job[0]) ; 
             DbJobs.SaveChangesAsync().Wait(); 
        }
    }
    public async Task Upd(Job j){
        Console.WriteLine("EDITING ") ; 
        var job = DbJobs.Jobs.Where(x => x.Id == j.Id).ToArray() ;
        if(j.Done != job[0].Done){
            Update(j.Id , j.Done) ; 
        }
        if(j.Name != job[0].Name)
        {
            UpdateName(j.Id , j.Name) ; 
        }
        if(j.Work != job[0].Work){
            UpdateWork(j.Id , j.Work) ; 
        }
    }
    public void Update(int id , bool isdone){
        var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
        if(job != null){
            job[0].Done = isdone ; 
            DbJobs.SaveChangesAsync().Wait();
        }
    }
    public void UpdateWork(int id , string work){
        var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
        if(job != null){
            job[0].Work = work ; 
            DbJobs.SaveChangesAsync().Wait();
        }
    }
    public void UpdateName(int id , string name){
        var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
        if(job != null){
            job[0].Name = name; 
            DbJobs.SaveChangesAsync().Wait();
        }
    }
    public Job[] GetAll(){
        Console.WriteLine("GETTING ALL") ; 
        return DbJobs.Jobs.ToArray() ; 
    }
    public List<string> HelpForTask(int id){
        var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
        if(job != null){
            string query = $"help me to ${job[0].Name} , i want to ${job[0].Work}" ;
            return GptQuery(query) ;  
        }else{
            List<string> res = new List<string>() ;
            res.Add("There is no Task with this id") ; 
            return res ; 
        }
    }
    public List<string> SuggestSourceForTask(int id){
        var job = DbJobs.Jobs.Where(x => x.Id == id).ToArray() ;
        var p = DbJobs.Profiles.ToArray() ;
        if(job != null && p != null){
            string query = $"i have this task to do : ${job[0].Name} , ${job[0].Work} , give youtube sources for help as link and i'm interested in {p[0].Interests} , suggest me some music to do my task" ;
            return GptQuery(query) ;  
        }else{
            List<string> res = new List<string>() ;
            res.Add("There is no Task with this id Or there is no profile") ; 
            return res ; 
        }
    }
    public List<string> Bored(int Hours){
        var p = DbJobs.Profiles.ToArray() ; 
        if(p != null){
            string query = $"I'm bored and interested in{p[0].Interests} , suggest me a task to do in {Hours} hours" ; 
            return GptQuery(query) ; 
        }else{
            List<string> res = new List<string>() ;
            res.Add("There is no Profile") ; 
            return res ; 
        }
    }
    public List<string> MoiveAndOrder(string problem){
        var jobs = DbJobs.Jobs.ToArray() ;
        var p = DbJobs.Profiles.ToArray() ;
        if(p != null && jobs != null){
            string query = "I want to do this tasks: " ; 
            foreach(var j in jobs){
                query = query + j.Name ; 
            }
            query = query + $" but i have this problem: {problem} , and I'm interested in {p[0].Interests} , Motive me to do them and give an order or a way to start" ; 
            return GptQuery(query) ;
        }else{
            List<string> res = new List<string>() ;
            res.Add("There is no Profile Or there is no Task") ; 
            return res ;
        }
    }
    public Gpt gpt = new Gpt();
    public List<string> GptQuery(string query){
        List<string> res = new List<string>() ;
        try
        {
            res.Add(gpt.UseChatGPT(query).Result) ;
        }
        catch
        {
            res.Add("GPT unavailabe due to network problem") ; 
        }
        return res ;  
    }
}
public class helper{
    public static JobContext DbJobs = new JobContext();
    public static Methods methods = new Methods(DbJobs) ;
}