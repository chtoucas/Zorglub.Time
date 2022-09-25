# See LICENSE in the project root for license information.

# Adapted from https://github.com/dotnet/roslyn/tree/master/scripts/PublicApi

#Requires -Version 7

[CmdletBinding()]
param(
    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Update the PublicAPI files:
- Unshipped members are moved to PublicAPI.Shipped.txt.
- Obsolete members are moved from PublicAPI.Shipped.txt to PublicAPI.Unshipped.txt
  and prefixed w/ *REMOVED*.

Usage: freeze-api.ps1 [arguments]
  -h|-Help           print this help then exit

"@
}

function Update-PublicAPI {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string] $dir
    )

    say "`nProcessing $dir" -Foreground Magenta

    $annotations = @("#nullable enable")

    # WARNING: Get-Content returns a string if the file has only one line,
    # in which case "$shippedContent += $item" (see below) is interpreted as a
    # string concatenation, therefore we MUST force the array context.

    $unshippedPath = Join-Path $dir "PublicAPI.Unshipped.txt" -Resolve
    $unshippedContent = @(Get-Content $unshippedPath)

    $shippedPath = Join-Path $dir "PublicAPI.Shipped.txt" -Resolve
    $shippedContent = @(Get-Content $shippedPath)
    if (-not $shippedContent) { $shippedContent = $annotations }

    $removedContent = @()

    foreach ($item in $unshippedContent) {
        if ($item.Length -gt 0) {
            if ($item.StartsWith("*REMOVED*", "InvariantCultureIgnoreCase")) {
                # Removed entries may only appear within PublicAPI.Unshipped.txt.
                $removedContent += $item
            }
            elseif (-not $item.StartsWith("#", "InvariantCultureIgnoreCase")) {
                # We ignore annotations.
                $shippedContent += $item
            }
        }
    }

    # NB: the original $shippedContent may contain empty lines.
    $shippedContent = $shippedContent | where { $_.Length -gt 0 } | Sort-Object
    $removedContent = $removedContent | Sort-Object

    say "Writing PublicAPI.Shipped.txt."
    $shippedContent | Out-File $shippedPath -Encoding UTF8

    say "Writing PublicAPI.Unshipped.txt."
    $removedContent | Out-File $unshippedPath -Encoding UTF8

    say "Public API files successfully updated." -Foreground Green
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

# ------------------------------------------------------------------------------

say "Hello, this is the script to update the Public API files."

try {
    pushd $SrcDir

    foreach ($file in Get-ChildItem -Recurse -Include "PublicApi.Unshipped.txt") {
        $dir = Split-Path -Parent $file
        Update-PublicAPI $dir
    }
}
finally {
    popd
}

################################################################################
