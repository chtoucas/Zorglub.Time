// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing;

using Zorglub.Time.Core.Intervals;

public sealed class DomainTester
{
    public DomainTester(Range<DayNumber> domain)
    {
        // Un peu naïf mais on s'en contentera pour le moment.
        var (minDayNumber, maxDayNumber) = domain.Endpoints;
        ValidDayNumbers = new List<DayNumber>
        {
            minDayNumber,
            minDayNumber + 1,
            maxDayNumber - 1,
            maxDayNumber,
        };
        InvalidDayNumbers = new List<DayNumber>
        {
            DayNumber.MinValue,
            minDayNumber - 1,
            maxDayNumber + 1,
            DayNumber.MaxValue,
        };
    }

    public IEnumerable<DayNumber> ValidDayNumbers { get; }
    public IEnumerable<DayNumber> InvalidDayNumbers { get; }

    public void TestInvalidDayNumber(Action<DayNumber> fun, string argName = "dayNumber")
    {
        foreach (var dayNumber in InvalidDayNumbers)
        {
            Assert.ThrowsAoorexn(argName, () => fun.Invoke(dayNumber));
        }
    }

    public void TestInvalidDayNumber<T>(Func<DayNumber, T> fun, string argName = "dayNumber")
    {
        foreach (var dayNumber in InvalidDayNumbers)
        {
            Assert.ThrowsAoorexn(argName, () => fun.Invoke(dayNumber));
        }
    }
}
