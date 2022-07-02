// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Zorglub.Time;
using Zorglub.Time.Core;
using Zorglub.Time.Core.Intervals;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology;

// Demonstration that almost all operations only depend on the schema.
//
// No epoch means no interconversion and no day of the week.
// Actually we can do everything since we focus on the Gregorian case, but
// let's keep the code as general as possible (only s_Schema is specific
// here). For exactly the same reason, the code may NOT be optimal.
// We shall use it to compare performance between the various date types.

/// <summary>
/// Provides an affine Gregorian date as a struct.
/// </summary>
public readonly partial struct GregorianTriple :
    IAffineDate<GregorianTriple>,
    IYearEndpointsProvider<GregorianTriple>,
    IMonthEndpointsProvider<GregorianTriple>,
    IMinMaxValue<GregorianTriple>,
    ISubtractionOperators<GregorianTriple, int, GregorianTriple>
{
    private static readonly SchemaContext __ = SchemaContext.Create<GregorianSchema>();

    [Pure]
    public override string ToString()
    {
        var (y, m, d) = _bin;
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} (Gregorian)");
    }
}

public partial struct GregorianTriple
{
    private readonly Yemoda _bin;

    public GregorianTriple(int year, int month, int day)
    {
        _bin = PartsCreator.CreateYemoda(year, month, day);
    }

    private GregorianTriple(Yemoda bin)
    {
        _bin = bin;
    }

    private static CalendricalSchema Schema { get; } = __.Schema;
    private static ICalendricalPartsFactory PartsFactory { get; } = __.PartsFactory;
    private static PartsCreator PartsCreator { get; } = __.PartsCreator;
    private static ICalendricalArithmetic Arithmetic { get; } = __.Arithmetic;
    private static Range<int> Domain { get; } = Schema.Domain;

    public static Range<int> SupportedYears => Schema.SupportedYears;
    public static GregorianTriple MinValue { get; } = new(PartsFactory.GetDatePartsAtStartOfYear(SupportedYears.Min));
    public static GregorianTriple MaxValue { get; } = new(PartsFactory.GetDatePartsAtEndOfYear(SupportedYears.Max));

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
        if (Domain.Contains(daysSinceEpoch) == false) throw new ArgumentOutOfRangeException(nameof(daysSinceEpoch));

        var ymd = PartsFactory.GetDateParts(daysSinceEpoch);
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
    #region Year and month boundaries

    [Pure]
    public static GregorianTriple GetStartOfYear(GregorianTriple day) => new(day._bin.StartOfYear);

    [Pure]
    public static GregorianTriple GetEndOfYear(GregorianTriple day)
    {
        int y = day.Year;
        var (_, m, d) = PartsFactory.GetDatePartsAtEndOfYear(y);
        // TODO(code): bypass validation? Idem GetEndOfMonth().
        return new GregorianTriple(y, m, d);
    }

    [Pure]
    public static GregorianTriple GetStartOfMonth(GregorianTriple day) => new(day._bin.StartOfMonth);

    [Pure]
    public static GregorianTriple GetEndOfMonth(GregorianTriple day)
    {
        var (y, m, _) = day;
        int d = Schema.CountDaysInMonth(y, m);
        return new GregorianTriple(y, m, d);
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
