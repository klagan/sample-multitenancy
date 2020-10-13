docker run \
--rm \
-d \
--name webapi1 \
-e ASPNETCORE_URLS=http://+:8080 \
-e AzureAd__ClientId=$AzureAd__ClientId \
-e Azuread__TenantId=$AzureAd__TenantId \
-e AzureAd__Domain=AzureAd__Domain \
-e ASPNETCORE_ENVIRONMENT=Development \
-p 8081:8080 \
local/webapi
