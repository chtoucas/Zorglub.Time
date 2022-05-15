// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

public static class CalCal
{
    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceRataDieInfo"/> to a set of data of type
    /// <see cref="DaysSinceEpochInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryData<DaysSinceEpochInfo> MapToDaysSinceEpochInfo(
        IEnumerable<DaysSinceRataDieInfo> source, DayNumber epoch)
    {
        Requires.NotNull(source);

        int shift = DayZero.RataDie - epoch;
        var data = new TheoryData<DaysSinceEpochInfo>();
        foreach (var (daysSinceRataDie, y, m, d) in source)
        {
            data.Add(new DaysSinceEpochInfo(daysSinceRataDie + shift, y, m, d));
        }
        return data;
    }

    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceRataDieInfo"/> to a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryData<DayNumberInfo> MapToDayNumberInfo(
        IEnumerable<DaysSinceRataDieInfo> source)
    {
        Requires.NotNull(source);

        DayNumber rd = DayZero.RataDie; // cache prop locally
        var data = new TheoryData<DayNumberInfo>();
        foreach (var (daysSinceRataDie, y, m, d) in source)
        {
            data.Add(new DayNumberInfo(rd + daysSinceRataDie, y, m, d));
        }
        return data;
    }

    /// <summary>
    /// Converts a sequence of <see cref="DaysSinceRataDieInfo"/> to a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryData<DayNumberInfo> MapToDayNumberInfo(
        IEnumerable<DaysSinceRataDieInfo> source, DayNumber epoch, DayNumber newEpoch)
    {
        Requires.NotNull(source);

        DayNumber zero = DayZero.RataDie + (newEpoch - epoch);
        var data = new TheoryData<DayNumberInfo>();
        foreach (var (daysSinceRataDie, y, m, d) in source)
        {
            data.Add(new DayNumberInfo(zero + daysSinceRataDie, y, m, d));
        }
        return data;
    }
}
