# See LICENSE in the project root for license information.

#Requires -Version 7

New-Variable ROOT_DIR (Get-Item $PSScriptRoot).Parent.FullName -Scope Script -Option Constant
New-Variable SRC_DIR       (Join-Path $ROOT_DIR 'src')  -Scope Script -Option Constant
New-Variable TEST_DIR      (Join-Path $ROOT_DIR 'test') -Scope Script -Option Constant
New-Variable ARTIFACTS_DIR (Join-Path $ROOT_DIR '__')   -Scope Script -Option Constant
New-Variable TEST_PROJECT  (Join-Path $TEST_DIR "Zorglub.Tests") -Scope Script -Option Constant

New-Alias "say" Write-Host

function die([string] $message) { Write-Error $message ; exit 1 }
