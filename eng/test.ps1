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
    |-Plan           specify the test plan. Default = "smoke".
  -c|-Configuration  the configuration to test the solution for. Default = "Debug"
    |-Build          build the project before running the test suite
  -h|-Help           print this help then exit

The default behaviour is to NOT build the project.

Examples.
> test.ps1                      # Smoke testing
> test.ps1 smoke                # Idem
> test.ps1 smoke -Build         # Idem but build the project before
> test.ps1 smoke -c Release     # Idem but use the Release configuration
> test.ps1 regular              # Run the regular test suite; test plan used by the code coverage tool
> test.ps1 more                 # Run the regular test suite + a few more tests
> test.ps1 extra                # (Very Slow) The complement of 'more', redundant or very slow tests

Of course, just use dotnet to run the whole test suite.

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
            # This is also the test plan used by the GitHub CI action.
            # Excluded:
            # - A bunch of tests in Postludes (slow unit)
            # - ArchetypalSchemaTestSuite (slow group)
            # - PrototypalSchemaTestSuite (slow group)
            # - Redundant tests
            # We only keep one test class per test suite (no smoke)
            # Filters = ExcludeFrom!=Smoke&Performance!~Slow&Redundant!=true
            $args += "--filter:$SmokeTestsFilters"
        }
        'regular' {
            # Regular test suite. It mimics the test plan used by the code coverage tool.
            # Excluded:
            # - A bunch of tests in Postludes (no code coverage)
            # - ArchetypalSchemaTestSuite (no code coverage OR redundant)
            # - PrototypalSchemaTestSuite (no code coverage OR redundant)
            # - Redundant tests
            # Filters = ExcludeFrom!=CodeCoverage&Redundant!=true
            $args += "--filter:$RegularTestsFilters"
        }
        'more' {
            # Excluded:
            # - ArchetypalSchemaTestSuite (slow group)
            # - PrototypalSchemaTestSuite (slow group)
            # - Redundant tests
            $args += '--filter:Performance!=SlowGroup&Redundant!=true'
        }
        'extra' {
            $args += '--filter:Redundant=true|Performance=SlowGroup'
        }
    }

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
