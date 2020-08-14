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
COPY --from=build /build/src/Sample/Sample.Web.Client /WebApp  
# COPY src/Sample/Sample.Web.Client/bin/Release/netcoreapp3.1/publish/ WebApp/

# switch folder in the image
WORKDIR /WebApp

EXPOSE 5005

# start up of the image is dotnet sample.webapi1.dll
ENTRYPOINT ["dotnet", "Sample.Web.Client.dll"]

