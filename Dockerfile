FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY NuGet.Config .
COPY *.sln .
COPY src/EventHorizon.Game.Server.Zone/EventHorizon.Game.Server.Zone.csproj ./src/EventHorizon.Game.Server.Zone/EventHorizon.Game.Server.Zone.csproj
COPY src/EventHorizon.Game.Server.Zone.Agent/EventHorizon.Game.Server.Zone.Agent.csproj ./src/EventHorizon.Game.Server.Zone.Agent/EventHorizon.Game.Server.Zone.Agent.csproj
COPY src/EventHorizon.Game.Server.Zone.Agent.Ai/EventHorizon.Game.Server.Zone.Agent.Ai.csproj ./src/EventHorizon.Game.Server.Zone.Agent.Ai/EventHorizon.Game.Server.Zone.Agent.Ai.csproj
COPY src/EventHorizon.Game.Server.Zone.Events/EventHorizon.Game.Server.Zone.Events.csproj ./src/EventHorizon.Game.Server.Zone.Events/EventHorizon.Game.Server.Zone.Events.csproj
COPY src/EventHorizon.Game.Server.Zone.External/EventHorizon.Game.Server.Zone.External.csproj ./src/EventHorizon.Game.Server.Zone.External/EventHorizon.Game.Server.Zone.External.csproj
COPY src/EventHorizon.Game.Server.Zone.Model/EventHorizon.Game.Server.Zone.Model.csproj ./src/EventHorizon.Game.Server.Zone.Model/EventHorizon.Game.Server.Zone.Model.csproj

COPY test/EventHorizon.Game.Server.Zone.Tests/EventHorizon.Game.Server.Zone.Tests.csproj ./test/EventHorizon.Game.Server.Zone.Tests/EventHorizon.Game.Server.Zone.Tests.csproj

RUN dotnet restore

# copy and build everything else
COPY src/. ./src/
COPY test/. ./test/

RUN dotnet build

FROM build AS publish
WORKDIR /source
RUN dotnet publish --output bin/publish --configuration Release

FROM microsoft/dotnet:2.1.3-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=publish /source/src/EventHorizon.Game.Server.Zone/bin/publish ./
ENTRYPOINT ["dotnet", "EventHorizon.Game.Server.Zone.dll"]