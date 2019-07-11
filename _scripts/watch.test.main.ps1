param([String]$project="./test/EventHorizon.Game.Server.Zone.Tests") #Must be the first statement in your script

dotnet watch --project $project test --filter Category!=Performance /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=../lcov.info 