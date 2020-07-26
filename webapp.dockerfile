# use aspnet 3.1 image as our base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

# copy the output from the host to a folder in the image
COPY src/Sample/Sample.Web.Client/bin/Release/netcoreapp3.1/publish/ WebApp/

# switch folder in the image
WORKDIR /WebApp

EXPOSE 5005

# start up of the image is dotnet sample.webapi1.dll
ENTRYPOINT ["dotnet", "Sample.Web.Client.dll"]

