namespace Sample.MyAuthentication
{
    using System.Collections.Generic;
    using System.Linq;

    public class EmptyTenantDataSource : ITenantDataSource
    {
        public IEnumerable<string> GetValidTenants()
        {
            return new string[] { };
        }

        public IQueryable<Tenant> List()
        {
            return new Tenant[]{}.AsQueryable();
        }
    }
}