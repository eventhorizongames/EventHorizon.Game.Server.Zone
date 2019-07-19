
param([String]$project="./test/EventHorizon.Game.Server.Zone.Tests")

dotnet test $project --filter Category!=Performance /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=../lcov.info 