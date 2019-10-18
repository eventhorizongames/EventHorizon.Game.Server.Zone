param([String]$fileName = "") #Must be the first statement in your script

$ScriptRanFromDir = Get-Location
$GeneratedCoverageFile = "coverage.lcov";
$GeneratedCoverageDirectory = ".\TestResults\Coverage\$GeneratedCoverageFile"
$ReportFilePattern = "**/$GeneratedCoverageFile"
$ReportOutputDirectory = "test/EventHorizon.Game.Server.Zone.Tests.Reporter/TestResults/Coverage/Reports"
$ReportOutputFile = "$ReportOutputDirectory/lcov.info"
$projectName = $fileName.Replace("test\", "").Replace("src\", "").Split("\")[0];
if(!$projectName.EndsWith(".Tests")) {
    $projectName += ".Tests";
}
$projectPath = "test/$projectName";
$projectFile = $projectName + ".csproj";
$project = "$projectPath/$projectFile"

Write-Host "Script Ran from directory $ScriptRanFromDir"
Write-Host "Project = $project"

## Using dotnet test and collect coverage
dotnet test $project --filter "Category!=Performance & WindowsOnly!=True" /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=$GeneratedCoverageDirectory 

## Use NodeJS to merge lcov files
mkdir $ReportOutputDirectory -ErrorAction SilentlyContinue
npx lcov-result-merger $ReportFilePattern $ReportOutputFile