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

// Demonstration that almost all operations only depend on the schema.
//
// No epoch means no interconversion and no day of the week.
// Actually we can do everything since we focus on the Gregorian case, but
// let's keep the code as general as possible (only s_Schema is specific
// here). For exactly the same reason, the code may NOT be optimal.
// We shall use it to compare performance between the various date types.

/// <summary>
/// Provides an affine Gregorian date as a struct based on <see cref="Yemoda"/>.
/// </summary>
public readonly partial struct GregorianTriple :
    IAffineDate<GregorianTriple>,
    IMinMaxValue<GregorianTriple>
{
    private static readonly SystemSchema Schema = SchemaActivator.CreateInstance<GregorianSchema>();
    private static readonly SystemSegment Segment = SystemSegment.Create(Schema, Schema.SupportedYears);

    [Pure]
    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }
}

public partial struct GregorianTriple
{
    private static DaysValidator DaysValidator { get; } = new(Segment.SupportedDays);

    private static SystemPartsFactory PartsFactory { get; } = SystemPartsFactory.Create(Segment);
    private static SystemArithmetic Arithmetic { get; } = SystemArithmetic.CreateDefault(Segment);

    private readonly Yemoda _bin;

    public GregorianTriple(int year, int month, int day)
    {
        _bin = PartsFactory.CreateYemoda(year, month, day);
    }

    private GregorianTriple(Yemoda bin)
    {
        _bin = bin;
    }

    public static GregorianTriple MinValue { get; } = new(Segment.MinMaxDateParts.LowerValue);
    public static GregorianTriple MaxValue { get; } = new(Segment.MinMaxDateParts.UpperValue);

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
            return Schema.GetDayOfYear(y, m, d);
        }
    }

    public int Day => _bin.Day;

    public bool IsIntercalary
    {
        get
        {
            var (y, m, d) = _bin;
            return Schema.IsIntercalaryDay(y, m, d);
        }
    }

    public bool IsSupplementary
    {
        get
        {
            var (y, m, d) = _bin;
            return Schema.IsSupplementaryDay(y, m, d);
        }
    }

    public void Deconstruct(out int year, out int month, out int day) =>
        (year, month, day) = _bin;
}

public partial struct GregorianTriple // Conversions, adjustments...
{
    [Pure]
    public static GregorianTriple FromDaysSinceEpoch(int daysSinceEpoch)
    {
        DaysValidator.Validate(daysSinceEpoch);
        var ymd = Schema.GetDateParts(daysSinceEpoch);
        return new GregorianTriple(ymd);
    }

    [Pure]
    public int CountDaysSinceEpoch()
    {
        var (y, m, d) = _bin;
        return Schema.CountDaysSinceEpoch(y, m, d);
    }

    #region Counting

    [Pure]
    public int CountElapsedDaysInYear()
    {
        var (y, m, d) = _bin;
        return Schema.CountDaysInYearBefore(y, m, d);
    }

    [Pure]
    public int CountRemainingDaysInYear()
    {
        var (y, m, d) = _bin;
        return Schema.CountDaysInYearAfter(y, m, d);
    }

    [Pure]
    public int CountElapsedDaysInMonth() => Day - 1;

    [Pure]
    public int CountRemainingDaysInMonth()
    {
        var (y, m, d) = _bin;
        return Schema.CountDaysInMonthAfter(y, m, d);
    }

    #endregion
}

public partial struct GregorianTriple // IEquatable
{
    public static bool operator ==(GregorianTriple left, GregorianTriple right) => left._bin == right._bin;
    public static bool operator !=(GregorianTriple left, GregorianTriple right) => left._bin != right._bin;

    [Pure]
    public bool Equals(GregorianTriple other) => _bin == other._bin;

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is GregorianTriple date && Equals(date);

    [Pure]
    public override int GetHashCode() => _bin.GetHashCode();
}

public partial struct GregorianTriple // IComparable
{
    public static bool operator <(GregorianTriple left, GregorianTriple right) => left._bin < right._bin;
    public static bool operator <=(GregorianTriple left, GregorianTriple right) => left._bin <= right._bin;
    public static bool operator >(GregorianTriple left, GregorianTriple right) => left._bin > right._bin;
    public static bool operator >=(GregorianTriple left, GregorianTriple right) => left._bin >= right._bin;

    [Pure]
    public static GregorianTriple Min(GregorianTriple x, GregorianTriple y) => x.CompareTo(y) < 0 ? x : y;

    [Pure]
    public static GregorianTriple Max(GregorianTriple x, GregorianTriple y) => x.CompareTo(y) > 0 ? x : y;

    [Pure]
    public int CompareTo(GregorianTriple other) => _bin.CompareTo(other._bin);

    [Pure]
    public int CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianTriple date ? CompareTo(date)
        : throw new ArgumentException(
            $"The object should be of type {nameof(obj)} but it is of type {typeof(GregorianTriple).GetType()}.",
            nameof(obj));
}

public partial struct GregorianTriple // Math ops
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage)

    public static int operator -(GregorianTriple left, GregorianTriple right) => left.CountDaysSince(right);
    public static GregorianTriple operator +(GregorianTriple value, int days) => value.PlusDays(days);
    public static GregorianTriple operator -(GregorianTriple value, int days) => value.PlusDays(-days);
    public static GregorianTriple operator ++(GregorianTriple value) => value.NextDay();
    public static GregorianTriple operator --(GregorianTriple value) => value.PreviousDay();

#pragma warning restore CA2225

    [Pure]
    public int CountDaysSince(GregorianTriple other) =>
        Arithmetic.CountDaysBetween(other._bin, _bin);

    [Pure]
    public GregorianTriple PlusDays(int days) =>
        new(Arithmetic.AddDays(_bin, days));

    [Pure]
    public GregorianTriple NextDay() =>
        new(Arithmetic.NextDay(_bin));

    [Pure]
    public GregorianTriple PreviousDay() =>
        new(Arithmetic.PreviousDay(_bin));
}
