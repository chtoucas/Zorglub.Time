// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Verification that one can create a calendar type without having access to
// the internals of the assembly Zorglub.

public class MyNakedCalendar : NakedCalendar
{
    public MyNakedCalendar(string name, ICalendricalSchema schema, DayNumber epoch)
        : this(name, new MinMaxYearScope(schema, epoch, 1, 9999)) { }

    public MyNakedCalendar(string name, MinMaxYearScope scope)
        : base(name, scope) { }

    //
    // Year, month, day infos
    //

    [Pure]
    public sealed override int CountMonthsInYear(int year)
    {
        SupportedYears.Validate(year);
        return Schema.CountMonthsInYear(year);
    }

    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        SupportedYears.Validate(year);
        return Schema.CountDaysInYear(year);
    }

    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.CountDaysInMonth(year, month);
    }

    //
    // Dates in a given year or month
    //

    [Pure]
    public sealed override IEnumerable<DateParts> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        SupportedYears.Validate(year);

        return Iterator();

        IEnumerable<DateParts> Iterator()
        {
            int monthsInYear = Schema.CountMonthsInYear(year);

            for (int m = 1; m <= monthsInYear; m++)
            {
                int daysInMonth = Schema.CountDaysInMonth(year, m);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new DateParts(year, m, d);
                }
            }
        }
    }

    [Pure]
    public sealed override IEnumerable<DateParts> GetDaysInMonth(int year, int month)
    {
        // Check arg eagerly.
        Scope.ValidateYearMonth(year, month);

        return Iterator();

        IEnumerable<DateParts> Iterator()
        {
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new DateParts(year, month, d);
            }
        }
    }

    [Pure]
    public sealed override DateParts GetStartOfYear(int year)
    {
        SupportedYears.Validate(year);
        return DateParts.AtStartOfYear(year);
    }

    [Pure]
    public sealed override DateParts GetEndOfYear(int year)
    {
        SupportedYears.Validate(year);
        return PartsAdapter.GetDatePartsAtEndOfYear(year);
    }

    [Pure]
    public sealed override DateParts GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return DateParts.AtStartOfMonth(year, month);
    }

    [Pure]
    public sealed override DateParts GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
    }
}
