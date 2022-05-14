# See LICENSE in the project root for license information.

#Requires -Version 7

################################################################################
#region Preamble.

<#
.SYNOPSIS
Test script.

.DESCRIPTION
Run the test suite.

.PARAMETER Plan
Specify the test plan. Default = "smoke".

.PARAMETER Configuration
The configuration to test the solution for. Default = "Debug".

.PARAMETER Build
Build the test project.
The default behaviour is not to build the project.

.PARAMETER Help
Print help text then exit?
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('smoke', 'regular', 'more', 'extra')]
                 [string] $Plan = 'smoke',

    [Parameter(Mandatory = $false)]
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

                 [switch] $Build,

    [Alias("h")] [switch] $Help
)

. (Join-Path $PSScriptRoot 'zorglub.ps1')

#endregion
################################################################################
#region Helpers.

function Print-Help {
    say @"

Test script.

Usage: test.ps1 [arguments]
    |-Plan
  -c|-Configuration  the configuration to test the solution for. Default = "Debug"
    |-Build          build the project before running the test suite
  -h|-Help           print this help then exit

Examples.
> test.ps1                      # Smoke testing
> test.ps1 smoke                # Idem
> test.ps1 smoke -Build         # Idem but build the project before
> test.ps1 smoke -c Release     # Idem but use the Release configuration
> test.ps1 regular              # Run the regular test suite; test plan used by the code coverage tool
> test.ps1 more                 # Run the regular test suite + a few more tests
> test.ps1 extra                # (Very Slow) The complement of 'more', redundant or very slow tests

Of course, just use dotnet to run the whole test suite.

Looking for more help?
> Get-Help -Detailed test.ps1

"@
}

#endregion
################################################################################

if ($Help) { Print-Help ; exit }

try {
    pushd $ROOT_DIR

    $args = @("-c:$configuration")
    if (-not $Build) { $args += "--no-build" }

    switch ($Plan) {
        'smoke' {
            # This is also the test plan used by the GitHub CI action.
            # Excluded:
            # - A bunch of tests in Postludes (slow unit)
            # - ArchetypalSchemaTestSuite (slow group)
            # - PrototypalSchemaTestSuite (slow group)
            # - Redundant tests
            # We only keep one test class per test suite (no smoke)
            $args += "--filter:ExcludeFrom!=Smoke&Performance!~Slow&Redundant!=true"
        }
        'regular' {
            # Regular test suite. It mimics the test plan used by the code coverage tool.
            # Excluded:
            # - A bunch of tests in Postludes (no code coverage)
            # - ArchetypalSchemaTestSuite (no code coverage OR redundant)
            # - PrototypalSchemaTestSuite (no code coverage OR redundant)
            # - Redundant tests
            $args += "--filter:ExcludeFrom!=CodeCoverage&Redundant!=true"
        }
        'more' {
            # Excluded:
            # - ArchetypalSchemaTestSuite (slow group)
            # - PrototypalSchemaTestSuite (slow group)
            # - Redundant tests
            $args += "--filter:Performance!=SlowGroup&Redundant!=true"
        }
        'extra' {
            $args += "--filter:Redundant=true|Performance=SlowGroup"
        }
    }

    & dotnet test $TEST_PROJECT $args `
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
