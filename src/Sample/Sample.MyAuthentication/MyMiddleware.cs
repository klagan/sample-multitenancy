namespace Sample.MyAuthentication
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
            // added on every call - context information - need not pass context information through params
            context.Items.Add(MyConstants.MyKey, "kaml");

            if (context.User.Identity.IsAuthenticated)
            {
                // TODO:: load tenant object and store in context 
            }

            if (_next != null)
                await _next(context);
            
            // do some stuff on the way out (cant write to response tho)
            context.Items.Add(MyConstants.MyExitKey, "im out!");
        }
    }
}