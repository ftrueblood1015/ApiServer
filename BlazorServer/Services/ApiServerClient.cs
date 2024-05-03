namespace BlazorServer.Services
{
    public class ApiServerClient
    {
        private readonly HttpClient _httpClient;

        public ApiServerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient Client => _httpClient;
    }
}
