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
                 [switch] $Badges,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Code coverage script.

Usage: cover.ps1 [arguments]
  -c|-Configuration  the configuration to test the solution for. Default = "Debug".
     -Smoke          use the test plan "smoke"
     -NoBuild        do NOT build the test suite?
     -NoTest         do NOT execute the test suite? Implies -NoBuild
     -NoReport       do NOT run ReportGenerator?
     -Badges         create badges?
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

    $outName  = "tests-Zorglub-$configuration"
    if ($Smoke) { $outName += "-smoke" }
    $outDir   = Join-Path $ArtifactsDir $outName.ToLowerInvariant()
    $output   = Join-Path $outDir "$format.xml"
    $rgInput  = Join-Path $outDir "$format.*xml"
    $rgOutput = Join-Path $outDir 'html'
    # Filters: https://github.com/Microsoft/vstest-docs/blob/main/docs/filter.md
    $includes = @("[$assemblyName]*")
    $excludes = @("[$assemblyName]System.*")
    $include  = '"' + ($includes -join '%2c') + '"'
    $exclude  = '"' + ($excludes -join '%2c') + '"'

    $args = @("-c:$Configuration")

    if ($NoTest) { $NoBuild = $true }

    if (-not $NoBuild) {
        & dotnet build $TestProject $args
            || die 'Failed to build the project'
    }

    if (-not $NoTest) {
        # If you change the filter, don't forget to update the plan "cover" in test.ps1.
        $filter = 'ExcludeFrom!=CodeCoverage&Performance!~Slow'
        if ($Smoke) { $filter = "ExcludeFrom!=Smoke&$filter" }
        $args += "--filter:$filter"

        & dotnet test $TestProject $args `
            --no-build `
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
            -reporttypes:"Html;Badges;TextSummary;MarkdownSummary" `
            -reports:$rgInput `
            -targetdir:$rgOutput `
            -verbosity:Warning
            || die 'Failed to create the reports.'

        if ($Badges -and $Configuration -eq 'Debug') {
            try {
                pushd $rgOutput

                cp -Force 'badge_branchcoverage.svg' (Join-Path $TestDir 'coverage_branch.svg')
                cp -Force 'badge_linecoverage.svg'   (Join-Path $TestDir 'coverage_line.svg')
                cp -Force 'badge_methodcoverage.svg' (Join-Path $TestDir 'coverage_method.svg')
                cp -Force 'badge_combined.svg' (Join-Path $TestDir 'coverage.svg')
                cp -Force 'Summary.txt' (Join-Path $TestDir 'coverage.txt')
                cp -Force 'Summary.md' (Join-Path $TestDir 'coverage.md')
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
        }
    }
}
finally {
    popd
}
