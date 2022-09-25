# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Benchmark script.

Usage: benchmark.ps1 [arguments]
  -h|-Help           print this help then exit

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $benchmarkProject = Join-Path $TestDir 'Benchmarks' -Resolve

    & dotnet run -c Release --project $benchmarkProject `
        -p:AnalysisMode=AllDisabledByDefault
}
finally {
    popd
}
