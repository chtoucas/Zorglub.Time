// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Core.Schemas;

internal static class SchemaHelpers
{
    // TODO(code): pas très propre tout ça.
    public static int[] GetDaysInMonthArray<TSchema>(bool leap)
        where TSchema : IDaysInMonthDistribution
    {
        var span = TSchema.GetDaysInMonthDistribution(leap);

        return Array.ConvertAll(span.ToArray(), c => (int)c);
    }
}
