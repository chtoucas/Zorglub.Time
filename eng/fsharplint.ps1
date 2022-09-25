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

F# linter.

Usage: fsharplint.ps1 [arguments]
  -h|-Help           print this help then exit

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $testProject = Join-Path $TestDir 'Zorglub.Tests\Zorglub.Tests.fsproj' -Resolve
    $conf = Join-Path $EngDir 'fsharplint.json' -Resolve

    & dotnet dotnet fsharplint lint -l $conf $testProject
}
finally {
    popd
}
