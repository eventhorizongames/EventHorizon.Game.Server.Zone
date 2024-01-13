param([String]$root = "") #Must be the first statement in your script

$ScriptRanFromDir = Get-Location
$GeneratedCoverageFile = "coverage.lcov";
$GeneratedCoverageDirectory = ".\TestResults\Coverage\$GeneratedCoverageFile"
$ReportFilePattern = "**/$GeneratedCoverageFile"
$ReportOutputDirectory = "test/EventHorizon.Game.Server.Zone.Tests.Reporter/TestResults/Coverage/Reports"
$ReportOutputFile = "$ReportOutputDirectory/lcov.info"

Write-Host "Script Ran from directory $ScriptRanFromDir"

## Using dotnet test and collect coverage
dotnet test --filter "Category!=Performance & WindowsOnly!=True" /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=$GeneratedCoverageDirectory 

## Use NodeJS to merge lcov files
### Create Reporting Directory
mkdir $ReportOutputDirectory -ErrorAction SilentlyContinue

### Fix issue with generated SF key in coverage.lcov files.
### They are currently mixing drive cases, C: and c:, so the files are duplicated incorrectly.
$MainDrive = "E".ToUpper()
$MainDriveLower = "E".ToLower()
$CoverageFiles = Get-ChildItem -Filter "*coverage.lcov*" -Recurse 
foreach ($CoverageFile in $CoverageFiles) {
    $CoverageFilePath = $CoverageFile.FullName
    # Write-Host "Updating Coverage File $CoverageFilePath"
    ((Get-Content -path $CoverageFilePath -Raw) -replace "${MainDrive}:\\", "${MainDriveLower}:\") | Set-Content -Path $CoverageFilePath
}

### RUn the Merging of LCOV files
npx lcov-result-merger@4.1.0 $ReportFilePattern $ReportOutputFile

Write-Host "Finished Test Run"