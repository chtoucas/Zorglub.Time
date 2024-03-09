// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Testing;

using Zorglub.Time.Core.Intervals;

public sealed class SupportedYearsTester
{
    public SupportedYearsTester(Range<int> supportedYears)
    {
        // Un peu naïf mais pour le moment on s'en contentera pour le moment.
        var (minYear, maxYear) = supportedYears.Endpoints;
        ValidYears = new List<int>
        {
            minYear,
            minYear + 1,
            maxYear - 1,
            maxYear,
        };
        InvalidYears = new List<int>
        {
            Int32.MinValue,
            minYear - 1,
            maxYear + 1,
            Int32.MaxValue,
        };
    }

    public IEnumerable<int> ValidYears { get; }
    public IEnumerable<int> InvalidYears { get; }

    public void TestInvalidYear(Action<int> fun, string argName = "year")
    {
        foreach (int y in InvalidYears)
        {
            Assert.ThrowsAoorexn(argName, () => fun.Invoke(y));
        }
    }

    public void TestInvalidYear<T>(Func<int, T> fun, string argName = "year")
    {
        foreach (int y in InvalidYears)
        {
            Assert.ThrowsAoorexn(argName, () => fun.Invoke(y));
        }
    }
}
