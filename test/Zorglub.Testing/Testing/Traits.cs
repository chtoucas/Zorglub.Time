// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing;

using Xunit.Abstractions;
using Xunit.Sdk;

#region Developer Notes

// Traits:
// - RedundantTest
// - TestPerfomance
// - TestExcludeFrom
//
// Profiles
// --------
// Smoke and GitHub action
//   "ExcludeFrom!=Smoke&Performance!~Slow&Redundant!=true"
//   Excluded:
//   - A bunch of tests in Postludes (slow unit)
//   - ArchetypalSchemaTestSuite (slow group)
//   - PrototypalSchemaTestSuite (slow group)
//   - Redundant tests
//   We only keep one test class per test suite (no smoke)
//
// Test
//   "Performance!=SlowGroup&Redundant!=true"
//   Excluded:
//   - ArchetypalSchemaTestSuite (slow group)
//   - PrototypalSchemaTestSuite (slow group)
//   - Redundant tests
//
// Code Coverage
//   "ExcludeFrom!=CodeCoverage&Redundant!=true"
//   Excluded:
//   - A bunch of tests in Postludes (no code coverage)
//   - ArchetypalSchemaTestSuite (no code coverage OR redundant)
//   - PrototypalSchemaTestSuite (no code coverage OR redundant)
//   - Redundant tests
//
// See https://github.com/xunit/samples.xunit/blob/main/TraitExtensibility/

#endregion

internal static class XunitTraitAssembly
{
    public const string Name = "Zorglub.Testing";
    public const string TypePrefix = Name + ".";
}

// Be careful if you change the values, the scripts rely on them.
internal static class XunitTrait
{
    // All traits have a single property: we use the same string for both name
    // and value of a trait.
    public const string Redundant = "Redundant";
    public const string Performance = "Performance";
    public const string ExcludeFrom = "ExcludeFrom";
}

// Be careful if you change the values, the scripts rely on the fact that
// the name contains the string "Slow" to filter out the slow tests.
public enum TestPerformance
{
    /// <summary>
    /// A single slow test.
    /// </summary>
    SlowUnit,

    /// <summary>
    /// A group of slow tests, typically a test class.
    /// </summary>
    SlowGroup
}

public enum TestExcludeFrom
{
    // We use this to exclude all classes in a test suite but the first one.
    Smoke,

    // For instance, we exclude deeply recursive functions.
    CodeCoverage
}

// We use this trait to exclude redundant tests, mostly apply to classes in a
// test suite. This is very similar to TestExcludeFrom.Smoke, except that we
// keep more than one test class.
[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(RedundantTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RedundantTestingAttribute : Attribute, ITraitAttribute
{
    public RedundantTestingAttribute() { }
}

[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(PerformanceTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class TestPerformanceAttribute : Attribute, ITraitAttribute
{
    public TestPerformanceAttribute(TestPerformance performance) { Performance = performance; }

    public TestPerformance Performance { get; }
}

[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(ExcludeFromTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class TestExcludeFromAttribute : Attribute, ITraitAttribute
{
    public TestExcludeFromAttribute(TestExcludeFrom excludeFrom) { ExcludeFrom = excludeFrom; }

    public TestExcludeFrom ExcludeFrom { get; }
}

#region Discovers

// FIXME(code): the trait discoverers do not inspect the base classes.

public class RedundantTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute!!)
    {
        yield return new KeyValuePair<string, string>(XunitTrait.Redundant, "true");
    }
}

public class PerformanceTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute!!)
    {
        var value = traitAttribute.GetNamedArgument<TestPerformance>(XunitTrait.Performance);
        yield return new KeyValuePair<string, string>(XunitTrait.Performance, value.ToString());
    }
}

public class ExcludeFromTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute!!)
    {
        var value = traitAttribute.GetNamedArgument<TestExcludeFrom>(XunitTrait.ExcludeFrom);
        yield return new KeyValuePair<string, string>(XunitTrait.ExcludeFrom, value.ToString());
    }
}

#endregion
