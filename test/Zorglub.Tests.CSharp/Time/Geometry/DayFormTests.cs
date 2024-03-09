// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry;

using Zorglub.Time.Geometry.Forms;

public static class DayFormTests
{
    [Fact]
    public static void Reverse()
    {
        var form = new DayForm();
        // Act
        Assert.Equal(new CalendricalForm(1, 1, 1), form.Reverse());
    }

    [Fact]
    public static void OtherMethods()
    {
        var form = new DayForm();
        // Act & Assert
        for (int d = 1; d < 100; d++)
        {
            int d0 = d - 1;
            Assert.Equal(d0, DayForm.CountDaysInMonthBeforeDay(d));
            Assert.Equal(d0, form.CountDaysInMonthBeforeDayCore(d));
            Assert.Equal(d, DayForm.GetDay(d0));
            Assert.Equal(d, form.GetDayCore(d0));
        }
    }
}
