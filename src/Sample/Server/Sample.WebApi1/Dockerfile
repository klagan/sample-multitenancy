# use sdk image to build the project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# copy the output from the host to a folder in the image
COPY . /build

# switch folder in the image
WORKDIR /build

RUN dotnet restore  \
&& dotnet build --configuration Release --no-restore\
&& dotnet test --configuration Release --logger "console;verbosity=detailed" --logger trx \
&& dotnet publish --configuration Release --no-build

# use aspnet 3.1 image as our base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final

# copy the output from the host to a folder in the image
COPY --from=build /build/bin/Release/netcoreapp3.1/publish/ /WebApp  

LABEL author="kam lagan" \
      email="github@lagan.me" 

# switch folder in the image
WORKDIR /WebApp

ENV ASPNETCORE_URLS=http://+:8080
ENV AzureAd__ClientId=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
ENV AzureAd__TenantId=common
ENV DOTNET_USE_POLLING_FILE_WATCHER=true 
ENV ASPNETCORE_ENVIRONMENT=Development

# start up of the image is dotnet sample.webapi1.dll
ENTRYPOINT ["dotnet", "Sample.WebApi1.dll"]

