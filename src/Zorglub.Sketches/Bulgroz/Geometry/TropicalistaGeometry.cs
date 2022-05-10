// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Bulgroz.Geometry;

using Zorglub.Time.Geometry.Forms;
using Zorglub.Time.Geometry.Schemas;

using static Zorglub.Bulgroz.TropicalistaConstants;

/// <summary>
/// Provides geometric schemas for the various "Tropicália" calendars.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class TropicalistaGeometry
{
    /// <summary>
    /// Represents the quasi-affine form (1461, 4, 0).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 365, 365, 365, 366.</para>
    /// </remarks>
    public static readonly YearForm YearForm = new(DaysPer4YearSubcycle, 4, 0);

    /// <summary>
    /// Represents the quasi-affine form (335, 11, 5).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 30-31 repeated 5 times,
    /// followed by 30-30.</para>
    /// <para>It matches the sequence of lengths of months in a <i>common</i>
    /// year.</para>
    /// </remarks>
    public static readonly MonthForm MonthForm3031ForCommonYear = new(335, 11, 5);

    /// <summary>
    /// Represents the quasi-affine form (61, 2, 0).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 30-31 repeated 6 times.</para>
    /// <para>It matches the sequence of lengths of months in a <i>leap</i>
    /// year.</para>
    /// </remarks>
    public static readonly MonthForm MonthForm3031ForLeapYear = new(61, 2, 0);

    //
    // Geometric schemas for Tropicalia 30-31.
    //

    private static readonly SecondOrderSchema s_SOSchema3031Short =
        new(YearForm, MonthForm3031ForCommonYear.WithOrdinalNumbering());
    public static LongCycleSchema Schema3031Short =>
        new(s_SOSchema3031Short, DaysPer128YearCycle, 128);

    private static readonly SecondOrderSchema s_SOSchema3031Long =
        new(YearForm, MonthForm3031ForLeapYear.WithOrdinalNumbering());
    public static LongCycleSchema Schema3031Long =>
        new(s_SOSchema3031Long, DaysPer128YearCycle, 128);

    //
    // Geometric schemas for Tropicalia 31-30.
    //

    /// <summary>
    /// Represents the quasi-affine form (61, 2, 1).
    /// <para>This field is read-only.</para>
    /// </summary>
    /// <remarks>
    /// <para>This form encodes the sequence: 31-30 repeated 6 times.</para>
    /// <para>It matches the sequence of lengths of months in a <i>leap</i>
    /// year.</para>
    /// <para>It also matches the sequence of lengths of the <i>first eleven
    /// months</i> in a common year.</para>
    /// </remarks>
    public static readonly MonthForm MonthForm3130 = new(61, 2, 1);

    private static readonly SecondOrderSchema s_SOSchema3130 =
        new(YearForm, MonthForm3130.WithOrdinalNumbering());
    public static LongCycleSchema Schema3130 =>
        new(s_SOSchema3130, DaysPer128YearCycle, 128);
}
