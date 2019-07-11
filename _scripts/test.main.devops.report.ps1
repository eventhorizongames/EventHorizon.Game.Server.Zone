param([String]$root="") #Must be the first statement in your script

## https://medium.com/bluekiri/code-coverage-in-vsts-with-xunit-coverlet-and-reportgenerator-be2a64cd9c2f 
dotnet reportgenerator "-reports:.\TestResults\Coverage\coverage.cobertura.xml" "-targetdir:.\TestResults\Coverage\Reports" -reportTypes:htmlInline



dotnet reportgenerator "-reports:../*/TestResults/Coverage.cobertura.xml" "-targetdir:TestResults/Coverage/Reports" "-reporttypes:HtmlInline_AzurePipelines_Dark;Cobertura"