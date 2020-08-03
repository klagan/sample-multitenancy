namespace Sample.MyAuthentication
{
    using System.Collections.Generic;
    using Microsoft.IdentityModel.Tokens;

    public interface ITenantRepository
    {
        IEnumerable<string> ValidTenant();

        string ValidateIssuers(
            string issuer,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters
        );
    }
}