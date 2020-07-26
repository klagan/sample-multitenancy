# use aspnet 3.1 image as our base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

# copy the output from the host to a folder in the image
COPY src/Sample/Sample.WebApi1/bin/Release/netcoreapp3.1/publish/ WebApi1/

# switch folder in the image
WORKDIR /WebApi1

EXPOSE 5004

# start up of the image is dotnet sample.webapi1.dll
ENTRYPOINT ["dotnet", "Sample.WebApi1.dll"]

