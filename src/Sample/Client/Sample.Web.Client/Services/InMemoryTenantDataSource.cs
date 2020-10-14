namespace Sample.Web.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MyAuthentication;

    /// <summary>
    /// Representation of a repository for storing any tenant specification information
    /// </summary>
    public class InMemoryTenantDataSource : ITenantDataSource
    {
        public IEnumerable<string> GetValidTenants()
        {
            // here for simplicity we just return a hard-coded list of TenantIds but idea is to change to dynamic list
            return new[]
            {
                "82d75a56-f939-4164-b05a-2a3c5328b458",  // laganlabs.it
                "100d1e66-3613-4505-91d6-b6c20c6370f9",  // test24.uk
            };  
        }

        public IQueryable<Tenant> List()
        {
            return new Tenant[]
                {
                    new Tenant(
                        "82d75a56-f939-4164-b05a-2a3c5328b458"
                        , "LaganLabs.IT"
                        , "LaganLabs db connection string"
                        , "http://localhost:8081/"
                        ),
                    // TODO: this section is not used currently, so dont expect it to work
                    new Tenant(
                        "100d1e66-3613-4505-91d6-b6c20c6370f9"
                        , "Test24.UK"
                        , "Test24 db connection string"
                        , "http://localhost:8089/"
                        )
                }
                .AsQueryable();
        }
    }
}