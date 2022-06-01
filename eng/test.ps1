# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet(
        'smoke', 'cover', 'regular', 'more', 'safe', 'most',
        'redundant-or-slow', 'redundant-and-slow', 'redundant-not-slow', 'slow-not-redundant')]
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

The common test plans are:
- "smoke"     = smoke testing
- "regular"   = exclude slow-running or redundant tests, and irrelevant tests
- "more"      = exclude slow-running groups of tests (individual tests are kept)
                and redundant tests
- "safe"      = exclude redundant tests
- "most"      = the whole test suite

The extra test plans are:
- "redundant-or-slow" =
    Only include slow-running groups of tests or redundant tests, that is the
    tests excluded from the test plan "more"
- "redundant-and-slow" =
    Only include redundant tests also part of a slow-running groups of tests;
    this is a subset of "redundant-or-slow"
- "redundant-not-slow" =
    Only include redundant tests not part of a slow-running groups of tests;
    this is a subset of "redundant-or-slow"
- "slow-not-redundant" =
    Only include non-redundant slow-running groups of tests;
    this is a subset of "redundant-or-slow"
- "cover" =
    Mimic the default test plan used by the code coverage tool
    The difference between "cover" and "regular" is really tiny. For a test to
    be in "regular" but not in "cover", it must be known to be slow and not
    being explicitely excluded from code coverage, right now there is none.

Of course, one can use "dotnet test" to run the whole test suite or to apply
custom filters.

Examples.
> test.ps1 -NoBuild             # Smoke testing (Debug)
> test.ps1 regular              # Regular test suite (Debug)
> test.ps1 regular -c Release   # Regular test suite (Release)

The common plans.
> test.ps1 smoke                # ~27 thousand tests (FAST)
> test.ps1 regular              # ~74 thousand tests
> test.ps1 more                 # ~82 thousand tests
> test.ps1 safe                 # ~84 thousand tests
> test.ps1 most                 # ~229 thousand tests (SLOW)

The extra plans.
> test.ps1 cover                # ~74 thousand tests
> test.ps1 slow-not-redundant   # ~3 thousand tests
> test.ps1 redundant-not-slow   # ~64 thousand tests
> test.ps1 redundant-and-slow   # ~81 thousand tests
> test.ps1 redundant-or-slow    # ~147 thousand tests (SLOW)

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
            # Smoke testing, exclude
            # - tests explicitely excluded from smoke testing
            # - slow units
            # - slow groups
            # - redundant tests (implicit)
            # - tests excluded from code coverage (implicit)
            # - tests excluded from the "regular" plan (implicit)
            #
            # If you change the filter, don't forget to update the github action.
            $filter = 'ExcludeFrom!=Smoke&Performance!~Slow'
        }
        'cover' {
            # Mimic the default test plan used by cover.ps1, exclude
            # - tests explicitely excluded from code coverage
            # - slow units
            # - slow groups
            # - redundant tests (implicit)
            # - tests excluded from the "regular" plan (implicit)
            $filter = 'ExcludeFrom!=CodeCoverage&Performance!~Slow'
        }
        'regular' {
            # Regular test suite, exclude
            # - tests explicitely excluded from the "regular" plan.
            # - slow units
            # - slow groups
            # - redundant tests (implicit)
            $filter = 'ExcludeFrom!=Regular&Performance!~Slow'
        }
        'more' {
            # Extended test suite, exclude
            # - slow groups
            # - redundant tests
            $filter = 'Performance!=SlowGroup&Redundant!=true'
        }
        'safe' {
            # Extended test suite, exclude
            # - redundant tests
            $filter = 'Redundant!=true'
        }
        'redundant-or-slow' {
            # Complement of the plan "more".
            # Only include slow groups and redundant tests.
            $filter = 'Performance=SlowGroup|Redundant=true'
        }
        # "redundant-or-slow" being pretty slow, we partition it into three subplans:
        # - "redundant-not-slow" = complement of {slow groups}
        # - "slow-not-redundant" = complement of {redundant tests}.
        # - "redundant-and-slow" = intersection of {redundant tests} and {slow groups}
        'redundant-and-slow' {
            $filter = 'Performance=SlowGroup&Redundant=true'
        }
        'redundant-not-slow' {
            $filter = 'Performance!=SlowGroup&Redundant=true'
        }
        'slow-not-redundant' {
            $filter = 'Performance=SlowGroup&Redundant!=true'
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
