docker run \
--rm \
-d \
--name webapp \
-e ASPNETCORE_URLS=http://+:8080 \
-e AzureAd__ClientId=7c262eb8-2fe5-4a99-868e-75945ca3a17d \
-e AzureAd__ClientSecret=U.r4e.z.1H2Q_dE11LTsMtoUSC~Fn10KhV \
-e AzureAd__TenantId=common \
-e WebApi1__ClientId=4fa0566b-8e90-4f9e-916a-1863ebfde39a \
-e WebApi1__TenantId=common \
-e WebApi1__BaseAddress=http://host.docker.internal:8081 \
-e DOTNET_USE_POLLING_FILE_WATCHER=true \
-e ASPNETCORE_ENVIRONMENT=Development \
-p 8080:8080 \
local/webapp
