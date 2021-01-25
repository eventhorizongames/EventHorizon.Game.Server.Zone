~~~ bash
dotnet publish -c Release .\src\EventHorizon.Game.Server.Zone\EventHorizon.Game.Server.Zone.csproj;
docker build -f Dockerfile.devops -t canhorn/ehz-platform-server-zone:dev .
~~~

~~~
# Display Docker Stats in an easier to read format
docker stats --format 'table {{.CPUPerc}}\t{{.MemUsage}}\t{{.MemPerc}}\t{{ printf \"%.25s\" .Name }}'
~~~