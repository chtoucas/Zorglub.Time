// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

public sealed class FauxNakedGregorianCalendar : NakedCalendar
{
    public FauxNakedGregorianCalendar(string name, GregorianSchema schema, FauxGregorianScope scope)
        : base(name, schema, scope) { }

    public override int CountDaysInMonth(int year, int month) => throw new NotSupportedException();
    public override int CountDaysInYear(int year) => throw new NotSupportedException();
    public override int CountMonthsInYear(int year) => throw new NotSupportedException();

    public override IEnumerable<DateParts> GetDaysInMonth(int year, int month) => throw new NotSupportedException();
    public override IEnumerable<DateParts> GetDaysInYear(int year) => throw new NotSupportedException();

    public override DateParts GetStartOfYear(int year) => throw new NotSupportedException();
    public override DateParts GetEndOfYear(int year) => throw new NotSupportedException();
    public override DateParts GetStartOfMonth(int year, int month) => throw new NotSupportedException();
    public override DateParts GetEndOfMonth(int year, int month) => throw new NotSupportedException();
}
