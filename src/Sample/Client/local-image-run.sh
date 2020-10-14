docker run \
--rm \
-d \
--name webapp \
-e ASPNETCORE_URLS=http://+:8080 \
-e AzureAd__ClientId=$MultiTenant_Client_ClientId \
-e AzureAd__ClientSecret=$MultiTenant_Client_ClientSecret \
-e AzureAd__TenantId=common \
-e WebApi1__ClientId=$MultiTenant_WebApi_ClientId \
-e WebApi1__TenantId=common \
-e WebApi1__PermissionScope=$MultiTenant_WebApi_PermissionScope \
-e WebApi1__BaseAddress=http://host.docker.internal:8081 \
-e DOTNET_USE_POLLING_FILE_WATCHER=true \
-e ASPNETCORE_ENVIRONMENT=Development \
-p 8080:8080 \
local/webapp
