// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes;

internal class FauxShortScope : ShortScope
{
    public FauxShortScope(ICalendricalSchema schema, DayNumber epoch, int minYear)
        : base(schema, epoch, minYear) { }

    public override void ValidateYear(int year, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateYearMonth(int year, int month, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null) => throw new NotSupportedException();
    public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null) => throw new NotSupportedException();
}
