// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Specialized
{
    using Zorglub.Time.Hemerology;

    public sealed class ArmenianAdjusters : DateAdjusters<ArmenianDate>
    {
        public ArmenianAdjusters() : this(ArmenianDate.Calendar) { }

        private ArmenianAdjusters(ArmenianCalendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override ArmenianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class Armenian13Adjusters : DateAdjusters<Armenian13Date>
    {
        public Armenian13Adjusters() : this(Armenian13Date.Calendar) { }

        private Armenian13Adjusters(Armenian13Calendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override Armenian13Date GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class CopticAdjusters : DateAdjusters<CopticDate>
    {
        public CopticAdjusters() : this(CopticDate.Calendar) { }

        private CopticAdjusters(CopticCalendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override CopticDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class Coptic13Adjusters : DateAdjusters<Coptic13Date>
    {
        public Coptic13Adjusters() : this(Coptic13Date.Calendar) { }

        private Coptic13Adjusters(Coptic13Calendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override Coptic13Date GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class EthiopicAdjusters : DateAdjusters<EthiopicDate>
    {
        public EthiopicAdjusters() : this(EthiopicDate.Calendar) { }

        private EthiopicAdjusters(EthiopicCalendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override EthiopicDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class Ethiopic13Adjusters : DateAdjusters<Ethiopic13Date>
    {
        public Ethiopic13Adjusters() : this(Ethiopic13Date.Calendar) { }

        private Ethiopic13Adjusters(Ethiopic13Calendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override Ethiopic13Date GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class TabularIslamicAdjusters : DateAdjusters<TabularIslamicDate>
    {
        public TabularIslamicAdjusters() : this(TabularIslamicDate.Calendar) { }

        private TabularIslamicAdjusters(TabularIslamicCalendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override TabularIslamicDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class WorldAdjusters : DateAdjusters<WorldDate>
    {
        public WorldAdjusters() : this(WorldDate.Calendar) { }

        private WorldAdjusters(WorldCalendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override WorldDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class ZoroastrianAdjusters : DateAdjusters<ZoroastrianDate>
    {
        public ZoroastrianAdjusters() : this(ZoroastrianDate.Calendar) { }

        private ZoroastrianAdjusters(ZoroastrianCalendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override ZoroastrianDate GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }

    public sealed class Zoroastrian13Adjusters : DateAdjusters<Zoroastrian13Date>
    {
        public Zoroastrian13Adjusters() : this(Zoroastrian13Date.Calendar) { }

        private Zoroastrian13Adjusters(Zoroastrian13Calendar calendar)
            : base(calendar.Epoch, calendar.Schema) { }

        [Pure]
        protected sealed override Zoroastrian13Date GetDate(int daysSinceEpoch) => new(daysSinceEpoch);
    }
}
