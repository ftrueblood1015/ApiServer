using BlazorServer.Services;
using Newtonsoft.Json;

namespace BlazorServer
{
    public static class ServiceExtensions
    {
        public static async Task<TResult> PostAsJson<TResult, TModel>(this ApiServerClient client, Uri uri, TModel model)
        {
            return await ExtractResponsePost<TResult>(async () => await client.Client.PostAsJsonAsync(uri, model));
        }

        private static async Task<T> ExtractResponsePost<T>(Func<Task<HttpResponseMessage>> func)
        {
            var response = await func();

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()) ?? Activator.CreateInstance<T>();
        }
    }
}
