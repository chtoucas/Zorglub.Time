// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas;

// daysToTargetEpoch = count of days from the epoch used by the schema to
// the target epoch. In general, it matches the number of days in a year left
// after the exceptional month. In more complex situations, we might have to
// shift the epoch backwards by more than a year.

public sealed class RebasedSchema : IGeometricSchema
{
    private readonly IGeometricSchema _schema;
    private readonly int _daysToTargetEpoch;

    public RebasedSchema(IGeometricSchema schema, int daysToTargetEpoch)
    {
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        _daysToTargetEpoch = daysToTargetEpoch;
    }

    /// <inheritdoc />
    [Pure]
    public int CountDaysSinceEpoch(int y, int m, int d) =>
        _schema.CountDaysSinceEpoch(y, m, d) - _daysToTargetEpoch;

    /// <inheritdoc />
    public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        daysSinceEpoch += _daysToTargetEpoch;

        _schema.GetDateParts(daysSinceEpoch, out y, out m, out d);
    }
}
