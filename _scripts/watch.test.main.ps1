dotnet watch --project ./test/EventHorizon.Game.Server.Zone.Tests test --filter Category!=Performance /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info 