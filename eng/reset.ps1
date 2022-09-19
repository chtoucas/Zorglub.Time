# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [switch] $Artifacts,
    [switch] $VS,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Cleanup script. Remove all folders "bin" and "obj".

Usage: reset.ps1 [arguments]
     -Artifacts     delete also the folder "__" containing the artifacts
     -VS            delete also the folder ".vs" containing the Visual Studio settings
  -h|-Help          print this help then exit

"@
}

#-------------------------------------------------------------------------------

function Remove-BinAndObj {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateNotNullOrEmpty()]
        [string] $path
    )

    say "Deleting ""bin"" and ""obj"" within ""$path""."

    if (-not (Test-Path $path)) {
        return Write-Verbose "Skipping ""$path""; the file does NOT exist."
    }
    if (-not [System.IO.Path]::IsPathRooted($path)) {
        return Write-Error "Skipping ""$path""; the path MUST be absolute."
    }

    ls $path -Include bin,obj -Recurse `
        | foreach { Write-Verbose "Deleting ""$_""." ; rm $_.FullName -Recurse }
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    Remove-BinAndObj (Join-Path $RootDir 'src' -Resolve)
    Remove-BinAndObj (Join-Path $RootDir 'test' -Resolve)

    if ($Artifacts) {
        say "Deleting ""$ArtifactsDir""."
        if (Test-Path $ArtifactsDir) {
            rm $ArtifactsDir -Recurse
        }
    }

    if ($VS) {
        $vsDir = Join-Path $RootDir '.vs'
        say "Deleting ""$vsDir""."
        if (Test-Path $vsDir) {
            # -Force because the folder is hidden.
            rm $vsDir -Recurse -Force
        }
    }
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
