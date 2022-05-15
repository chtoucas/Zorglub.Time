// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core
{
    // TODO(doc): expliquer pourquoi cette classe est publique.
    // Construction n'est pas possible directement car un schéma n'a pas de
    // constructeur publique (GetInstance() -> Box<schema>).
    // ICalendricalSchemaPlus:
    // - si on a un ICalendricalSchema, on l'étend en utilisant checked = true,
    //   ainsi toute création de Yemoda/Yedoy est protégée (Yemoda.Create()).
    // - si on a un ICalendricalSchemaPlus, on suppose... quoi?

    /// <summary>
    /// Provides a plain implementation for <see cref="ICalendricalArithmetic"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class CalendricalArithmetic : ICalendricalArithmetic
    {
        /// <summary>
        /// Represents the earliest day number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _minDaysSinceEpoch;

        /// <summary>
        /// Represents the latest day number.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly int _maxDaysSinceEpoch;

        /// <summary>
        /// Represents the schema.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalSchema _schema;

        /// <summary>
        /// Represents the factory for calendrical parts.
        /// <para>This field is read-only.</para>
        /// </summary>
        private readonly ICalendricalPartsFactory _partsFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendricalArithmetic"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        public CalendricalArithmetic(ICalendricalSchema schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _partsFactory = ICalendricalPartsFactory.Create(schema, @checked: true);

            (_minDaysSinceEpoch, _maxDaysSinceEpoch) =
                schema.SupportedYears.Endpoints.Select(schema.GetStartOfYear, schema.GetEndOfYear);
        }
    }

    public partial class CalendricalArithmetic // Operations on Yemoda
    {
        /// <inheritdoc />
        [Pure]
        public Yemoda AddDays(Yemoda ymd, int days)
        {
            ymd.Unpack(out int y, out int m, out int d);

            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, m, d) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return _partsFactory.GetDateParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public Yemoda NextDay(Yemoda ymd) => AddDays(ymd, 1);

        /// <inheritdoc />
        [Pure]
        public Yemoda PreviousDay(Yemoda ymd) => AddDays(ymd, -1);

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(Yemoda start, Yemoda end)
        {
            start.Unpack(out int y0, out int m0, out int d0);
            end.Unpack(out int y1, out int m1, out int d1);

            return _schema.CountDaysSinceEpoch(y1, m1, d1) - _schema.CountDaysSinceEpoch(y0, m0, d0);
        }
    }

    public partial class CalendricalArithmetic // Operations on Yedoy
    {
        /// <inheritdoc />
        [Pure]
        public Yedoy AddDays(Yedoy ydoy, int days)
        {
            ydoy.Unpack(out int y, out int doy);

            int daysSinceEpoch = checked(_schema.CountDaysSinceEpoch(y, doy) + days);
            if (daysSinceEpoch < _minDaysSinceEpoch || daysSinceEpoch > _maxDaysSinceEpoch)
            {
                Throw.DateOverflow();
            }

            return _partsFactory.GetOrdinalParts(daysSinceEpoch);
        }

        /// <inheritdoc />
        [Pure]
        public Yedoy NextDay(Yedoy ydoy) => AddDays(ydoy, 1);

        /// <inheritdoc />
        [Pure]
        public Yedoy PreviousDay(Yedoy ydoy) => AddDays(ydoy, -1);

        /// <inheritdoc />
        [Pure]
        public int CountDaysBetween(Yedoy start, Yedoy end)
        {
            start.Unpack(out int y0, out int doy0);
            end.Unpack(out int y1, out int doy1);

            return _schema.CountDaysSinceEpoch(y1, doy1) - _schema.CountDaysSinceEpoch(y0, doy0);
        }
    }
}
