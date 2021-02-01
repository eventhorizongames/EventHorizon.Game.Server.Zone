~~~ bash
dotnet publish -c Release .\src\EventHorizon.Game.Server.Zone\EventHorizon.Game.Server.Zone.csproj;
docker build -f Dockerfile.devops -t canhorn/ehz-platform-server-zone:dev .
~~~

~~~
# Display Docker Stats in an easier to read format
docker stats --format 'table {{.CPUPerc}}\t{{.MemUsage}}\t{{.MemPerc}}\t{{ printf \"%.25s\" .Name }}'
~~~

## Docker - Sub Process Testing
~~~ bash
# Build SubProcess Image
docker build --target client-scripts-sub-process -f ./Dockerfile.devops -t canhorn/ehz-platform-server-zone:sub-process-client-scripts .

# Bash into an Image
docker run -it --rm --entrypoint /bin/bash canhorn/ehz-platform-server-zone:sub-process-client-scripts

~~~
