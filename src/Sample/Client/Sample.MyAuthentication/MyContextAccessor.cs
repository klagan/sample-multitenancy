namespace Sample.MyAuthentication
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

        public string PreferredName => _httpContextAccessor?.HttpContext?.User?.GetDisplayName();

        public string Name => _httpContextAccessor?.HttpContext?.GetName();

        public Tenant Tenant => _httpContextAccessor?.HttpContext?.GetMyTenant();
    }
}