// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing;

using Zorglub.Testing.Data;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Hemerology;

/// <summary>
/// Provides a base for calendar testing.
/// </summary>
public abstract class CalendarTesting<TDataSet> : CalendarDataConsumer<TDataSet>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarTesting(CtorArgs args!!) : this(args.SupportedYears, args.Domain) { }

    private CalendarTesting(Range<int> supportedYears, Range<DayNumber> domain)
    {
        Domain = domain;

        SupportedYearsTester = new SupportedYearsTester(supportedYears);
        DomainTester = new DomainTester(domain);
    }

    protected Range<DayNumber> Domain { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }
    protected DomainTester DomainTester { get; }

    [Pure]
    protected static CtorArgs CreateCtorArgs(ICalendar calendar!!) =>
        new(calendar.SupportedYears, calendar.Domain);

    protected sealed record CtorArgs(Range<int> SupportedYears, Range<DayNumber> Domain);
}
