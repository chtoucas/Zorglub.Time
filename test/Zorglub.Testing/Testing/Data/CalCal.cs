// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data;

public static class CalCal
{
    /// <summary>
    /// Converts a collection of (RataDie, Year, Month, Day) o a set of data of type
    /// <see cref="DaysSinceEpochInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryData<DaysSinceEpochInfo> ConvertRataDieToDaysSinceEpochInfo(
        IEnumerable<(int RataDie, int Year, int Month, int Day)> rdSource, DayNumber epoch)
    {
        Requires.NotNull(rdSource);

        int shift = DayZero.RataDie - epoch;
        var data = new TheoryData<DaysSinceEpochInfo>();
        foreach (var (rd, y, m, d) in rdSource)
        {
            data.Add(new DaysSinceEpochInfo(rd + shift, y, m, d));
        }
        return data;
    }

    /// <summary>
    /// Converts a collection of (RataDie, Year, Month, Day) o a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryData<DayNumberInfo> ConvertRataDieToDayNumberInfo(
        IEnumerable<(int RataDie, int Year, int Month, int Day)> rdSource)
    {
        Requires.NotNull(rdSource);

        DayNumber rdEpoch = DayZero.RataDie; // cache prop locally
        var data = new TheoryData<DayNumberInfo>();
        foreach (var (rd, y, m, d) in rdSource)
        {
            data.Add(new DayNumberInfo(rdEpoch + rd, y, m, d));
        }
        return data;
    }

    /// <summary>
    /// Converts a collection of (RataDie, Year, Month, Day) o a set of data of type
    /// <see cref="DayNumberInfo"/>.
    /// </summary>
    [Pure]
    public static TheoryData<DayNumberInfo> ConvertRataDieToDayNumberInfo(
        IEnumerable<(int RataDie, int Year, int Month, int Day)> rdSource, DayNumber epoch, DayNumber newEpoch)
    {
        Requires.NotNull(rdSource);

        DayNumber zero = DayZero.RataDie + (newEpoch - epoch);
        var data = new TheoryData<DayNumberInfo>();
        foreach (var (rd, y, m, d) in rdSource)
        {
            data.Add(new DayNumberInfo(zero + rd, y, m, d));
        }
        return data;
    }
}
