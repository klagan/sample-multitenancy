namespace Sample.MyAuthentication
{
    using System;

    public class Tenant
    {
        public Tenant(
            string id,
            string name,
            string dbConnectionString,
            string baseAddress
        )
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
            
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                throw new ArgumentNullException(nameof(dbConnectionString));
            }

            DbConnectionString = dbConnectionString;

            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            BaseAddress = baseAddress;
        }
        
        public string Id { get; private set; }
        
        public string Name { get; private set; }
        
        public string DbConnectionString { get; private set; }
        
        public string BaseAddress { get; private set; }
    }
}