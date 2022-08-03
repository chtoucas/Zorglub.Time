// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Validation;
using Zorglub.Time.Hemerology;

using static Zorglub.Time.Extensions.Unboxing;

using ZRange = Zorglub.Time.Core.Intervals.Range;

// Demonstration that almost all operations only depend on the schema.
//
// No epoch means no interconversion and no day of the week.
// Actually we can do everything since we focus on the Gregorian case, but
// let's keep the code as general as possible (only s_Schema is specific
// here). For exactly the same reason, the code may NOT be optimal.
// We shall use it to compare performance between the various date types.

/// <summary>
/// Provides an <i>affine</i> Gregorian date as a struct based on <see cref="Yemoda"/>.
/// <para>Suppoted years = [1, 9999]</para>
/// </summary>
public readonly partial struct CivilTriple :
    IAffineDate<CivilTriple>,
    IMinMaxValue<CivilTriple>
{
    // NB: the order in which the static fields are written is important.
    // Being based on Yemoda, the schema should derived from SystemSchema.

    private static readonly SystemSchema s_Schema = CivilSchema.GetInstance().Unbox();
    private static readonly SystemSegment s_Segment = SystemSegment.Create(s_Schema, ZRange.Create(1, 9999));

    private static readonly DaysValidator s_DaysValidator = new(s_Segment.SupportedDays);

    private static readonly SystemPartsFactory s_PartsFactory = SystemPartsFactory.Create(s_Segment);
    private static readonly SystemArithmetic s_Arithmetic = SystemArithmetic.CreateDefault(s_Segment);

    private static readonly CivilTriple s_MinValue = new(s_Segment.MinMaxDateParts.LowerValue);
    private static readonly CivilTriple s_MaxValue = new(s_Segment.MinMaxDateParts.UpperValue);

    private readonly Yemoda _bin;

    public CivilTriple(int year, int month, int day)
    {
        _bin = s_PartsFactory.CreateYemoda(year, month, day);
    }

    private CivilTriple(Yemoda bin)
    {
        _bin = bin;
    }

    public static CivilTriple MinValue => s_MinValue;
    public static CivilTriple MaxValue => s_MaxValue;

    public Ord CenturyOfEra => Ord.FromInt32(Century);
    public int Century => YearNumbering.GetCentury(Year);
    public Ord YearOfEra => Ord.FromInt32(Year);
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    public int Year => _bin.Year;
    public int Month => _bin.Month;

    public int DayOfYear
    {
        get
        {
            var (y, m, d) = _bin;
            return s_Schema.GetDayOfYear(y, m, d);
        }
    }

    public int Day => _bin.Day;

    public bool IsIntercalary
    {
        get
        {
            var (y, m, d) = _bin;
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    public bool IsSupplementary
    {
        get
        {
            var (y, m, d) = _bin;
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    [Pure]
    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = _bin;

    public void Deconstruct(out int year, out int dayOfYear)
    {
        (year, var m, var d) = _bin;
        dayOfYear = s_Schema.GetDayOfYear(year, m, d);
    }
}

public partial struct CivilTriple // Conversions, adjustments...
{
    [Pure]
    public static CivilTriple FromDaysSinceEpoch(int daysSinceEpoch)
    {
        s_DaysValidator.Validate(daysSinceEpoch);
        var ymd = s_Schema.GetDateParts(daysSinceEpoch);
        return new CivilTriple(ymd);
    }

    [Pure]
    public int CountDaysSinceEpoch()
    {
        var (y, m, d) = _bin;
        return s_Schema.CountDaysSinceEpoch(y, m, d);
    }

    #region Counting

    [Pure]
    public int CountElapsedDaysInYear()
    {
        var (y, m, d) = _bin;
        return s_Schema.CountDaysInYearBefore(y, m, d);
    }

    [Pure]
    public int CountRemainingDaysInYear()
    {
        var (y, m, d) = _bin;
        return s_Schema.CountDaysInYearAfter(y, m, d);
    }

    [Pure]
    public int CountElapsedDaysInMonth() => Day - 1;

    [Pure]
    public int CountRemainingDaysInMonth()
    {
        var (y, m, d) = _bin;
        return s_Schema.CountDaysInMonthAfter(y, m, d);
    }

    #endregion
}

public partial struct CivilTriple // IEquatable
{
    public static bool operator ==(CivilTriple left, CivilTriple right) => left._bin == right._bin;
    public static bool operator !=(CivilTriple left, CivilTriple right) => left._bin != right._bin;

    [Pure]
    public bool Equals(CivilTriple other) => _bin == other._bin;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilTriple date && Equals(date);

    [Pure]
    public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct CivilTriple // IComparable
{
    public static bool operator <(CivilTriple left, CivilTriple right) => left._bin < right._bin;
    public static bool operator <=(CivilTriple left, CivilTriple right) => left._bin <= right._bin;
    public static bool operator >(CivilTriple left, CivilTriple right) => left._bin > right._bin;
    public static bool operator >=(CivilTriple left, CivilTriple right) => left._bin >= right._bin;

    [Pure]
    public static CivilTriple Min(CivilTriple x, CivilTriple y) => x.CompareTo(y) < 0 ? x : y;

    [Pure]
    public static CivilTriple Max(CivilTriple x, CivilTriple y) => x.CompareTo(y) > 0 ? x : y;

    [Pure]
    public int CompareTo(CivilTriple other) => _bin.CompareTo(other._bin);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilTriple date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(CivilTriple).GetType()}.",
            nameof(obj));
}

public partial struct CivilTriple // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(CivilTriple left, CivilTriple right) => left.CountDaysSince(right);
    public static CivilTriple operator +(CivilTriple value, int days) => value.PlusDays(days);
    public static CivilTriple operator -(CivilTriple value, int days) => value.PlusDays(-days);
    public static CivilTriple operator ++(CivilTriple value) => value.NextDay();
    public static CivilTriple operator --(CivilTriple value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(CivilTriple other) => s_Arithmetic.CountDaysBetween(other._bin, _bin);

    [Pure]
    public CivilTriple PlusDays(int days) => new(s_Arithmetic.AddDays(_bin, days));

    [Pure]
    public CivilTriple NextDay() => new(s_Arithmetic.NextDay(_bin));

    [Pure]
    public CivilTriple PreviousDay() => new(s_Arithmetic.PreviousDay(_bin));
}
