namespace Sample.Web.Client.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    internal class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // always added on every call to server
            // as its named, very useful for context information so need not pass context information through params but context
            context.Items.Add("MyKey", "kaml");
            
            if (_next != null)
                await _next(context);
            
            // do some stuff on the way out (cant write to response tho)
            context.Items.Add("MyExitKey", "im out!");
        }
    }
}