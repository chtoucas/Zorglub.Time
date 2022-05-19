// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Extensions;

using System.Linq;

// TODO(data): to be removed.

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class TheoryDataExtensions
{
    [Pure]
    public static TheoryData<Yemoda, Yemoda, T1, T2, T3> ToTheoryData<T1, T2, T3>(
        this IEnumerable<(int, int, int, int, int, int, T1, T2, T3)> @this)
    {
        Requires.NotNull(@this);

        var data = new TheoryData<Yemoda, Yemoda, T1, T2, T3>();
        foreach (var (y1, m1, d1, y2, m2, d2, t1, t2, t3) in @this)
        {
            data.Add(new Yemoda(y1, m1, d1), new Yemoda(y2, m2, d2), t1, t2, t3);
        }
        return data;
    }

    [Pure]
    public static TheoryData<DayNumber, int, int, DayOfWeek> ToTheoryData(
        this IEnumerable<(int Ord, int Year, int WeekOfYear, DayOfWeek DayOfWeek)> @this)
    {
        Requires.NotNull(@this);

        var data = new TheoryData<DayNumber, int, int, DayOfWeek>();
        foreach (var (ord, y, woy, dayOfWeek) in @this)
        {
            data.Add(DayZero.NewStyle + (ord - 1), y, woy, dayOfWeek);
        }
        return data;
    }
}
