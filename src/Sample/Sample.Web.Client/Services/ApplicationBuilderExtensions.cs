namespace Sample.Web.Client.Services
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)  
            => builder.UseMiddleware<MyMiddleware>();
    }
}