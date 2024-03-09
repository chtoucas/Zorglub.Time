// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

public sealed class FauxSpecialAdjuster<TDate> : SpecialAdjuster<TDate>
    where TDate : IDateable
{
    public FauxSpecialAdjuster(MinMaxYearScope scope) : base(scope) { }

    private protected override TDate GetDate(int daysSinceEpoch) =>
        throw new NotSupportedException();
}
