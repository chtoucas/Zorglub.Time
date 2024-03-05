// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Samples;

using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

// Verification that one can create a calendar type without having access to
// the internals of the assembly Zorglub.

public class MyMinMaxYearCalendar : BasicCalendar<MinMaxYearScope>, ICalendar<DateParts>
{
    public MyMinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope)
    {
        PartsAdapter = new PartsAdapter(Schema);
    }

    protected PartsAdapter PartsAdapter { get; }

    //
    // Year, month, day infos
    //

    [Pure]
    public sealed override int CountMonthsInYear(int year)
    {
        YearsValidator.Validate(year);
        return Schema.CountMonthsInYear(year);
    }

    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        YearsValidator.Validate(year);
        return Schema.CountDaysInYear(year);
    }

    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.CountDaysInMonth(year, month);
    }

    //
    // IDateProvider<DateParts>
    //

    [Pure]
    public IEnumerable<DateParts> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        YearsValidator.Validate(year);

        return iterator();

        IEnumerable<DateParts> iterator()
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
    public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
    {
        // Check arg eagerly.
        Scope.ValidateYearMonth(year, month);

        return iterator();

        IEnumerable<DateParts> iterator()
        {
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new DateParts(year, month, d);
            }
        }
    }

    [Pure]
    public DateParts GetStartOfYear(int year)
    {
        YearsValidator.Validate(year);
        return DateParts.AtStartOfYear(year);
    }

    [Pure]
    public DateParts GetEndOfYear(int year)
    {
        YearsValidator.Validate(year);
        return PartsAdapter.GetDatePartsAtEndOfYear(year);
    }

    [Pure]
    public DateParts GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return DateParts.AtStartOfMonth(year, month);
    }

    [Pure]
    public DateParts GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
    }
}
