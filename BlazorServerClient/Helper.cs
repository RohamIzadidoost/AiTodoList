public class helper{
    public static List<Serverlib.Job> JobList = new List<Serverlib.Job>()  ;
    public static SynchronizedCollection<Serverlib.Job> SC = new SynchronizedCollection<Serverlib.Job>("http://localhost:5000/" , JobList) ; 
}