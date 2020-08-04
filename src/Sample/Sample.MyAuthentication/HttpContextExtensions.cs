namespace Sample.MyAuthentication
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Identity.Web;

    public static class HttpContextExtensions
    {
        public static string TenantId(
            this HttpContext context
        )
        {
            return context.User.GetTenantId();
        }
        
        public static string GetMyKey(this HttpContext context)
        {
            return context.GetValue<string>(MyConstants.MyKey);
        }
        
        public static Tenant GetMyTenant(this HttpContext context)
        {
            return context.GetValue<Tenant>(MyConstants.TenantKey);
        }
        
        private static T GetValue<T>(this HttpContext context, string keyName) 
            where T : class
        {
            if (!context.Items.ContainsKey(keyName))
                throw new KeyNotFoundException($"{keyName} not found in context");
            
            return context.Items[keyName] as T;
        }
    }
}