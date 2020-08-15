namespace Sample.Web.Client.Services
{
    using System.Linq;
    using Models;

    public class InMemoryWebApiRepository : IWebApiRepository
    {
        static readonly WebApiOptions[] WebApiServices;

        static InMemoryWebApiRepository()
        {
            WebApiServices = new[]
            {
                new WebApiOptions()
                {
                    // Instance = "https://login.microsoftonline.com/",
                    ClientId = "87482a8b-2100-484f-a1e6-3d962787c113",
                    TenantId = "82d75a56-f939-4164-b05a-2a3c5328b458",
                    // CallbackPath = "/signin-oidc",
                    // SignedOutCallbackPath = "/signout-oidc",
                    BaseAddress = "http://host.docker.internal:5001"
                },
                new WebApiOptions()
                {
                    // Instance = "https://login.microsoftonline.com/",
                    ClientId = "90db28e3-5af6-49a5-8826-a479e3029be7",
                    TenantId = "100d1e66-3613-4505-91d6-b6c20c6370f9",
                    // CallbackPath = "/signin-oidc",
                    // SignedOutCallbackPath = "/signout-oidc",
                    BaseAddress = "http://localhost:5002"
                }
            };
        }
        
        public WebApiOptions GetBy(
            string tenantId
        )
        {
            return WebApiServices.SingleOrDefault(o => o.TenantId == tenantId);
        }
    }
}