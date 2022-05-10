# See LICENSE in the project root for license information.

#Requires -Version 7

################################################################################
#region Preamble.

<#
.SYNOPSIS
Test script.

.DESCRIPTION
Test script.

.PARAMETER Configuration
The configuration to test the solution for. Default (explicit) = "Debug".

.PARAMETER Help
Print help text then exit?
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $NoBuild,

    [Alias('a')] [switch] $All,
    [Alias('s')] [switch] $Solution,

    [Alias("h")] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#endregion
################################################################################
#region Helpers.

function Print-Help {
    say @"

Test script.

Usage: test.ps1 [arguments]
  -c|-Configuration  the configuration to test the solution for
  -h|-Help           print this help then exit

Examples.
> test.ps1                  # Run the core tests (default target)
> test.ps1 -All             # Run the whole test suite

Looking for more help?
> Get-Help -Detailed test.ps1

"@
}

#endregion
################################################################################

if ($Help) { Print-Help ; exit }

try {
    pushd $ROOT_DIR

    say "Testing..." -ForegroundColor Yellow

    $project = $Solution ? "" : $TEST_PROJECT

    $args = @("-c:$configuration")

    if ($NoBuild)  { $args += "--no-build" }
    if (-not $All) { $args += "--filter:Performance!~Slow" }

    & dotnet test $project $args `
        || die 'Failed to test the project.'
}
catch {
    say $_ -Foreground Red
    say $_.Exception
    say $_.ScriptStackTrace
    exit 1
}
finally {
    popd
}
