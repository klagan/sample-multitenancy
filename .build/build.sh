docker build -t kaml/webapp:latest . -f ../webapp.dockerfile
docker run -it -p 5000:5000 -e ASPNETCORE_URLS="http://+:5000" kaml/webapp:latest

docker build -t kaml/webapi1:latest . -f ../webapi1.dockerfile
docker run -it -p 5001:5001 -e ASPNETCORE_URLS="http://+:5001" kaml/webapi1:latest
