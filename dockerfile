# use aspnet 3.1 image as our base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

# copy the output from the host to a folder in the image
COPY src/Sample/Sample.WebApi1/bin/Release/netcoreapp3.1/publish/ WebApi1/

# switch folder in the image
WORKDIR /WebApi1

EXPOSE 5004

# start up of the image is dotnet sample.webapi1.dll
ENTRYPOINT ["dotnet", "Sample.WebApi1.dll"]
# build project
# dotnet build -c release

# publish project 
# dotnet build -c release

# build docker image
# docker build -t sample/multitenancy -f dockerfile .

# run docker container
# docker run -t -p 5004:5004 sample/multitenancy -d

# OR better still
# docker-compose up
