// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddSwaggerGen();
// builder.Services.AddEndpointsApiExplorer();
// Gpt gpt = new Gpt();

//     var app = builder.Build();
//     app.UseSwagger();
//     app.UseSwaggerUI();
// List<string> GptQuery(string query){
//     List<string> res = new List<string>() ;
//     res.Add(gpt.UseChatGPT(query).Result) ;
//     return res ;  
// }
// app.MapPost("/Query" , (string s) => GptQuery(s)) ;
// app.Run(); 