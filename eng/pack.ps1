# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param()

. (Join-Path $PSScriptRoot 'zorglub.ps1')

try {
    pushd $RootDir

    $project = Join-Path $SrcDir 'Zorglub' -Resolve
    $packageDir = Join-Path $ArtifactsDir 'packages'

    & dotnet pack $project `
        -c Release `
        -o $packageDir `
        /p:ContinuousIntegrationBuild=true `
        /p:HideInternals=true `
        /p:PrintSettings=true
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
