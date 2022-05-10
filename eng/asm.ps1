# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateNotNullOrEmpty()]
    [string] $Path
)

New-Variable ROOT_DIR (Get-Item $PSScriptRoot).Parent.FullName `
    -Scope Script -Option Constant

# ------------------------------------------------------------------------------

try {
    pushd $ROOT_DIR

    if (-not (Test-Path $Path)) {
        Write-Error "The file does NOT exist."
    }
    if (-not [System.IO.Path]::IsPathRooted($Path)) {
        $Path = Join-Path (Get-Location) $Path -Resolve
    }

    $asm = [System.Reflection.AssemblyName]::GetAssemblyName($Path)
    # System.Diagnostics.FileVersionInfo
    $fileInfo = Get-Item $Path | % VersionInfo

    Write-Host "`nAssembly's Full Name."
    Write-Host $asm.FullName

    Write-Host "`nAssembly's Version Attributes."
    Write-Host ("AssemblyVersion      = {0}" -f $asm.Version)
    Write-Host ("FileVersion          = {0}" -f $fileInfo.FileVersion)
    Write-Host ("InformationalVersion = {0}" -f $fileInfo.ProductVersion)

    Write-Host "`nFile Informations."
    Write-Host $fileInfo
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
