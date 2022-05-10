// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology.Scopes;

internal class FauxProlepticShortScope : ProlepticShortScope
{
    public FauxProlepticShortScope(ICalendricalSchema schema, DayNumber epoch) : base(schema, epoch) { }

    public override void ValidateYearMonth(int year, int month, string? paramName = null) =>
        throw new NotSupportedException();

    public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null) =>
        throw new NotSupportedException();

    public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null) =>
        throw new NotSupportedException();
}
