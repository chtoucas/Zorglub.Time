# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param()

function Remove-BinAndObj {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateNotNullOrEmpty()]
        [string] $path
    )

    Write-Host "Deleting ""bin"" and ""obj"" directories within ""$path""."

    if (-not (Test-Path $path)) {
        return Write-Verbose "Skipping ""$path""; the file does NOT exist."
    }
    if (-not [System.IO.Path]::IsPathRooted($path)) {
        return Write-Error "Skipping ""$path""; the path MUST be absolute."
    }

    ls $path -Include bin,obj -Recurse `
        | foreach { Write-Verbose "Deleting ""$_""." ; rm $_.FullName -Recurse }
}

try {
    $rootdir = (Get-Item $PSScriptRoot).Parent.FullName
    pushd $rootdir

    Remove-BinAndObj (Join-Path $rootdir "src" -Resolve)
    Remove-BinAndObj (Join-Path $rootdir "test" -Resolve)
    Remove-BinAndObj (Join-Path $rootdir "tools" -Resolve)
}
catch {
    Write-Host $_ -Foreground Red
    Write-Host $_.Exception
    Write-Host $_.ScriptStackTrace
    exit 1
}
finally {
    popd
}
