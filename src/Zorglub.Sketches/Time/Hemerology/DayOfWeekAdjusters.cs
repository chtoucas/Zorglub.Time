﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

using System.Numerics;

public static class DayOfWeekAdjusters
{
    [Pure]
    public static T Previous<T>(T self, DayOfWeek dayOfWeek)
        where T : IFixedDate, IAdditionOperators<T, int, T>
    {
        Requires.NotNull(self);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - self.DayOfWeek;
        return self + (δ >= 0 ? δ - 7 : δ);
    }

    [Pure]
    public static T PreviousOrSame<T>(T self, DayOfWeek dayOfWeek)
        where T : IFixedDate, IAdditionOperators<T, int, T>
    {
        Requires.NotNull(self);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - self.DayOfWeek;
        return δ == 0 ? self : self + (δ > 0 ? δ - 7 : δ);
    }

    //[Pure]
    //public static T Nearest<T>(T self, DayOfWeek dayOfWeek)
    //    where T : IFixedDate, IAdditionOperators<T, int, T>
    //{
    //    Requires.NotNull(self);
    //    Requires.Defined(dayOfWeek);

    //    DayNumber nearest = self.DayNumber.Nearest(dayOfWeek);
    //    return T.FromDayNumber(nearest);
    //}

    [Pure]
    public static T NextOrSame<T>(T self, DayOfWeek dayOfWeek)
        where T : IFixedDate, IAdditionOperators<T, int, T>
    {
        Requires.NotNull(self);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - self.DayOfWeek;
        return δ == 0 ? self : self + (δ < 0 ? δ + 7 : δ);
    }

    [Pure]
    public static T Next<T>(T self, DayOfWeek dayOfWeek)
        where T : IFixedDate, IAdditionOperators<T, int, T>
    {
        Requires.NotNull(self);
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - self.DayOfWeek;
        return self + (δ <= 0 ? δ + 7 : δ);
    }
}
