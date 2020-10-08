namespace Sample.Web.Client.Services
{
    using Models;

    public interface IWebApiRepository
    {
        WebApiOptions GetBy(
            string tenantId
        );
    }
}