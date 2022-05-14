# See LICENSE in the project root for license information.

#Requires -Version 7

New-Variable RootDir (Get-Item $PSScriptRoot).Parent.FullName -Scope Script -Option Constant
New-Variable SrcDir       (Join-Path $RootDir 'src')  -Scope Script -Option Constant
New-Variable TestDir      (Join-Path $RootDir 'test') -Scope Script -Option Constant
New-Variable ArtifactsDir (Join-Path $RootDir '__')   -Scope Script -Option Constant

New-Variable TestProject (Join-Path $TestDir "Zorglub.Tests") -Scope Script -Option Constant
New-Variable SmokeTestsFilters 'ExcludeFrom!=Smoke&Performance!~Slow&Redundant!=true' -Scope Script -Option Constant
New-Variable RegularTestsFilters 'ExcludeFrom!=CodeCoverage&Redundant!=true' -Scope Script -Option Constant

New-Alias "say" Write-Host

function die([string] $message) { Write-Error $message ; exit 1 }
