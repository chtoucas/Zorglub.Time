// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using System.Linq;
using System.Reflection;

public static class CalendricalSchemaTests
{
    private static readonly IEnumerable<TypeInfo> s_DefinedTypes =
        typeof(ICalendricalSchema).Assembly.DefinedTypes;

    [Fact]
    public static void Constructor_IsNotPublic()
    {
        // We also test the abstract classes.
        var schemaTypes = s_DefinedTypes
            .Where(t => t.IsSubclassOf(typeof(SystemSchema)));

        Assert.NotEmpty(schemaTypes);

        foreach (var type in schemaTypes)
        {
            var publicCtors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            Assert.Empty(publicCtors);
        }
    }

    [Fact]
    public static void GetInstance()
    {
        var schemaTypes = s_DefinedTypes
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(SystemSchema)));

        Assert.NotEmpty(schemaTypes);

        var methodName = "GetInstance";

        foreach (var type in schemaTypes)
        {
            var getInstance = type.GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Static,
                null,
                Type.EmptyTypes,
                null);

            if (getInstance is null)
            {
                Assert.Fails($"Method {methodName} not found for {type}.");
                continue;
            }

            // GetInstance() does NOT return a singleton instance.
            var inst1 = getInstance.Invoke(null, null);
            var inst2 = getInstance.Invoke(null, null);
            Assert.NotSame(inst1, inst2);
        }
    }

    [Fact]
    public static void MonthsInYear()
    {
        var schemaTypes = s_DefinedTypes
            .Where(t => !t.IsAbstract && typeof(IRegularSchema).IsAssignableFrom(t));

        Assert.NotEmpty(schemaTypes);

        foreach (var type in schemaTypes)
        {
            var emptyCtor = type.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            if (emptyCtor is null)
            {
                Assert.Fails($"Private parameterless ctor not found for {type}.");
                continue;
            }

            var sch = (IRegularSchema)emptyCtor.Invoke(null);
            // Act & Assert
            Assert.True(sch.MonthsInYear > 0);
        }
    }
}
