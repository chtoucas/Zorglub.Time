# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
                 [switch] $Artifacts,
    [Alias('b')] [switch] $BinAndObj,
                 [switch] $PackagesLock,
                 [switch] $Vss,

                 [switch] $Soft,
    [Alias('a')] [switch] $All,
    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Cleanup script. This script does nothing unless you specifiy at least one option.

Usage: reset.ps1 [arguments]
     -Artifacts     delete the folder "__" containing the artifacts
  -b|-BinAndObj     delete all folders "bin" and "obj".
     -PackagesLock  delete all files "packages.lock.json".
     -Vss           delete the folder ".vs" containing the Visual Studio settings

     -Soft          remove untracked files from the working tree
  -a|-All

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

    if ($Soft) {
        say "Use `git clean --dry-run [-d -X]`."
        exit 0
    }

    if ($All -or $Artifacts) {
        say "Deleting ""$ArtifactsDir""."
        if (Test-Path $ArtifactsDir) {
            rm $ArtifactsDir -Recurse
        }
    }

    if ($All -or $BinAndObj) {
        Remove-BinAndObj (Join-Path $RootDir 'src' -Resolve)
        Remove-BinAndObj (Join-Path $RootDir 'test' -Resolve)
    }

    if ($All -or $PackagesLock) {
        say "Deleting ""packages.lock.json""."
        gci -Recurse -File -Filter "packages.lock.json"
            | foreach { Write-Verbose "Deleting ""$_""." ; rm $_.FullName }
    }

    if ($All -or $Vss) {
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
