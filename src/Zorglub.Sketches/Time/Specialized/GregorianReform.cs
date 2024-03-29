﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Specialized;

public sealed record GregorianReform
{
    public static readonly GregorianReform Official = new();

    private GregorianReform()
        : this(
            new JulianDate(1582, 10, 4),
            new GregorianDate(1582, 10, 15),
            null)
    { }

    private GregorianReform(
        JulianDate lastJulianDate,
        GregorianDate firstGregorianDate,
        DayNumber? switchover)
    {
        LastJulianDate = lastJulianDate;
        FirstGregorianDate = firstGregorianDate;
        Switchover = switchover ?? FirstGregorianDate.DayNumber;
        SecularShift = InitSecularShift();
    }

    public JulianDate LastJulianDate { get; }
    public GregorianDate FirstGregorianDate { get; }
    public DayNumber Switchover { get; }
    public int SecularShift { get; }

    [Pure]
    public static GregorianReform FromLastJulianDate(JulianDate date)
    {
        if (date < Official.LastJulianDate) Throw.ArgumentOutOfRange(nameof(date));

        var switchover = date.DayNumber + 1;
        var firstGregorianDate = GregorianDate.FromDayNumber(switchover);

        return new GregorianReform(date, firstGregorianDate, switchover);
    }

    [Pure]
    public static GregorianReform FromFirstGregorianDate(GregorianDate date)
    {
        if (date < Official.FirstGregorianDate) Throw.ArgumentOutOfRange(nameof(date));

        var switchover = date.DayNumber;
        var lastJulianDate = JulianDate.FromDayNumber(switchover - 1);

        return new GregorianReform(lastJulianDate, date, switchover);
    }

    [Pure]
    private int InitSecularShift()
    {
        var (y, m, d) = FirstGregorianDate;
        var dayNumber = new JulianDate(y, m, d).DayNumber;
        return dayNumber - Switchover;
    }
}
