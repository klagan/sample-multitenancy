# use sdk image to build the project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# copy the required source code to the container
COPY . /build

RUN find . -name "appsettings.*.json" -type f -delete

# got to the folder
WORKDIR /build

RUN \
dotnet restore  Sample.MyAuthentication/Sample.MyAuthentication.csproj \
&& dotnet restore  Sample.Web.Client/Sample.Web.Client.csproj \
&& dotnet build --configuration Release --no-restore Sample.MyAuthentication/Sample.MyAuthentication.csproj \
&& dotnet build --configuration Release --no-restore Sample.Web.Client/Sample.Web.Client.csproj \
&& dotnet test --configuration Release --logger "console;verbosity=detailed" --logger trx Sample.MyAuthentication/Sample.MyAuthentication.csproj \
&& dotnet test --configuration Release --logger "console;verbosity=detailed" --logger trx Sample.Web.Client/Sample.Web.Client.csproj \
&& dotnet publish --configuration Release --no-build Sample.Web.Client/Sample.Web.Client.csproj

# use aspnet 3.1 image as our base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final

# copy the output from the host to a folder in the image
COPY --from=build /build/Sample.Web.Client/bin/Release/netcoreapp3.1/publish/ /WebApp

LABEL author="kam lagan" \
      email="github@lagan.me"

# switch folder in the image
WORKDIR /WebApp

ENV ASPNETCORE_URLS=http://+:8080 \
AzureAd__ClientId=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx \
AzureAd__ClientSecret=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx \
AzureAd__TenantId=common \
WebApi1__ClientId=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx \
WebApi1__TenantId=common \
WebApi1__BaseAddress='' \
DOTNET_USE_POLLING_FILE_WATCHER=true \
ASPNETCORE_ENVIRONMENT=Development

# start up of the image is dotnet sample.webapi1.dll
ENTRYPOINT ["dotnet", "Sample.Web.Client.dll"]



