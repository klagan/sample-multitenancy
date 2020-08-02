namespace Sample.Web.Client.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Identity.Web;

    public class MyContextAccessor : IMyContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyContextAccessor (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string MyKey => _httpContextAccessor.HttpContext.GetMyKey();

        public string TenantId => _httpContextAccessor.HttpContext.User.GetTenantId();
    }
}