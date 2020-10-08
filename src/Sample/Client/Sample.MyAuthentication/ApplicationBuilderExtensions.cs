namespace Sample.MyAuthentication
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)  
            => builder.UseMiddleware<MyMiddleware>();
    }
}