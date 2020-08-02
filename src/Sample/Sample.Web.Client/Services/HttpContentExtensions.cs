namespace Sample.Web.Client.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class HttpContentExtensions
    {
        /// <summary>
        /// Convert the http content to a concrete object
        /// </summary>
        /// <param name="content"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> ConvertAsync<T>(this HttpContent content)
         where T : new()
        {
            try
            {
                var json = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return new T();
            }
        }
    }
}