// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms
{
    public sealed class OrdinalMonthRegularizer : MonthRegularizer
    {
        public OrdinalMonthRegularizer(int monthsInYear, int exceptionalMonth)
            : base(monthsInYear, exceptionalMonth) { }

        public override void Regularize(ref int y, ref int m)
        {
            // Après transformation:
            //   mars: 1, avril: 2, ..., décembre: 10, janvier: 11, février: 12
            // autrement dit numérotation ordinaire en partant de mars.

            if (m <= ExceptionalMonth)
            {
                y--;
                m += MonthsAfterExceptionalMonth;
            }
            else
            {
                m -= ExceptionalMonth;
            }
        }

        public override void Deregularize(ref int y0, ref int m0)
        {
            if (m0 > MonthsAfterExceptionalMonth)
            {
                y0++;
                m0 -= MonthsAfterExceptionalMonth;
            }
            else
            {
                m0 += ExceptionalMonth;
            }
        }
    }
}
