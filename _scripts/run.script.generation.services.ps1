param([String]$root = "") #Must be the first statement in your script

dotnet clean;
dotnet script ./_scripts/rsp/clean.rsp.csx;
dotnet publish;
dotnet script ./_scripts/rsp/generate.rsp.file.csx;