namespace Sample.Web.Client.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class WebApiLocator
    {
        private IEnumerable<WebApiOptions> _webApiOptions;

        public WebApiLocator(
            IEnumerable<WebApiOptions> webApiOptions
        )
        {
            _webApiOptions = webApiOptions;
        }

        public WebApiOptions Get(
            string tenantId
        )
        {
            // TODO:: test line to make us always get a known webapi
            // allows us to test against unauthorised access temporarily
            tenantId = "82d75a56-f939-4164-b05a-2a3c5328b458";
            // TODO: protect against duplicate tenant id
            // relies on the fact that all webapi configurations are unique by tenant id
            return _webApiOptions.SingleOrDefault(a => a.TenantId == tenantId);
        }
    }
}