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
            // TODO: protect against duplicate tenant id
            // relies on the fact that all webapi configurations are unique by tenant id
            return _webApiOptions.SingleOrDefault(a => a.TenantId == tenantId);
        }
    }
}