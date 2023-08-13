using System.Net.Http ; 
namespace Serverlib{
    public class VirtualSwagger
    {
        public static HttpClient httpClient = new HttpClient();
        public swaggerClient Client = new swaggerClient("http://localhost:5000/", httpClient) ;
    }
}

