// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Hemerology.Scopes;

public sealed class FauxBasicCalendar : BasicCalendar<CalendarScope>
{
    public FauxBasicCalendar(string name, CalendarScope scope) : base(name, scope) { }

    public override int CountMonthsInYear(int year) => throw new NotSupportedException();
    public override int CountDaysInYear(int year) => throw new NotSupportedException();
    public override int CountDaysInMonth(int year, int month) => throw new NotSupportedException();
}
