FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY NuGet.Config .
COPY *.sln .
COPY src/EventHorizon.Game.Server.Zone/EventHorizon.Game.Server.Zone.csproj ./src/EventHorizon.Game.Server.Zone/EventHorizon.Game.Server.Zone.csproj
COPY src/EventHorizon.Game.Server.Zone.Agent/EventHorizon.Game.Server.Zone.Agent.csproj ./src/EventHorizon.Game.Server.Zone.Agent/EventHorizon.Game.Server.Zone.Agent.csproj
COPY src/EventHorizon.Game.Server.Zone.Agent.Ai/EventHorizon.Game.Server.Zone.Agent.Ai.csproj ./src/EventHorizon.Game.Server.Zone.Agent.Ai/EventHorizon.Game.Server.Zone.Agent.Ai.csproj
COPY src/EventHorizon.Zone.Core.Model/EventHorizon.Zone.Core.Model.csproj ./src/EventHorizon.Zone.Core.Model/EventHorizon.Zone.Core.Model.csproj
COPY src/EventHorizon.Game.I18n/EventHorizon.Game.I18n.csproj ./src/EventHorizon.Game.I18n/EventHorizon.Game.I18n.csproj

# Systems
COPY src/EventHorizon.Zone.System.ServerModule/EventHorizon.Zone.System.ServerModule.csproj ./src/EventHorizon.Zone.System.ServerModule/EventHorizon.Zone.System.ServerModule.csproj

# Embedded Plugins
COPY src/EventHorizon.Plugin.Zone.System.Model/EventHorizon.Plugin.Zone.System.Model.csproj ./src/EventHorizon.Plugin.Zone.System.Model/EventHorizon.Plugin.Zone.System.Model.csproj
COPY src/EventHorizon.Plugin.Zone.System.Combat/EventHorizon.Plugin.Zone.System.Combat.csproj ./src/EventHorizon.Plugin.Zone.System.Combat/EventHorizon.Plugin.Zone.System.Combat.csproj

COPY test/EventHorizon.Game.Server.Zone.Tests/EventHorizon.Game.Server.Zone.Tests.csproj ./test/EventHorizon.Game.Server.Zone.Tests/EventHorizon.Game.Server.Zone.Tests.csproj

RUN dotnet restore

# copy and build everything else
COPY src/. ./src/
COPY test/. ./test/

RUN dotnet build

FROM build AS publish
WORKDIR /source
RUN dotnet publish --output bin/publish --configuration Release

FROM microsoft/dotnet-aspnetcore-runtime:2.2 AS runtime
WORKDIR /app
COPY --from=publish /source/src/EventHorizon.Game.Server.Zone/bin/publish ./
ENTRYPOINT ["dotnet", "EventHorizon.Game.Server.Zone.dll"]