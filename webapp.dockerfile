# use sdk image to build the project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# copy the required source code to the container
COPY src/Sample/Sample.Web.Client build/src/Sample/Sample.Web.Client
COPY src/Sample/Sample.MyAuthentication build/src/Sample/Sample.MyAuthentication

# got to the folder
WORKDIR /build/src/Sample/Sample.Web.Client

RUN dotnet restore  \
&& dotnet build --configuration Release --no-restore\
&& dotnet test --configuration Release --logger "console;verbosity=detailed" --logger trx \
&& dotnet publish --configuration Release --no-build

# use aspnet 3.1 image as our base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final

# copy the output from the host to a folder in the image
COPY --from=build /build/src/Sample/Sample.Web.Client/bin/Release/netcoreapp3.1/publish/ /WebApp  

LABEL author="kam lagan" \
      email="github@lagan.me" 

# switch folder in the image
WORKDIR /WebApp

ENV ASPNETCORE_URLS=http://+:8080
ENV AzureAd:ClientId=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
ENV AzureAd__TenantId=common
ENV AzureAd__ClientId=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
ENV AzureAd__ClientSecret=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
ENV WebApi1__ClientId=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
ENV WebApi1__BaseAddress=http://host.docker.internal:5004
ENV DOTNET_USE_POLLING_FILE_WATCHER=true 
ENV ASPNETCORE_ENVIRONMENT=Development

# start up of the image is dotnet sample.webapi1.dll
ENTRYPOINT ["dotnet", "Sample.Web.Client.dll"]

# docker build -t kamtest:latest . -f webapp.dockerfile
# docker run -it -p 5111:5552 -e ASPNETCORE_URLS="http://+:5552" kamtest:latest

