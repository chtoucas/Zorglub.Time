// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas;

// TODO: add YearForm/MonthForm?

public interface IGeometricSchema
{
    /// <summary>
    /// Counts the number of consecutive days from the epoch to the
    /// specified date.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure] int CountDaysSinceEpoch(int y, int m, int d);

    /// <summary>
    /// Obtains the year, month and day of the month for the specified day
    /// count (the number of consecutive days from the epoch to a date); the
    /// results are given in output parameters.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d);
}
