#!/bin/bash
# Setting -e will cause any error code to stop execution
set -e

dotnet test --configuration $BUILD_CONFIGURATION \
    --no-restore --no-build --filter "Category!=Performance & WindowsOnly!=True & Category!=DebugOnly" \
    /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/Coverage/ \
    --logger:"trx;LogFilePrefix=TestResult" --results-directory /TestResults/Reports /nodeReuse:false /maxCpuCount:6

ls -la /TestResults/Reports

cd /source/test/EventHorizon.Game.Server.Zone.Tests.Reporter
dotnet build
dotnet reportgenerator \
    /source/test/EventHorizon.Game.Server.Zone.Tests.Reporter/EventHorizon.Game.Server.Zone.Tests.Reporter.csproj \
    "-reports:/source/test/**/TestResults/Coverage/coverage.cobertura.xml" "-targetdir:/TestResults/Coverage/Reports" "-reporttypes:HtmlInline_AzurePipelines_Dark;Cobertura"

ls -la /TestResults/Coverage/Reports
