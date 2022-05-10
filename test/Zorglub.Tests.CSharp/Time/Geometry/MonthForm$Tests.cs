// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Forms;

using static Zorglub.Bulgroz.GJConstants;

// We use the GJ forms to test the extension methods.

public static class MonthFormExtensionsTests
{
    [Fact]
    public static void WithAlgebraicNumbering()
    {
        var form = new MonthForm(153, 5, 2);
        var ordForm = new MonthForm(153, 5, -151, MonthFormNumbering.Ordinal);
        var troeschForm = new TroeschMonthForm(153, 5, -457, ExceptionalMonth);

        // Act & Assert
        Assert.Equal(form, form.WithAlgebraicNumbering());
        Assert.Equal(form, ordForm.WithAlgebraicNumbering());
        Assert.Equal(form, troeschForm.WithAlgebraicNumbering());
    }

    [Fact]
    public static void WithOrdinalNumbering()
    {
        var form = new MonthForm(153, 5, 2);
        var ordForm = new MonthForm(153, 5, -151, MonthFormNumbering.Ordinal);
        var troeschForm = new TroeschMonthForm(153, 5, -457, ExceptionalMonth);

        // Act & Assert
        Assert.Equal(ordForm, form.WithOrdinalNumbering());
        Assert.Equal(ordForm, ordForm.WithOrdinalNumbering());
        Assert.Equal(ordForm, troeschForm.WithOrdinalNumbering());
    }

    [Fact]
    public static void WithTroeschNumbering()
    {
        var form = new MonthForm(153, 5, 2);
        var ordForm = new MonthForm(153, 5, -151, MonthFormNumbering.Ordinal);
        var troeschForm = new TroeschMonthForm(153, 5, -457, ExceptionalMonth);

        // Act & Assert
        Assert.Equal(troeschForm, form.WithTroeschNumbering(ExceptionalMonth));
        Assert.Equal(troeschForm, ordForm.WithTroeschNumbering(ExceptionalMonth));
        Assert.Equal(troeschForm, troeschForm.WithTroeschNumbering(ExceptionalMonth));
    }
}
