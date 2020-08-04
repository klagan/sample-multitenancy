namespace Sample.MyAuthentication
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Agent to manage tenants
    /// </summary>
    public interface ITenantDataSource
    {
        /// <summary>
        /// Return a list of valid tenant guids
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetValidTenants();

        /// <summary>
        /// Find tenant information
        /// </summary>
        /// <returns></returns>
        IQueryable<Tenant> List();
    }
}