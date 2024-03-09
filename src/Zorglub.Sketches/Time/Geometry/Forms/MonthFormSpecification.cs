// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry.Forms;

public sealed class MonthFormSpecification
{
    public MonthFormNumbering Numbering { get; set; }

    public int MonthsInYear { get; set; }

    public int ExceptionalMonth { get; set; }

    public bool LeapYear { get; set; } = true;
}
