dotnet restore
dotnet publish -c Release -o ./app
docker pull microsoft/dotnet:2.1-aspnetcore-runtime
docker build -t qsocollector .
docker save -o qsocollector.tar qsocollector
rem docker load -i qsocollector.tar
rem docker swarm init
rem docker service create --name qsocollector_service --replicas 5 --restart-condition on-failure --publish 80:80 qsocollector
rem docker service rm qsocollector_service
rem docker swarm leave --force