FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/EventHorizon.Game.Server.Zone.csproj ./src/EventHorizon.Game.Server.Zone.csproj
COPY test/EventHorizon.Game.Server.Zone.Tests.csproj ./test/EventHorizon.Game.Server.Zone.Tests.csproj
RUN dotnet restore

# copy and build everything else
COPY src/. ./src/
COPY test/. ./test/

RUN dotnet build

# Details on how to use: https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-dev
FROM build AS testrunner
WORKDIR /source/test
ENTRYPOINT ["dotnet", "test", "--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=lcov", "/p:CoverletOutput=./TestResults//lcov.info", "./EventHorizon.Game.Server.Zone.Tests.csproj"]

FROM build AS test
WORKDIR /source/test
RUN dotnet test

FROM build AS publish
WORKDIR /source
RUN dotnet publish --output bin/publish --configuration Release

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=publish /source/src/bin/publish ./
ENTRYPOINT ["dotnet", "EventHorizon.Game.Server.Zone.dll"]