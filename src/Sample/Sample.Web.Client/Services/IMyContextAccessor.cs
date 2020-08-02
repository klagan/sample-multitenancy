namespace Sample.Web.Client.Services
{
    public interface IMyContextAccessor
    {
        string MyKey { get; }
        
        string TenantId { get; }
    }
}