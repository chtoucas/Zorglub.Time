// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

public sealed partial class OrdinalDateAdjustersTests : GregorianOnlyTesting
{
    public OrdinalDateAdjustersTests() : base(GregorianCalendar.Instance) { }
}

public partial class OrdinalDateAdjustersTests // Adjustments
{
    #region WithYear()

    [Fact]
    public void WithYear_InvalidYear()
    {
        var ordate = CalendarUT.GetOrdinalDate(3, 45);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(y => ordate.WithYear(y), "newYear");
    }

    [Fact]
    public void WithYear_InvalidResult()
    {
        var ordate = CalendarUT.GetOrdinalDate(4, 366);
        // Act & Assert
        Assert.ThrowsAoorexn("newYear", () => ordate.WithYear(3));
    }

    [Fact]
    public void WithYear()
    {
        var ordate = CalendarUT.GetOrdinalDate(3, 45);
        var dateE = CalendarUT.GetOrdinalDate(4, 45);
        // Act & Assert
        Assert.Equal(dateE, ordate.WithYear(4));
    }

    #endregion
    #region WithDayOfYear()

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void WithDayOfYear_InvalidDayOfYear(int y, int doy)
    {
        var ordate = CalendarUT.GetOrdinalDate(y, 1);
        // Act & Assert
        Assert.ThrowsAoorexn("newDayOfYear", () => ordate.WithDayOfYear(doy));
    }

    [Fact]
    public void WithDayOfYear()
    {
        var ordate = CalendarUT.GetOrdinalDate(3, 45);
        var dateE = CalendarUT.GetOrdinalDate(3, 54);
        // Act & Assert
        Assert.Equal(dateE, ordate.WithDayOfYear(54));
    }

    #endregion
}
