namespace Sample.MyAuthentication
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.IdentityModel.Tokens;

    public class TenantRepository : ITenantRepository
    {
        // TODO:: add dbcontext to source tenants from dynamic source
        
        public IEnumerable<string> ValidTenant()
        {
            // TODO: package this up into a more dynamic solution
            // If you are an ISV who wants to make the Web app available only to certain customers who
            // are paying for the service, you might want to fetch this list of accepted tenant ids from
            // a database.
            // Here for simplicity we just return a hard-coded list of TenantIds.
            return new[]
            {
                "82d75a56-f939-4164-b05a-2a3c5328b458",  // laganlabs.it
                "100d1e66-3613-4505-91d6-b6c20c6370f9",  // test24.uk
                "<Another GUID>"
            };            
        }
        
        public string ValidateIssuers(string issuer, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            // TODO: have changed accessTokenAcceptedVersion in AAD manifest - need to check any impact
            // https://github.com/AzureAD/microsoft-authentication-library-for-js/issues/560
            var validIssuers = ValidTenant()
                .Select(tid => $"https://login.microsoftonline.com/{tid}/v2.0"); // v2
            //.Select(tid => $"https://sts.windows.net/{tid}/"); // v1
            
            if (validIssuers.Contains(issuer))
            {
                return issuer;
            }
            
            throw new SecurityTokenInvalidIssuerException(
                "The sign-in user's account does not belong to one of the tenants that this Web App accepts users from.")
            {
                InvalidIssuer = issuer
            };
        }
    }
}