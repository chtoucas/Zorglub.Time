# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('smoke', 'regular', 'more', 'extra')]
                 [string] $Plan = 'smoke',

    [Parameter(Mandatory = $false)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $Build,

    [Alias('h')] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#-------------------------------------------------------------------------------

function Print-Help {
    say @"

Run the test suite.

Usage: test.ps1 [arguments]
    |-Plan           specify the test plan. Default = "smoke"
  -c|-Configuration  the configuration to test the solution for. Default = "Debug"
    |-Build          build the project before running the test suite
  -h|-Help           print this help then exit

The default behaviour is to not build the project, and to run only the smoke
tests using the configuration Debug.

With the "regular" test plan, we exclude:
- Slow-running individual tests OR groups of tests
- Redundant tests
The three other test plans are:
- "smoke" = "regular" AND keep only one test class per test suite
- "more"  = "regular" AND do not exclude slow-running individual tests
- "extra" = complement of "more" (VERY SLOW)

Examples.
> test.ps1                      # Smoke testing
> test.ps1 smoke                # Idem
> test.ps1 regular              # Execute the regular test suite
> test.ps1 more                 # Execute the regular test suite + a few more tests
> test.ps1 extra                # (VERY SLOW) Execute the tests excluded from the test plan "more"

Typical test plan executions:
> test.ps1                              # Smoke testing, no build, Debug
> test.ps1 regular -Build -c Release    # Regular test suite, build, Release

Of course, just use dotnet to run the whole test suite or apply custom filters.

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = @("-c:$configuration")
    if (-not $Build) { $args += '--no-build' }

    switch ($Plan) {
        'smoke' {
            # Smoke testing.
            # - Only keep one test class per test suite (smoke)
            # - Exclude a bunch of tests in Postludes (slow unit)
            # - Exclude ArchetypalSchemaTestSuite (slow group)
            # - Exclude PrototypalSchemaTestSuite (slow group)
            # - Exclude redundant tests
            # Filters = ExcludeFrom!=Smoke&Performance!~Slow&Redundant!=true
            $filter += "ExcludeFrom!=Smoke&$RegularTestFilter"
        }
        'regular' {
            # Regular test suite.
            # - Exclude a bunch of tests in Postludes (slow unit)
            # - Exclude ArchetypalSchemaTestSuite (slow group)
            # - Exclude PrototypalSchemaTestSuite (slow group)
            # - Exclude redundant tests
            # Filters = Performance!~Slow&Redundant!=true
            $filter += $RegularTestFilter
        }
        'more' {
            # Extended test suite.
            # - Exclude ArchetypalSchemaTestSuite (slow group)
            # - Exclude PrototypalSchemaTestSuite (slow group)
            # - Exclude redundant tests
            $filter += 'Performance!=SlowGroup&Redundant!=true'
        }
        'extra' {
            $filter += 'Performance=SlowGroup|Redundant=true'
        }
    }
    $args += "--filter:$filter"

    & dotnet test $TestProject $args
        || die 'Failed to run the test suite.'
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
