// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using System.Collections.Generic;

using Zorglub.Time.Hemerology.Scopes;

public sealed class FauxCalendar<TDate> : BasicCalendar<CalendarScope>, ICalendar<TDate>
{
    public FauxCalendar(string name, CalendarScope scope) : base(name, scope) { }

    public override int CountDaysInMonth(int year, int month) => throw new NotSupportedException();
    public override int CountDaysInYear(int year) => throw new NotSupportedException();
    public override int CountMonthsInYear(int year) => throw new NotSupportedException();

    public IEnumerable<TDate> GetDaysInMonth(int year, int month) => throw new NotSupportedException();
    public IEnumerable<TDate> GetDaysInYear(int year) => throw new NotSupportedException();

    public TDate GetStartOfYear(int year) => throw new NotSupportedException();
    public TDate GetEndOfYear(int year) => throw new NotSupportedException();

    public TDate GetStartOfMonth(int year, int month) => throw new NotSupportedException();
    public TDate GetEndOfMonth(int year, int month) => throw new NotSupportedException();
}
