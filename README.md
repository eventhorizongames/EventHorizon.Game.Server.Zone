
# .NET

~~~ bash
# Run Code Coverage Testing
dotnet test --configuration Release --filter "Category!=Performance & WindowsOnly!=True & Category!=DebugOnly" /p:CollectCoverage=true
~~~

~~~ bash
# Publish SubProcesses
dotnet publish --configuration Release ./src/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess
dotnet publish --configuration Release ./src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess

# Create Output Directory SubProcesses
dotnet publish --output ./sub-processes/server-scripts/ --configuration Release ./src/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess
dotnet publish --output ./sub-processes/client-scripts/ --configuration Release ./src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess
~~~

# Docker

~~~ bash
# Create Local :dev Image
docker build -t canhorn/ehz-platform-server-zone:dev .
~~~

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
# Build Client Scripts SubProcess Image
docker build --target client-scripts-sub-process -f ./Dockerfile.devops -t canhorn/ehz-platform-server-zone:sub-process-client-scripts .

# Build Server Scripts SubProcess Image
docker build --target server-scripts-sub-process -f ./Dockerfile.devops -t canhorn/ehz-platform-server-zone:sub-process-server-scripts .

# Bash into an Image
docker run -it --rm --entrypoint /bin/bash canhorn/ehz-platform-server-zone:sub-process-client-scripts

# Bash into an Image
docker run -it --rm --entrypoint /bin/bash canhorn/ehz-platform-server-zone:sub-process-server-scripts

~~~
