docker run \
--rm \
-d \
--name webapi1 \
-e ASPNETCORE_URLS=http://+:8080 \
-e AzureAd__ClientId=$MultiTenant_WebApi_ClientId \
-e Azuread__TenantId=$MultiTenant_WebApi_TenantId \
-e AzureAd__Domain=$MultiTenant_WebApi_Domain \
-e ASPNETCORE_ENVIRONMENT=Development \
-p 8081:8080 \
local/webapi1
