# See LICENSE in the project root for license information.

#Requires -Version 7

################################################################################
#region Preamble.

<#
.SYNOPSIS
Packaging script.

.DESCRIPTION
Packaging script.

.PARAMETER Help
Print help text then exit?
#>
[CmdletBinding()]
param(
    [Alias("h")] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#endregion
################################################################################
#region Helpers.

function Print-Help {
    say @"

Packaging script.

Usage: make.ps1 [arguments]
  -h|-Help           print this help then exit

Examples.
> pack.ps1

Looking for more help?
> Get-Help -Detailed pack.ps1

"@
}

#endregion
################################################################################

if ($Help) { Print-Help ; exit }

try {
    pushd $ROOT_DIR

    say "Packing..." -ForegroundColor Yellow

    $packageDir = Join-Path $ARTIFACTS_DIR 'packages'
    $project = Join-Path $SRC_DIR "Zorglub"

    & dotnet pack $project `
        -c Release `
        -o $packageDir `
        "/p:ContinuousIntegrationBuild=true" `
        "/p:HideInternals=true" `
        "/p:PrintSettings=true"
        || die 'Failed to pack the project.'
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
