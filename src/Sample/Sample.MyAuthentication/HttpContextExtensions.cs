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
            return context.GetValue("MyKey");
        }
        
        private static string GetValue(this HttpContext context, string keyName)
        {
            if (!context.Items.ContainsKey(keyName))
                throw new KeyNotFoundException($"{keyName} not found in context");
            
            // TODO: test conversions
            return context.Items[keyName] as string;
        }
    }
}