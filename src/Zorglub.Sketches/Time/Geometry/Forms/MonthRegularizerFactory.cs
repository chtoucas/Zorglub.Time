// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms
{
    public static class MonthRegularizerFactory
    {
        [Pure]
        public static MonthRegularizer CreateRegularizer(
            MonthForm monthForm,
            int monthsInYear,
            int exceptionalMonth)
        {
            Requires.NotNull(monthForm);

            return monthForm switch
            {
                MonthForm { Numbering: MonthFormNumbering.Algebraic } =>
                    new AlgebraicMonthRegularizer(monthsInYear, exceptionalMonth),

                MonthForm { Numbering: MonthFormNumbering.Ordinal } =>
                    new OrdinalMonthRegularizer(monthsInYear, exceptionalMonth),

                TroeschMonthForm =>
                    new TroeschMonthRegularizer(monthsInYear, exceptionalMonth),

                _ => Throw.NotSupported<MonthRegularizer>()
            };
        }
    }
}
