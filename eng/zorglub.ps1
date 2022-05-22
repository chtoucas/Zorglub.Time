# See LICENSE in the project root for license information.

#Requires -Version 7

New-Variable RootDir (Get-Item $PSScriptRoot).Parent.FullName -Scope Script -Option Constant
New-Variable SrcDir       (Join-Path $RootDir 'src')  -Scope Script -Option Constant
New-Variable TestDir      (Join-Path $RootDir 'test') -Scope Script -Option Constant
New-Variable ArtifactsDir (Join-Path $RootDir '__')   -Scope Script -Option Constant

New-Variable TestProject (Join-Path $TestDir "Zorglub.Tests") -Scope Script -Option Constant
# If you change RegularTestFilter, don't forget to update the plans "more" and
# "extra" in test.ps1, but also the github action.
# WARNING: think twice before using the conditional operator | in the filter.
# It's possible, but it's much simpler not to because then we don't have to use
# parenthesis when combining the default filter with other conditionals.
New-Variable RegularTestFilter 'Performance!~Slow&Redundant!=true' -Scope Script -Option Constant

New-Alias "say" Write-Host

function die([string] $message) { say $message -Foreground Magenta ; exit 1 }
