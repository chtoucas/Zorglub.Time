# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('smoke', 'regular', 'more', 'extra', 'most', 'cover')]
                 [string] $Plan = 'smoke',

    [Parameter(Mandatory = $false)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $NoBuild,

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
    |-NoBuild        do NOT build the test suite?
  -h|-Help           print this help then exit

The default behaviour is to run the smoke tests using the configuration Debug.

The available test plans are:
- "smoke"   = smoke testing
- "regular" = exclude slow-running or redundant tests
- "more"    = "regular" AND include slow-running -individual- tests
- "extra"   = only include slow-running groups of tests and redundant tests,
              that is the tests excluded from the test plan "more"
- "most"    = the whole test suite
- "cover"   = mimic the default test plan used by the code coverage tool
              It's "regular" minus the tests that don't play well with the code
              coverage tool and a bunch of tests for types in Narvalo.Sketches

Of course, one can use "dotnet test" to run the whole test suite or to apply
custom filters.

Typical test plan executions.
> test.ps1 -NoBuild             # Smoke testing (Debug)
> test.ps1 regular              # Regular test suite (Debug)
> test.ps1 regular -c Release   # Regular test suite (Release)
> test.ps1 cover                # Convenient when not working on Sketches (Debug)

Examples.
> test.ps1 smoke                # ~27 thousand tests (FAST)
> test.ps1 cover                # ~73 thousand tests
> test.ps1 regular              # ~75 thousand tests
> test.ps1 more                 # ~82 thousand tests
> test.ps1 extra                # ~147 thousand tests (SLOW)
> test.ps1 most                 # ~229 thousand tests (SLOW)

"@
}

#-------------------------------------------------------------------------------

if ($Help) { Print-Help ; exit }

try {
    pushd $RootDir

    $args = @("-c:$configuration")
    if ($NoBuild) { $args += '--no-build' }

    switch ($Plan) {
        'smoke' {
            # Smoke testing.
            # - Exclude explicitely a bunch of tests from smoke testing
            # - Exclude slow units
            # - Exclude slow groups
            # - Exclude redundant tests (implicit exclusion)
            # - Exclude a bunch of tests for Zorglub.Sketches (implicit exclusion)
            # - Exclude tests ignored by the "regular" plan (implicit exclusion)
            #
            # If you change the filter, don't forget to update the github action.
            $filter = 'ExcludeFrom!=Smoke&Performance!~Slow'
        }
        'cover' {
            # Mimic the default test plan used by cover.ps1.
            # - Exclude explicitely a bunch of tests from code coverage
            # - Exclude slow units
            # - Exclude slow groups
            # - Exclude redundant tests (implicit exclusion)
            # - Exclude a bunch of tests for Zorglub.Sketches (implicit exclusion)
            # - Exclude tests ignored by the "regular" plan (implicit exclusion)
            $filter = 'ExcludeFrom!=CodeCoverage&Performance!~Slow'
        }
        'regular' {
            # Regular test suite.
            # - Exclude explicitely a bunch of tests from the "regular" plan.
            # - Exclude slow units
            # - Exclude slow groups
            # - Exclude redundant tests
            $filter = 'ExcludeFrom!=Regular&Performance!~Slow&Redundant!=true'
        }
        'more' {
            # Extended test suite.
            # - Exclude slow groups
            # - Exclude redundant tests
            $filter = 'Performance!=SlowGroup&Redundant!=true'
        }
        'extra' {
            # Complement of the plan "more".
            $filter = 'Performance=SlowGroup|Redundant=true'
        }
        'most' {
            $filter = ''
        }
    }

    if ($filter) { $args += "--filter:$filter" }

    & dotnet test $TestProject $args
        || die 'Failed to run the test suite.'
}
finally {
    popd
}
