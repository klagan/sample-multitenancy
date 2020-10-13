namespace Sample.Web.Client.Models
{
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;

    /// <summary>
    /// Configuration details of the WebAPI instance
    /// </summary>
    public class WebApiOptions : AzureADOptions
    {
        public string Name { get; set; }
        public string PermissionScope { get; set; }
    }
}