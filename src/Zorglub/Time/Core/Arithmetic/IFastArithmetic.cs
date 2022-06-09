// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Arithmetic
{
    internal interface IFastArithmetic : ICalendricalArithmetic
    {
        [Pure] Yemoda AddDaysViaDayOfMonth(Yemoda ymd, int days);

        [Pure] Yedoy AddDaysViaDayOfYear(Yedoy ydoy, int days);
    }
}
