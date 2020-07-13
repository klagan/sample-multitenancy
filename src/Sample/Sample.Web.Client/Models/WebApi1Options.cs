namespace Sample.Web.Client.Models
{
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;

    public class WebApi1Options : AzureADOptions
    {
        public string BaseAddress { get; set; }
    }
}