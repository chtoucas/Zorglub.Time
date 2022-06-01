// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing;

using Xunit.Abstractions;
using Xunit.Sdk;

#region Developer Notes

// Traits:
// - RedundantTest          => TestExcludeFrom=Smoke and CodeCoverage
// - RedundantTestGroup     => TestExcludeFrom=Smoke and CodeCoverage
// - TestPerfomance
// - TestExcludeFrom
//   - CodeCoverage => TestExcludeFrom=Smoke
//   - Regular      => TestExcludeFrom=Smoke and CodeCoverage
//
// Used by eng\test.ps1, eng\cover.ps1 and the github action.
// See https://github.com/xunit/samples.xunit/blob/main/TraitExtensibility/

#endregion

internal static class XunitTraitAssembly
{
    public const string Name = "Zorglub.Testing";
    public const string TypePrefix = Name + ".";
}

// Be careful if you change the values, the scripts rely on them.
internal static class XunitTraits
{
    // All traits have a single property: we use the same string for both name
    // and value of a trait.
    public const string ExcludeFrom = "ExcludeFrom";
    public const string Performance = "Performance";
    public const string Redundant = "Redundant";
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
    /// <summary>
    /// Exclude from smoke testing.
    /// <para>We use this value to exclude all classes in a test suite but the first one.</para>
    /// </summary>
    Smoke,

    /// <summary>
    /// Exclude from code coverage.
    /// <para>For instance, we exclude deeply recursive functions.</para>
    /// <para>A test marked with this value will also be excluded from smoke testing.</para>
    /// </summary>
    CodeCoverage,

    /// <summary>
    /// Exclude from the "regular" test plan.
    /// <para>We use this value to exclude tests of very low importance and not
    /// needed to achieve full code coverage.</para>
    /// <para>This value only exists to reduce the time needed to complete the
    /// "regular" test.</para>
    /// <para>A test marked with this value will also be excluded from smoke testing and code
    /// coverage.</para>
    /// </summary>
    Regular
}

// We use this trait to exclude redundant individual tests.
[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(RedundantTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RedundantTestAttribute : Attribute, ITraitAttribute
{
    public RedundantTestAttribute() { }
}

// We use this trait to exclude redundant groups of tests, apply to classes in a
// test suite. This is very similar to TestExcludeFrom.Smoke, except that
// we keep all test classes necessary for full code coverage.
// IMPORTANT: We automatically exclude the group from smoke testing.
[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(RedundantTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class RedundantTestGroupAttribute : Attribute, ITraitAttribute
{
    public RedundantTestGroupAttribute() { }
}

[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(ExcludeFromTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class TestExcludeFromAttribute : Attribute, ITraitAttribute
{
    public TestExcludeFromAttribute(TestExcludeFrom excludeFrom) { ExcludeFrom = excludeFrom; }

    public TestExcludeFrom ExcludeFrom { get; }
}

[TraitDiscoverer(XunitTraitAssembly.TypePrefix + nameof(PerformanceTraitDiscoverer), XunitTraitAssembly.Name)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class TestPerformanceAttribute : Attribute, ITraitAttribute
{
    public TestPerformanceAttribute(TestPerformance performance) { Performance = performance; }

    public TestPerformance Performance { get; }
}

#region Discoverers

// TODO(code): the trait discoverers do not inspect the base classes.

public sealed class ExcludeFromTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        Requires.NotNull(traitAttribute);

        var value = traitAttribute.GetNamedArgument<TestExcludeFrom>(XunitTraits.ExcludeFrom);

        if (value == TestExcludeFrom.CodeCoverage)
        {
            // We automatically exclude the test(s) from smoke testing and code coverage.
            yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, TestExcludeFrom.Smoke.ToString());
        }
        if (value == TestExcludeFrom.Regular)
        {
            // We automatically exclude the test(s) from smoke testing and code coverage.
            yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, TestExcludeFrom.Smoke.ToString());
            yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, TestExcludeFrom.CodeCoverage.ToString());
        }

        yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, value.ToString());
    }
}

public sealed class PerformanceTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        Requires.NotNull(traitAttribute);

        var value = traitAttribute.GetNamedArgument<TestPerformance>(XunitTraits.Performance);
        yield return new KeyValuePair<string, string>(XunitTraits.Performance, value.ToString());
    }
}

public sealed class RedundantTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        Requires.NotNull(traitAttribute);

        yield return new KeyValuePair<string, string>(XunitTraits.Redundant, "true");
        // We automatically exclude the test(s) from smoke testing and code coverage.
        yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, TestExcludeFrom.Smoke.ToString());
        yield return new KeyValuePair<string, string>(XunitTraits.ExcludeFrom, TestExcludeFrom.CodeCoverage.ToString());
    }
}

#endregion
