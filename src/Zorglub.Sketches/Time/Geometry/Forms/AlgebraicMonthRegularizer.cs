// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Forms;

public sealed class AlgebraicMonthRegularizer : MonthRegularizer
{
    public AlgebraicMonthRegularizer(int monthsInYear, int exceptionalMonth)
        : base(monthsInYear, exceptionalMonth) { }

    public override void Regularize(ref int y, ref int m)
    {
        // Après transformation:
        //   mars: 0, avril: 1, ..., décembre: 9, janvier: 10, février: 11
        // autrement dit numérotation algébrique en partant de mars.

        m--;
        if (m < ExceptionalMonth)
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
        m0++;
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
