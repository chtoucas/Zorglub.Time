# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $Smoke,

                 [switch] $NoExtras,
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
     -NoExtras       do NOT include Zorglub.Time.Extras in the reports?
     -NoBuild        do NOT build the test suite?
     -NoTest         do NOT execute the test suite? Implies -NoBuild
     -NoReport       do NOT run ReportGenerator?
     -Badges         create badges?
  -h|-Help           print this help then exit

Be sure to restore the NuGet tools before running this script:
> dotnet tool restore

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
    $extrasAssemblyName = 'Zorglub.Time.Extras'
    $format   = 'opencover'

    $outName  = "cover-Zorglub"
    if (-not $NoExtras) { $outName += "-extras" }
    if ($Smoke) { $outName += "-smoke" }
    $outName += "-$configuration"
    $outDir   = Join-Path $ArtifactsDir $outName.ToLowerInvariant()
    $output   = Join-Path $outDir "$format.xml"
    $rgInput  = Join-Path $outDir "$format.*xml"
    $rgOutput = Join-Path $outDir 'html'

    # Filters: https://github.com/Microsoft/vstest-docs/blob/main/docs/filter.md
    $includes = @("[$assemblyName]*")
    $excludes = @("[$assemblyName]System.*")
    if (-not $NoExtras) {
        $includes += "[$extrasAssemblyName]*"
        $excludes += "[$extrasAssemblyName]System.*"
    }
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

        $publish = $Badges -and -not $Smoke -and $Configuration -eq 'Debug'
        $reporttypes = $publish ? 'Html;Badges;TextSummary;MarkdownSummary' : 'Html'

        say 'Creating the reports...'

        & dotnet tool run reportgenerator `
            -reporttypes:$reporttypes `
            -reports:$rgInput `
            -targetdir:$rgOutput `
            -verbosity:Warning
            || die 'Failed to create the reports.'

        if ($publish) {
            try {
                pushd $rgOutput

                say 'Publishing the reports...'

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

    say "`nCode coverage completed successfully" -Foreground Green
}
finally {
    popd
}
