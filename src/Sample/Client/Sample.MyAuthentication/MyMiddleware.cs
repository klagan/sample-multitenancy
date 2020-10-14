namespace Sample.MyAuthentication
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    internal class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // EXAMPLE: added on every call - context information - need not pass context information through params
            context.Items.Add(MyConstants.MyKey, "kaml");

            if (context.User.Identity.IsAuthenticated)
            {
                var tenantService = context.RequestServices.GetService<ITenantDataSource>() ?? new EmptyTenantDataSource();
                var tenant = tenantService.List().SingleOrDefault(x => x.Id == context.TenantId());
                    
                context.Items.Add(MyConstants.TenantKey, tenant);
            }

            if (_next != null)
                await _next(context);
            
            // EXAMPLE: do some stuff on the way out (cant write to response tho)
            context.Items.Add(MyConstants.MyExitKey, "im out!");
        }
    }
}