# See LICENSE in the project root for license information.

#Requires -Version 7

################################################################################
#region Preamble.

<#
.SYNOPSIS
Run the Code Coverage script and build human-readable reports.

.DESCRIPTION
Run the Code Coverage script with Coverlet, then optionally build human-readable
reports.

.PARAMETER Configuration
The configuration to test the solution for. Default (explicit) = "Debug".

.PARAMETER NoCoverage
Do NOT run any Code Coverage tool?
This option and -NoReport are mutually exclusive.

.PARAMETER NoReport
Do NOT build HTML/text reports and badges w/ ReportGenerator?
This option and -NoCoverage are mutually exclusive.

.PARAMETER Help
Print help text then exit?
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $NoCoverage,
                 [switch] $NoReport,

    [Alias("h")] [switch] $Help
)

. (Join-Path $PSScriptRoot 'common.ps1')

#endregion
################################################################################
#region Helpers.

function Print-Help {
    say @"

Code coverage script.

Usage: cover.ps1 [arguments]
  -c|-Configuration  the configuration to test the solution for
     -NoCoverage     do NOT run any Code Coverage tool?
     -NoReport       do NOT run ReportGenerator?
  -h|-Help           print this help then exit

Examples.
> cover.ps1             # Run Coverlet then build reports and badges
> cover.ps1 -NoReport   # Run Coverlet, do NOT build reports and badges

Looking for more help?
> Get-Help -Detailed cover.ps1

"@
}

# ------------------------------------------------------------------------------

function Invoke-Coverlet {
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string] $projectName,

        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string] $assemblyName,

        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string] $configuration
    )

    $format   = 'opencover'
    $outDir   = Join-Path $ARTIFACTS_DIR "tests-$projectName-$configuration\".ToLowerInvariant()
    $output   = Join-Path $outDir "$format.xml"
    $rgInput  = Join-Path $outDir "$format.*xml"
    $rgOutput = Join-Path $outDir 'html'

    if (Test-Path $rgOutput) {
        Remove-Item $rgOutput -Force -Recurse
    }

    #$project = ""
    $project = Join-Path $TEST_DIR "$projectName.Tests"

    # Common args.
    $args = @("-c:$Configuration")
    # Test args.
    $test_args = @(
        '/p:ExcludeByAttribute=DebuggerNonUserCode',
        '/p:DoesNotReturnAttribute=DoesNotReturnAttribute')
    # Filters.
    $includes = @("[$assemblyName]*")
    $excludes = @("[$assemblyName]System.*")

    $include = '"' + ($includes -join '%2c') + '"'
    $exclude = '"' + ($excludes -join '%2c') + '"'

    Write-Verbose "Include = $include"
    Write-Verbose "Exclude = $exclude"

    Write-Host "Building test project for ""$projectName""..." -ForegroundColor Yellow
    & dotnet build $project $args
        || die "Failed to build the project: ""$project""."

    # https://github.com/Microsoft/vstest-docs/blob/main/docs/filter.md
    Write-Host "`nTesting ""$projectName""..." -ForegroundColor Yellow
    & dotnet test $project $args $test_args `
        --no-build `
        --filter "ExcludeFrom!=CodeCoverage&Redundant!=true" `
        /p:CollectCoverage=true `
        /p:CoverletOutputFormat=$format `
        /p:CoverletOutput=$output `
        /p:Include=$include `
        /p:Exclude=$exclude
        || die "Failed to run the project: ""$project""."

    Write-Host "Creating reports..." -ForegroundColor Yellow
    & dotnet tool run reportgenerator `
        -reporttypes:"Html" `
        -reports:$rgInput `
        -targetdir:$rgOutput
        || die 'Failed to create the reports.'
}

#endregion
################################################################################

if ($Help) { Print-Help ; exit }

try {
    pushd $ROOT_DIR

    Invoke-Coverlet `
        -ProjectName  'Zorglub' `
        -AssemblyName 'Zorglub.Time' `
        -Configuration $Configuration
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
