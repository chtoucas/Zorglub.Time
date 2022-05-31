// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using System.Linq;
using System.Reflection;

public static class CalendricalArithmeticTests
{
    [Fact]
    public static void Constructor_IsNotPublic()
    {
        // On teste aussi les classes abstraites.
        var addTypes = typeof(Ord).Assembly.DefinedTypes
            .Where(t => t.IsPublic && typeof(ICalendricalArithmetic).IsAssignableFrom(t));

        Assert.NotEmpty(addTypes);

        foreach (var type in addTypes)
        {
            var publicCtors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            Assert.Empty(publicCtors);
        }
    }
}
