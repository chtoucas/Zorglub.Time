// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing;

using Xunit.Abstractions;
using Xunit.Sdk;

// See https://github.com/xunit/samples.xunit/blob/main/TraitExtensibility/

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
    public const string Performance = "Performance";
    public const string ExcludeFrom = "ExcludeFrom";
}

// Be careful if you change the values, the scripts rely on the fact that
// the name contains the string "Slow" to filter out the slow tests.
public enum TestPerformance
{
    // Usually applied to a single test, or to a test class, that is a set of
    // tests that run together are quite slow.
    Slow,
    // Usually applied to single tests that take more than 1s to run.
    VerySlow
}

public enum TestExcludeFrom
{
    // We mostly use this trait to exclude all members of a test suite but the first one.
    Smoke,
    CodeCoverage
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
