// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using Zorglub.Time.Hemerology.Scopes;
    using Zorglub.Time.Specialized;

    public class GregorianMinMaxYearCalendar : MinMaxYearCalendar<GregorianDate>
    {
        public GregorianMinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }
        public GregorianMinMaxYearCalendar(string name, CalendarScope scope) : base(name, scope) { }

        protected override GregorianDate GetDate(int daysSinceEpoch) => new(Epoch + daysSinceEpoch);
    }
}
