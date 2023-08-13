using OpenAI_API;
using OpenAI_API.Completions;
// using HttpClient client = new HttpClient();

// var result = CallSearchAPI("HOSSEIN", 2343)
//     .ContinueWith(t => System.Console.WriteLine(t.Result));

// result.Wait();
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