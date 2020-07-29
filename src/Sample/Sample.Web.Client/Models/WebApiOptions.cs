namespace Sample.Web.Client.Models
{
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;

    public class WebApiOptions : AzureADOptions
    {
        public string BaseAddress { get; set; }
    }
}