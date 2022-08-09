// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized;

using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

public sealed class FauxSpecialAdjuster<TDate> : SpecialAdjuster<TDate>
    where TDate : IDate<TDate>, IDateableOrdinally
{
    public FauxSpecialAdjuster(ICalendar<TDate> calendar) : base(calendar) { }
    public FauxSpecialAdjuster(CalendarScope scope) : base(scope) { }

    private protected override TDate GetDate(int daysSinceEpoch) =>
        TDate.FromDayNumber(Scope.Epoch + daysSinceEpoch);
}
