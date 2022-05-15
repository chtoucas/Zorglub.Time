// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms
{
    /// <summary>
    /// Provides extension methods for <see cref="MonthForm"/>.
    /// </summary>
    public static class MonthFormExtensions
    {
        [Pure]
        public static MonthForm WithAlgebraicNumbering(this MonthForm @this)
        {
            Requires.NotNull(@this);

            return @this switch
            {
                MonthForm { Numbering: MonthFormNumbering.Algebraic } => @this,

                MonthForm { Numbering: MonthFormNumbering.Ordinal } =>
                    AdjustNumbering(@this, MonthFormNumbering.Algebraic, -1),

                TroeschMonthForm t =>
                    AdjustNumbering(t, MonthFormNumbering.Algebraic, -t.ExceptionalMonth - 1),

                _ => Throw.NotSupported<MonthForm>()
            };
        }

        [Pure]
        public static MonthForm WithOrdinalNumbering(this MonthForm @this)
        {
            Requires.NotNull(@this);

            return @this switch
            {
                MonthForm { Numbering: MonthFormNumbering.Algebraic } =>
                    AdjustNumbering(@this, MonthFormNumbering.Ordinal, 1),

                MonthForm { Numbering: MonthFormNumbering.Ordinal } => @this,

                TroeschMonthForm t =>
                    AdjustNumbering(t, MonthFormNumbering.Ordinal, -t.ExceptionalMonth),

                _ => Throw.NotSupported<MonthForm>()
            };
        }

        [Pure]
        public static TroeschMonthForm WithTroeschNumbering(
            this MonthForm @this, int exceptionalMonth)
        {
            Requires.NotNull(@this);

            return @this switch
            {
                MonthForm { Numbering: MonthFormNumbering.Algebraic } =>
                    WithTroeschNumbering(@this, exceptionalMonth + 1, exceptionalMonth),

                MonthForm { Numbering: MonthFormNumbering.Ordinal } =>
                    WithTroeschNumbering(@this, exceptionalMonth, exceptionalMonth),

                TroeschMonthForm t => t,

                _ => Throw.NotSupported<TroeschMonthForm>()
            };

            static TroeschMonthForm WithTroeschNumbering(
                MonthForm form, int offset, int exceptionalMonth)
            {
                Debug.Assert(form != null);

                return new(
                    form.A,
                    form.B,
                    form.Remainder - form.A * offset,
                    exceptionalMonth)
                // REVIEW: Origin.
                { Origin = form.Origin };
            }
        }

        [Pure]
        private static MonthForm AdjustNumbering(
            MonthForm form, MonthFormNumbering numbering, int offset)
        {
            Debug.Assert(form != null);

            return new(
                form.A,
                form.B,
                form.Remainder - form.A * offset,
                numbering)
            // REVIEW: Origin.
            { Origin = form.Origin };
        }
    }
}
