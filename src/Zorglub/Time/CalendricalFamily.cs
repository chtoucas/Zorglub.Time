// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time;

/// <summary>Specifies the family of the calendar according to its main cycle.</summary>
public enum CalendricalFamily
{
    /// <summary>The calendar family is not directly related to any terrestrial cycle.</summary>
    Other = 0,

    /// <summary>The calendar cycle is based upon the Annus Vagus.
    /// <para>A vague (or wandering) year is uniformly 365-day long, it is the integer truncation of
    /// the tropical year.</para>
    /// <para>At first sight, we might think that this type of calendar uses the tropical year, but
    /// it is not the case since the cycles are out of sync with the seasons, therefore we keep it
    /// separated from the solar family.</para></summary>
    AnnusVagus,

    /// <summary>The calendar cycle is based upon the position of Earth on its revolution around the
    /// Sun.
    /// <para>A common year is 365 days long and a leap year is 366 days long.</para></summary>
    Solar,

    /// <summary>The calendar cycle is based upon the phases of the Moon.
    /// <para>A common year is 354 days long and a leap year is 355 days long.</para></summary>
    Lunar,

    /// <summary>The calendar tries to maintain synchronicity with both the lunar and the solar
    /// cycles.</summary>
    Lunisolar,
}
