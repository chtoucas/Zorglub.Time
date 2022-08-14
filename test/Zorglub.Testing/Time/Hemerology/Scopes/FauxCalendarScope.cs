// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes;

using Zorglub.Time.Core.Intervals;

public sealed class FauxCalendarScope : CalendarScope
{
    public FauxCalendarScope(ICalendricalSchema schema, int minYear, int maxYear)
        : this(schema, default, minYear, maxYear) { }

    public FauxCalendarScope(ICalendricalSchema schema, DayNumber epoch, int minYear, int maxYear)
        : base(epoch, CalendricalSegment.Create(schema, Range.Create(minYear, maxYear))) { }

    public FauxCalendarScope(DayNumber epoch, CalendricalSegment segment)
        : base(epoch, segment) { }

    // Constructor to be able to test the base constructors; see also WithMinDaysInXXX().
    public FauxCalendarScope(CalendricalSegment segment)
        : base(default, segment) { }

    public override void ValidateYearMonth(int year, int month, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null) => throw new NotSupportedException();
}
