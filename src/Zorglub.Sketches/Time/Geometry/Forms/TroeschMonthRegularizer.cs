// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry.Forms;

public sealed class TroeschMonthRegularizer : MonthRegularizer
{
    public TroeschMonthRegularizer(int monthsInYear, int exceptionalMonth)
        : base(monthsInYear, exceptionalMonth) { }

    public override void Regularize(ref int y, ref int m)
    {
        // Après transformation :
        //   mars: 3, avril: 4, ..., décembre: 12, janvier: 13, février: 14.
        // autrement dit numérotation spéciale en partant de mars.

        if (m <= ExceptionalMonth)
        {
            y--;
            m += MonthsInYear;
        }
    }

    public override void Deregularize(ref int y0, ref int m0)
    {
        if (m0 > MonthsInYear)
        {
            y0++;
            m0 -= MonthsInYear;
        }
    }
}
