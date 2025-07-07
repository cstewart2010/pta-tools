# check environment
if (!$env:MongoDBConnectionString){
    throw "Missing MongoDBConnectionString Environment Variable"
}

if (!$env:MongoUsername){
    throw "Missing MongoUsername Environment Variable"
}

if (!$env:MongoPassword){
    throw "Missing MongoPassword Environment Variable"
}

if (!$env:CookieKey){
    throw "Missing CookieKey Environment Variable"
}

# prebuild
Write-Host Running .\testbuild.ps1
$currentDirectory = (Get-Location).Path
$repository = Split-Path -Path $MyInvocation.MyCommand.Path -Parent
Set-Location -Path $repository

# build
mongosh $env:MongoDBConnectionString -f database\scripts\update.js
dotnet build src/PTABackend.sln --configuration Release -p:Version="0.1.$env:BUILD_NUMBER.$env:BUILD_NUMBER"
$proc = Start-Process -FilePath ".\src\MongoDbImportTool\bin\Release\netcoreapp3.1\MongoDbImportTool.exe" -NoNewWindow -PassThru -Wait
if ($proc.ExitCode -ne 0){
    return 1
}

# test
Write-Host "Updating the Test Environment"
$env:MongoDBConnectionString = "mongodb+srv://$($env:MongoUsername):$($env:MongoPassword)@ptatestcluster.1ekcs.mongodb.net/test?retryWrites=true&w=majority"
mongosh $env:MongoDBConnectionString -f  .\database\scripts\update.js
$env:Database = "test"
$proc = Start-Process -FilePath ".\src\MongoDbImportTool\bin\Release\netcoreapp3.1\MongoDbImportTool.exe" -PassThru -Wait
if ($proc.ExitCode -ne 0){
    return 1
}
dotnet test .\src\PTABackEnd.sln --logger:"trx;LogFileName=C:\Users\zachagrey\.jenkins\workspace\PTA backend develop build/TestOutput.trx" --filter:Category=smoke

# postbuild
mongosh $env:MongoDBConnectionString -f .\database\scripts\catalog_logs.js
Set-Location -Path $currentDirectory
if ($env:BUILD_NUMBER)
{
    New-Item -ItemType Directory -Force -Path .\$env:BUILD_NUMBER | Out-Null
    Compress-Archive -Path .\database\scripts\* -DestinationPath .\$env:BUILD_NUMBER\InstallTools.zip -Update
    Compress-Archive -Path .\src\TheReplacement.PTA.Services.Core\bin\Release\**\* -DestinationPath .\$env:BUILD_NUMBER\PTA_Backend.zip -Update
    Compress-Archive -Path .\src\MongoDbImportTool\bin\Release\**\* -DestinationPath .\$env:BUILD_NUMBER\ImportTool.zip -Update
}