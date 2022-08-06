// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Hemerology;

public sealed class FauxSpecialAdjusters<TDate> : SpecialAdjusters<TDate>
    where TDate : IFixedDay<TDate>, IFixedDateable
{
    public FauxSpecialAdjusters(ICalendar<TDate> calendar) : base(calendar) { }

    protected override TDate GetDate(int daysSinceEpoch) =>
        TDate.FromDayNumber(Scope.Epoch + daysSinceEpoch);
}
