namespace Sample.MyAuthentication
{
    using System.Collections.Generic;

    public interface ITenantDataSource
    {
        IEnumerable<string> GetValidTenants();
    }
}