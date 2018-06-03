# Sample contents of Dockerfile
# Stage 1
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /source

# caches restore result by copying csproj file separately
COPY *.csproj .
RUN dotnet restore

# copies the rest of your code
COPY . .
RUN dotnet publish --output /app/ --configuration Release
 
# Stage 2
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "EventHorizon.Game.Server.Zone.dll"]