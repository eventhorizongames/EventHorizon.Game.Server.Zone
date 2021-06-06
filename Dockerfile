FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-publish
WORKDIR /source

COPY *.sln .
COPY src/. ./src/
COPY test/. ./test/

RUN dotnet build
RUN dotnet publish --output /app --configuration Release


# Build the Server Script Sub Process 
## This allows for easier memory cleanup, since this will run in a sub process from the main server process.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS server-scripts-sub-process
WORKDIR /source

## Projects
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

# Restore Server SubProcess
RUN dotnet restore ./src/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess.csproj

# Copy all SubProcess Project
COPY . .

## Single folder publish of project
RUN dotnet publish --output /sub-processes/server-scripts/ --configuration Release ./src/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess


#########################
# Stage - server-scripts-sub-process
## Build the Server Script Sub Process 
## This allows for easier memory cleanup, since this will run in a sub process from the main server process.
#########################
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS server-scripts-sub-process
WORKDIR /source

## Projects
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

# Restore Server SubProcess
RUN dotnet restore ./src/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess.csproj

# Copy all SubProcess Project
COPY . .

## Single folder publish of project
RUN dotnet publish --output /sub-processes/server-scripts/ --configuration Release ./src/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess


#########################
# Stage - client-scripts-sub-process 
## Build the Client Script Sub Process 
## This allows for easier memory cleanup, since this will run in a sub process from the main server process.
#########################
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS client-scripts-sub-process
WORKDIR /source

## Projects
COPY src/EventHorizon.Extensions/EventHorizon.Extensions.csproj ./src/EventHorizon.Extensions/EventHorizon.Extensions.csproj
COPY src/EventHorizon.Monitoring.Events/EventHorizon.Monitoring.Events.csproj ./src/EventHorizon.Monitoring.Events/EventHorizon.Monitoring.Events.csproj
COPY src/EventHorizon.Zone.Core/EventHorizon.Zone.Core.csproj ./src/EventHorizon.Zone.Core/EventHorizon.Zone.Core.csproj
COPY src/EventHorizon.Zone.Core.Model/EventHorizon.Zone.Core.Model.csproj ./src/EventHorizon.Zone.Core.Model/EventHorizon.Zone.Core.Model.csproj
COPY src/EventHorizon.Zone.Core.Events/EventHorizon.Zone.Core.Events.csproj ./src/EventHorizon.Zone.Core.Events/EventHorizon.Zone.Core.Events.csproj
COPY src/EventHorizon.Zone.System.Client.Scripts.Model/EventHorizon.Zone.System.Client.Scripts.Model.csproj ./src/EventHorizon.Zone.System.Client.Scripts.Model/EventHorizon.Zone.System.Client.Scripts.Model.csproj
COPY src/EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler/EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.csproj ./src/EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler/EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.csproj
COPY src/EventHorizon.Zone.System.Client.Scripts.Plugin.Shared/EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.csproj ./src/EventHorizon.Zone.System.Client.Scripts.Plugin.Shared/EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.csproj

COPY src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess.csproj ./src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess.csproj

RUN dotnet restore ./src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess.csproj

# Publish SubProcess Project

COPY src/EventHorizon.Extensions ./src/EventHorizon.Extensions
COPY src/EventHorizon.Monitoring.Events ./src/EventHorizon.Monitoring.Events/
COPY src/EventHorizon.Zone.Core ./src/EventHorizon.Zone.Core
COPY src/EventHorizon.Zone.Core.Model ./src/EventHorizon.Zone.Core.Model
COPY src/EventHorizon.Zone.Core.Events ./src/EventHorizon.Zone.Core.Events
COPY src/EventHorizon.Zone.System.Client.Scripts.Model ./src/EventHorizon.Zone.System.Client.Scripts.Model
COPY src/EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler ./src/EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler
COPY src/EventHorizon.Zone.System.Client.Scripts.Plugin.Shared ./src/EventHorizon.Zone.System.Client.Scripts.Plugin.Shared

COPY src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess ./src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess

COPY src/EventHorizon.Game.Server.Zone/appsettings.json ./src/EventHorizon.Game.Server.Zone/appsettings.json

## Single folder publish of whole project
RUN dotnet publish --no-restore --output /sub-processes/client-scripts/ --configuration Release ./src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess


# Runtime Zone Server target
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
ARG BUILD_VERSION=0.0.0
ENV APPLICATION_VERSION=$BUILD_VERSION

WORKDIR /app
COPY --from=build-publish /app /app
COPY --from=server-scripts-sub-process /sub-processes/server-scripts /sub-processes/server-scripts
COPY --from=client-scripts-sub-process /sub-processes/client-scripts /sub-processes/client-scripts
ENTRYPOINT ["dotnet", "EventHorizon.Game.Server.Zone.dll"]
