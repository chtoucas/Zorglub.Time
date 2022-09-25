# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateNotNullOrEmpty()]
    [string] $Path
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

try {
    pushd $RootDir

    if (-not (Test-Path $Path)) {
        Write-Error "The file does NOT exist."
    }
    if (-not [System.IO.Path]::IsPathRooted($Path)) {
        $Path = Join-Path (Get-Location) $Path -Resolve
    }

    $asm = [System.Reflection.AssemblyName]::GetAssemblyName($Path)
    # System.Diagnostics.FileVersionInfo
    $fileInfo = Get-Item $Path | % VersionInfo

    say "`nAssembly's Full Name."
    say $asm.FullName

    say "`nAssembly's Version Attributes."
    say ("AssemblyVersion      = {0}" -f $asm.Version)
    say ("FileVersion          = {0}" -f $fileInfo.FileVersion)
    say ("InformationalVersion = {0}" -f $fileInfo.ProductVersion)

    say "`nFile Informations."
    say $fileInfo
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
