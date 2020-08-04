namespace Sample.MyAuthentication
{
    public interface IMyContextAccessor
    {
        string MyKey { get; }
        
        string TenantId { get; }
        
        string Name { get; }
        
        Tenant Tenant { get; }
    }
}