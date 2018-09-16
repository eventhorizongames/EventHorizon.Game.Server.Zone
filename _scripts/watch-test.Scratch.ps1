
dotnet watch --project ./test test --filter Category=Scratch /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.performance.info 