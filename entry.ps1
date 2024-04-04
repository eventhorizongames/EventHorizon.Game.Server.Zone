param(
    [string]
    [ValidateSet(
        "clean",
        "restore",
        "build",
        "watch",
        "run",
        "format",
        "check",
        "test",
        "publish",
        "serve"
    )]
    $Command,
    [string] $PactUrl,
    [string] $Configuration = "Release"
)

# Base Env Varaiables - Override with .entry-env file
$serverProject = "./src/EventHorizon.Game.Server.Zone/EventHorizon.Game.Server.Zone.csproj"

switch ($Command) {
    "clean" {
        dotnet clean
    }
    "restore" {
        dotnet restore --no-cache
    }
    "build" {
        dotnet build
    }
    "watch" {
        dotnet watch --project $serverProject
    }
    "run" {
        dotnet run --project $serverProject
    }
    "format" { 
        dotnet csharpier .
    }
    "check" {
        dotnet build --no-incremental -c $Configuration
        dotnet csharpier --check .
    }
    "test" { 
        # dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info $testsProject
        dotnet test
    }
    "publish" { 
        ./publish.ps1
    }
    "serve" {
        ./entry.ps1 publish
        dotnet serve -d="./output/wwwroot"
    }
    Default {
        Write-Output "Invalid Command"
    }
}