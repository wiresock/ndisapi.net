$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot 'DriverNameTests/DriverNameTests.csproj'
$testDirectory = Split-Path $project
$output = Join-Path $testDirectory 'bin/x64/Release/net8.0'

# Select x64 even when called from a Developer PowerShell targeting x86.
if ($env:VSCMD_ARG_TGT_ARCH -ne 'x64' -or -not (Get-Command cl.exe -ErrorAction SilentlyContinue)) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
    $visualStudio = & $vswhere -latest -products '*' -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath
    if (-not $visualStudio) { throw 'Install the Visual Studio C++ x64 build tools.' }
    $vcvars = Join-Path $visualStudio 'VC/Auxiliary/Build/vcvarsall.bat'
    & cmd.exe /d /s /c "call `"$vcvars`" x64 >nul && set" | ForEach-Object {
        if ($_ -match '^([^=]+)=(.*)$') {
            [Environment]::SetEnvironmentVariable($matches[1], $matches[2], 'Process')
        }
    }
    if ($LASTEXITCODE -ne 0) { throw 'Could not configure the C++ build tools.' }
}

$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:MSBuildEnableWorkloadResolver = 'false'
$env:DOTNET_CLI_WORKLOAD_UPDATE_NOTIFY_DISABLE = 'true'

& dotnet build $project -c Release -p:Platform=x64 -p:TargetFrameworks=net8.0 -p:LangVersion=12.0 -p:GeneratePackageOnBuild=false -p:NuGetAudit=false --disable-build-servers --verbosity minimal
if ($LASTEXITCODE -ne 0) { throw 'The regression harness build failed.' }

Push-Location $output
try {
    & cl.exe /nologo /LD /MT /EHsc /W4 (Join-Path $testDirectory 'native-stub.cpp') /Fostub.obj /link /OUT:ndisapi.dll /IMPLIB:stub.lib
    if ($LASTEXITCODE -ne 0) { throw 'The native test stub build failed.' }
    & dotnet (Join-Path $output 'DriverNameTests.dll')
    $testExitCode = $LASTEXITCODE
}
finally { Pop-Location }
exit $testExitCode
