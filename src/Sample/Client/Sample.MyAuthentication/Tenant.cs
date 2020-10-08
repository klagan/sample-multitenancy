namespace Sample.MyAuthentication
{
    using System;

    public class Tenant
    {
        public Tenant(
            string id,
            string name,
            string dbConnectionString
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
        }
        
        public string Id { get; private set; }
        
        public string Name { get; private set; }
        
        public string DbConnectionString { get; private set; }
    }
}