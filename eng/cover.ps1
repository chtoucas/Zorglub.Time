# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $Smoke,

                 [switch] $NoBuild,
                 [switch] $NoTest,
                 [switch] $NoReport,

    [Alias("h")] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Code coverage script.

Usage: cover.ps1 [arguments]
  -c|-Configuration  the configuration to test the solution for. Default = "Debug".
     -Smoke
     -NoBuild        do NOT build the test suite?
     -NoTest         do NOT execute the test suite? Implies -NoBuild
     -NoReport       do NOT run ReportGenerator?
  -h|-Help           print this help then exit

Examples.
> cover.ps1             # Run Coverlet then build an HTML report
> cover.ps1 -NoReport   # Run Coverlet, do NOT build an HTML report

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $assemblyName = 'Zorglub.Time'
    $format   = 'opencover'

    $outDir   = Join-Path $ArtifactsDir "tests-Zorglub-$configuration\".ToLowerInvariant()
    $output   = Join-Path $outDir "$format.xml"
    $rgInput  = Join-Path $outDir "$format.*xml"
    $rgOutput = Join-Path $outDir 'html'
    # Filters: https://github.com/Microsoft/vstest-docs/blob/main/docs/filter.md
    $includes = @("[$assemblyName]*")
    $excludes = @("[$assemblyName]System.*")
    $include = '"' + ($includes -join '%2c') + '"'
    $exclude = '"' + ($excludes -join '%2c') + '"'

    $args = @("-c:$Configuration")

    if ($NoTest) { $NoBuild = $true }

    if ($NoBuild) {
        $args += '--no-build'
    } else {
        & dotnet build $TestProject $args
            || die 'Failed to build the project'
    }

    if (-not $NoTest) {
        $args += $Smoke ? "--filter:$SmokeTestsFilters" : "--filter:$RegularTestsFilters"

        & dotnet test $TestProject $args `
            /p:ExcludeByAttribute=DebuggerNonUserCode `
            /p:DoesNotReturnAttribute=DoesNotReturnAttribute `
            /p:CollectCoverage=true `
            /p:CoverletOutputFormat=$format `
            /p:CoverletOutput=$output `
            /p:Include=$include `
            /p:Exclude=$exclude
            || die 'Failed to run the test suite.'
    }

    if (-not $NoReport) {
        if (Test-Path $rgOutput) {
            Remove-Item $rgOutput -Force -Recurse
        }

        & dotnet tool run reportgenerator `
            -reporttypes:"Html" `
            -reports:$rgInput `
            -targetdir:$rgOutput
            || die 'Failed to create the reports.'
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
