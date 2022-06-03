# See LICENSE in the project root for license information.

#Requires -Version 7

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet(
        'smoke', 'cover', 'regular', 'more', 'most',
        'redundant', 'redundant-slow', 'redundant-not-slow')]
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
  -c|-Configuration  configuration to test the solution for. Default = "Debug"
    |-NoBuild        do NOT build the test suite?
  -h|-Help           print this help then exit

The default behaviour is to run the smoke tests using the configuration Debug.

Test plans
----------
- "smoke"     = smoke testing
- "regular"   = exclude redundant tests, slow-running test units or bundles
- "more"      = exclude redundant tests
- "most"      = the whole test suite

The extra test plans are
- "redundant" = redundant tests, this is the complement of the plan "more".
- "redundant-slow" = redundant tests part of a slow-running test bundle.
- "redundant-not-slow" = redundant tests not part of a slow-running test bundle.

We have also a plan named "cover". It mimics the default test plan used by the
code coverage tool. The difference between "cover" and "regular" is really tiny.
For a test to be in "regular" but not in "cover", it must be known to be slow
and not being explicitely excluded from code coverage, right now there is none.

Of course, one can use "dotnet test" to run the whole test suite or to apply
custom filters.

Examples
--------
> test.ps1 -NoBuild             # Smoke testing (Debug)
> test.ps1 regular -c Release   # Regular test suite (Release)
> test.ps1 more                 # Comprehensive test suite (Debug)

Figures.
> test.ps1 smoke                # ~26 thousand tests (FAST)
> test.ps1 regular              # ~71 thousand tests
> test.ps1 more                 # ~82 thousand tests
> test.ps1 most                 # ~234 thousand tests (SLOW)
Extra plans.
> test.ps1 redundant-slow       # ~64 thousand tests
> test.ps1 redundant-not-slow   # ~93 thousand tests
> test.ps1 redundant            # ~158 thousand tests (SLOW)

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
            # - tests explicitely excluded from this plan
            # - slow test units
            # - slow test bundles
            # - redundant tests (implicit)
            # - tests excluded from code coverage (implicit)
            # - tests excluded from the "regular" plan (implicit)
            #
            # If you change the filter, don't forget to update the github action.
            $filter = 'ExcludeFrom!=Smoke&Performance!~Slow'
        }
        'cover' {
            # Mimic the default test plan used by cover.ps1, exclude
            # - tests explicitely excluded from this plan
            # - slow test units
            # - slow test bundles
            # - redundant tests (implicit)
            # - tests excluded from the "regular" plan (implicit)
            $filter = 'ExcludeFrom!=CodeCoverage&Performance!~Slow'
        }
        'regular' {
            # Regular test suite, exclude
            # - tests explicitely excluded from this plan
            # - slow test units
            # - slow test bundles
            # - redundant tests (implicit)
            $filter = 'ExcludeFrom!=Regular&Performance!~Slow'
        }
        'more' {
            # Only exclude redundant tests.
            $filter = 'Redundant!=true'
        }
        'redundant' {
            # Only include redundant tests.
            $filter = 'Redundant=true'
        }
        # "redundant" being pretty slow, we partition it into two subplans.
        'redundant-slow' {
            $filter = 'Redundant=true&Performance=SlowBundle'
        }
        'redundant-not-slow' {
            $filter = 'Redundant=true&Performance!=SlowBundle'
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
