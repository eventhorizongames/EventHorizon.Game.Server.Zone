param([String]$root = "") #Must be the first statement in your script

dotnet publish;

dotnet script ./_scripts/generate.rsp.file.csx;