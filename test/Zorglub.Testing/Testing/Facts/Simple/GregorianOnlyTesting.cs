// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts.Simple;

using Zorglub.Testing.Data;
using Zorglub.Testing.Data.Bounded;
using Zorglub.Time.Simple;

// TODO(fact): to be removed.

/// <summary>
/// Provides a base for Gregorian-only tests.
/// </summary>
public abstract class GregorianOnlyTesting : CalendarDataConsumer<ProlepticGregorianDataSet>
{
    protected GregorianOnlyTesting(GregorianCalendar calendar)
    {
        CalendarUT = calendar;

        SupportedYearsTester = new SupportedYearsTester(calendar.SupportedYears);
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected GregorianCalendar CalendarUT { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }
}
