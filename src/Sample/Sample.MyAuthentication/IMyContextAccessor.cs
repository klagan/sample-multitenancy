namespace Sample.MyAuthentication
{
    public interface IMyContextAccessor
    {
        string MyKey { get; }
        
        string TenantId { get; }
        
        string PreferredName { get; }
        
        string Name { get; }
        
        Tenant Tenant { get; }
    }
}