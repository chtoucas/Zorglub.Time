// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Hemerology.Scopes;

public sealed class FauxCalendricalScope : CalendarScope
{
    public FauxCalendricalScope(SystemSchema schema, DayNumber epoch)
        : base(schema, epoch, MinMaxYearScope.GetSegment(schema, 1, 2)) { }

    public bool ValidateOrdinalWasCalled { get; private set; }
    public bool ValidateYearMonthWasCalled { get; private set; }
    public bool ValidateYearMonthDayWasCalled { get; private set; }

    public override void ValidateYearMonth(int year, int month, string? paramName = null) =>
        ValidateYearMonthWasCalled = true;

    public override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null) =>
        ValidateYearMonthDayWasCalled = true;

    public override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null) =>
        ValidateOrdinalWasCalled = true;
}
