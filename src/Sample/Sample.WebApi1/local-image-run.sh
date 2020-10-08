docker run \
--rm \
-d \
--name webapi \
-e ASPNETCORE_URLS=http://+:8080 \
-e AzureAd__ClientId=4fa0566b-8e90-4f9e-916a-1863ebfde39a \
-e Azuread__TenantId1=82d75a56-f939-4164-b05a-2a3c5328b458 \
-e Azuread__TenantId=common \
-e AzureAd__Domain=laganlabs.it \
-e ASPNETCORE_ENVIRONMENT=Development \
-p 8080:8080 \
local/webapi
