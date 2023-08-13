internal class Program
{
     
    private static void Main(string[] args)
    {
        Gpt g = new Gpt();
        while (true)
        {
            Console.Write("Query? ");
            string query = Console.ReadLine();
            var result = g.UseChatGPT(query);
            System.Console.WriteLine(result.Result);
        }
    }
}