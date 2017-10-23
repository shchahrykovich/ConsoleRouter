param($publish = $false, $config = "Release")

if (Test-Path "$PSScriptRoot\output") {
   Remove-Item -Force -Recurse "$PSScriptRoot\output"
}

dotnet build -c $config

dotnet test --no-build -c $config "$PSScriptRoot\Src\ConsoleRouter.Tests"

dotnet pack "$PSScriptRoot\Src\ConsoleRouter.NetCore" --no-build -c $config -o "$PSScriptRoot\output"
dotnet pack "$PSScriptRoot\Src\ConsoleRouter.NetCore" --no-build --include-source --include-symbols -c $config -o "$PSScriptRoot\output"

if ($publish) {
   $packageName = (ls "$PSScriptRoot\output\ConsoleRouter*.nupkg" | select -first 1)[0].Name
   nuget push -Source "https://api.nuget.org/v3/index.json" "$PSScriptRoot\output\$packageName"
}