namespace Sample.Web.Client.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class HttpContentExtensions
    {
        public static async Task<T> ConvertAsync<T>(this HttpContent content)
         where T : new()
        {
            try
            {
                string json = await content.ReadAsStringAsync();
                T value = JsonConvert.DeserializeObject<T>(json);
                return value;
            }
            catch
            {
                return new T();
            }
        }
    }
}