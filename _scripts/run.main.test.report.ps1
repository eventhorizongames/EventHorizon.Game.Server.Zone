param([String]$root = "") #Must be the first statement in your script

$ScriptRanFromDir = Get-Location
$GeneratedCoverageFile = "coverage.lcov";
$GeneratedCoverageDirectory = ".\TestResults\Coverage\$GeneratedCoverageFile"
$ReportFilePattern = "**/$GeneratedCoverageFile"
$ReportOutputDirectory = "test/EventHorizon.Game.Server.Zone.Tests.Reporter/TestResults/Coverage/Reports"
$ReportOutputFile = "$ReportOutputDirectory/lcov.info"

Write-Host "Script Ran from directory $ScriptRanFromDir"

## Using dotnet test and collect coverage
dotnet test --filter Category!=Performance /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=$GeneratedCoverageDirectory 

## Use NodeJS to merge lcov files
mkdir $ReportOutputDirectory -ErrorAction SilentlyContinue
npx lcov-result-merger $ReportFilePattern $ReportOutputFile