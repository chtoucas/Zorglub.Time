// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Hemerology.Scopes;

public sealed class FauxGregorianScope : CalendarScope
{
    public FauxGregorianScope(GregorianSchema schema, int minYear, int maxYear)
        : this(schema, default, minYear, maxYear) { }

    public FauxGregorianScope(GregorianSchema schema, DayNumber epoch, int minYear, int maxYear)
        : base(schema, epoch, CalendricalSegment.Create(schema, minYear, maxYear)) { }

    public override void ValidateYearMonth(int year, int month, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null) => throw new NotSupportedException();
}
