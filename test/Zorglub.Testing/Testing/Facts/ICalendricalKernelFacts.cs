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
public abstract partial class ICalendricalKernelFacts<TKernel, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TKernel : ICalendricalKernel
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalKernelFacts(TKernel schema)
    {
        SchemaUT = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    /// <summary>
    /// Gets the schema under test.
    /// </summary>
    protected TKernel SchemaUT { get; }
}

public partial class ICalendricalKernelFacts<TKernel, TDataSet> // Abstract
{
    [Fact] public abstract void Algorithm_Prop();
    [Fact] public abstract void Family_Prop();
    [Fact] public abstract void PeriodicAdjustments_Prop();
    [Fact] public abstract void SupportedYears_Prop();

    [Fact] public abstract void IsRegular();
}

public partial class ICalendricalKernelFacts<TKernel, TDataSet> // Methods
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
