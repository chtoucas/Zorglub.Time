# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
                 [switch] $NoExtras,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Packaging script.

Usage: pack.ps1 [arguments]
     -NoExtras       do NOT pack Zorglub.Time.Extras?
  -h|-Help           print this help then exit

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = "-c:Release",
        "/p:ContinuousIntegrationBuild=true",
        "/p:HideInternals=true",
        "/p:PrintSettings=true"

    $ZorglubTime       = Join-Path $SrcDir 'Zorglub' -Resolve
    $ZorglubTimeExtras = Join-Path $SrcDir 'Zorglub.Extras' -Resolve

    say 'Cleaning solution...' -Foreground Magenta
    & dotnet clean -c Release -v minimal

    say "`nBuilding project Zorglub.Extras..." -Foreground Magenta
    # Safety measures:
    # - Always build Zorglub.Extras, not just Zorglub
    #   This is to ensure that HideInternals=true does not break Zorglub.Extras
    # - Delete project.assets.json (--force)
    & dotnet build $ZorglubTimeExtras $args --force

    say "`nPackaging Zorglub.Time..." -Foreground Magenta
    # Pack Zorglub.Time
    & dotnet pack $ZorglubTime $args --no-build --output $PackagesDir `
        || die "Failed to pack '$ZorglubTime'."

    say "`nPackaging Zorglub.Time.Extras..." -Foreground Magenta
    # Pack Zorglub.Time.Extras
    if (-not $NoExtras) {
        & dotnet pack $ZorglubTimeExtras $args --no-build --output $PackagesDir `
            || die "Failed to pack '$ZorglubTimeExtras'."
    }

    say "`nPackaging completed successfully" -Foreground Green
}
finally {
    popd
}
