// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using Zorglub.Testing.Data;

#region Developer Notes

// Les tests de dépassement arithmétique dépendent de la configuration du
// projet, c-à-d de la valeur de la propriété CheckForOverflowUnderflow.
//
// Ces tests reposent sur l'hypothèse forte qu'il suffirait de vérifier les
// valeurs limites (MinYear, 1, 1) et (MaxYear, MaxMonth, MaxDay) car les
// fonctions testées sont croissantes.
// En fait, rien ne nous permet d'affirmer qu'un calcul intermédiaire pour des
// données entre ces limites n'échouera pas.

// Les tests suivants vérifient qu'il n'y a pas de dépassement arithmétique
// tant qu'on reste dans les limites déclarées par le schéma.

// Cas particulier des méthodes amenées à être utilisées en dehors des
// limites pré-fixées par un schéma.

// All fields/props Min/MaxXXX are not actual Min/Max.
// We only use them for overflow testing.
// - If a schema uses table lookup, we MUST use the genuine max month.
//   See CountDaysInYearBeforeMonth() & CountDaysInMonth().
// - Be careful with methods returning a Yemoda.
//   For a "dayOfYear" bigger than expected, GetMonth() will most
//   certainly create parts that are not representable by the system.
//   See GetMonth().

#endregion

/// <summary>
/// Provides facts about <see cref="ICalendricalKernel"/>.
/// <para>Only use this class to test schemas (unbounded calendars).</para>
/// </summary>
public abstract partial class ICalendricalKernelFacts<TSchema, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TSchema : ICalendricalKernel
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalKernelFacts(TSchema schema)
    {
        SchemaUT = schema ?? throw new ArgumentNullException(nameof(schema));
        (MinYear, MaxYear) = schema.SupportedYears.Endpoints;
    }

    /// <summary>
    /// Gets the schema under test.
    /// </summary>
    protected TSchema SchemaUT { get; }

    /// <summary>
    /// Gets a minimum value of the year for which the methods are known not to overflow.
    /// </summary>
    protected int MinYear { get; }

    /// <summary>
    /// Gets a maximum value of the year for which the methods are known not to overflow.
    /// </summary>
    protected int MaxYear { get; }

    protected int MaxDay { get; init; } = Yemoda.MaxDay;
    protected int MaxMonth { get; init; } = Yemoda.MaxMonth;
    protected int MaxDayOfYear { get; init; } = Yedoy.MaxDayOfYear;
}

public partial class ICalendricalKernelFacts<TSchema, TDataSet> // Abstract
{
    [Fact] public abstract void Algorithm_Prop();
    [Fact] public abstract void Family_Prop();
    [Fact] public abstract void PeriodicAdjustments_Prop();
    [Fact] public abstract void SupportedYears_Prop();

    [Fact] public abstract void IsRegular();
}

public partial class ICalendricalKernelFacts<TSchema, TDataSet> // Methods
{
    #region Characteristics

    [Theory, MemberData(nameof(YearInfoData))]
    public void IsLeapYear(YearInfo info)
    {
        // Act
        bool actual = SchemaUT.IsLeapYear(info.Year);
        // Assert
        Assert.Equal(info.IsLeap, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void IsIntercalaryMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        bool actual = SchemaUT.IsIntercalaryMonth(y, m);
        // Assert
        Assert.Equal(info.IsIntercalary, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsIntercalaryDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool actual = SchemaUT.IsIntercalaryDay(y, m, d);
        // Assert
        Assert.Equal(info.IsIntercalary, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsSupplementaryDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool actual = SchemaUT.IsSupplementaryDay(y, m, d);
        // Assert
        Assert.Equal(info.IsSupplementary, actual);
    }

    #endregion
    #region Counting

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountMonthsInYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountMonthsInYear(info.Year);
        // Assert
        Assert.Equal(info.MonthsInYear, actual);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYear(info.Year);
        // Assert
        Assert.Equal(info.DaysInYear, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = SchemaUT.CountDaysInMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }

    #endregion
}

public partial class ICalendricalKernelFacts<TSchema, TDataSet> // Overflows
{
    [Fact]
    public virtual void KernelDoesNotOverflow()
    {
        _ = SchemaUT.IsLeapYear(Int32.MinValue);
        _ = SchemaUT.IsLeapYear(Int32.MaxValue);

        _ = SchemaUT.CountMonthsInYear(Int32.MinValue);
        _ = SchemaUT.CountMonthsInYear(Int32.MaxValue);

        _ = SchemaUT.CountDaysInYear(Int32.MinValue);
        _ = SchemaUT.CountDaysInYear(Int32.MaxValue);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = SchemaUT.IsIntercalaryMonth(Int32.MinValue, m);
            _ = SchemaUT.IsIntercalaryMonth(Int32.MaxValue, m);

            _ = SchemaUT.CountDaysInMonth(Int32.MinValue, m);
            _ = SchemaUT.CountDaysInMonth(Int32.MaxValue, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = SchemaUT.IsIntercalaryDay(Int32.MinValue, m, d);
                _ = SchemaUT.IsIntercalaryDay(Int32.MaxValue, m, d);

                _ = SchemaUT.IsSupplementaryDay(Int32.MinValue, m, d);
                _ = SchemaUT.IsSupplementaryDay(Int32.MaxValue, m, d);
            }
        }
    }

    [Fact] public void IsLeapYear_DoesNotUnderflow() => _ = SchemaUT.IsLeapYear(MinYear);
    [Fact] public void IsLeapYear_DoesNotOverflow() => _ = SchemaUT.IsLeapYear(MaxYear);

    [Fact] public void IsIntercalaryMonth_DoesNotUnderflow() => _ = SchemaUT.IsIntercalaryMonth(MinYear, 1);
    [Fact] public void IsIntercalaryMonth_DoesNotOverflow() => _ = SchemaUT.IsIntercalaryMonth(MaxYear, MaxMonth);

    [Fact] public void IsIntercalaryDay_DoesNotUnderflow() => _ = SchemaUT.IsIntercalaryDay(MinYear, 1, 1);
    [Fact] public void IsIntercalaryDay_DoesNotOverflow() => _ = SchemaUT.IsIntercalaryDay(MaxYear, MaxMonth, MaxDay);

    [Fact] public void IsSupplementaryDay_DoesNotUnderflow() => _ = SchemaUT.IsSupplementaryDay(MinYear, 1, 1);
    [Fact] public void IsSupplementaryDay_DoesNotOverflow() => _ = SchemaUT.IsSupplementaryDay(MaxYear, MaxMonth, MaxDay);

    [Fact] public void CountMonthsInYear_DoesNotUnderflow() => _ = SchemaUT.CountMonthsInYear(MinYear);
    [Fact] public void CountMonthsInYear_DoesNotOverflow() => _ = SchemaUT.CountMonthsInYear(MaxYear);

    [Fact] public void CountDaysInYear_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYear(MinYear);
    [Fact] public void CountDaysInYear_DoesNotOverflow() => _ = SchemaUT.CountDaysInYear(MaxYear);

    [Fact] public void CountDaysInMonth_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonth(MinYear, 1);
    [Fact] public void CountDaysInMonth_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonth(MaxYear, MaxMonth);
}
