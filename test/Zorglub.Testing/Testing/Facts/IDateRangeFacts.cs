// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Facts;

using System.Collections;
using System.Linq;

using Zorglub.Testing.Data;
using Zorglub.Time.Hemerology;
using Zorglub.Time.Hemerology.Scopes;

/// <summary>
/// Provides facts about <see cref="IDateRange{T, TDate}"/>.
/// </summary>
[Obsolete("IDateRange is obsolete.")]
public abstract partial class IDateRangeFacts<TDate, TRange, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : struct, IComparable<TDate>
    where TRange : class, IDateRange<TRange, TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected IDateRangeFacts() { }

    protected abstract TDate GetDate(int y, int m, int d);
    protected abstract TDate GetDate(int y, int doy);

    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "end", Justification = "F# & VB.NET End statement.")]
    protected abstract TRange CreateRange(TDate start, TDate end);
    protected abstract TRange CreateRange(TDate start, int length);

    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", MessageId = "date", Justification = "VB.NET Date.")]
    protected abstract TDate PlusDays(TDate date, int days);

    protected abstract bool CheckCalendar(TRange range);
}

public partial class IDateRangeFacts<TDate, TRange, TDataSet> // Construction
{
    [Fact]
    public void Create_InvalidEnd()
    {
        var start = GetDate(3, 4, 6);
        var end = GetDate(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAoorexn("end", () => CreateRange(start, end));
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(0)]
    public void Create_InvalidLength(int length)
    {
        var start = GetDate(3, 4, 5);
        // Act & Assert
        Assert.ThrowsAoorexn("length", () => CreateRange(start, length));
    }

    [Fact]
    public void Create_Singleton()
    {
        var start = GetDate(3, 4, 5);
        // Act
        var range = CreateRange(start, start);
        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(start, range.End);
        Assert.Equal(1, range.Length);
        Assert.True(CheckCalendar(range));
    }

    [Fact]
    public void Create_Length_Singleton()
    {
        var start = GetDate(3, 4, 5);
        // Act
        var range = CreateRange(start, 1);
        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(start, range.End);
        Assert.Equal(1, range.Length);
        Assert.True(CheckCalendar(range));
    }

    [Fact]
    public void Create()
    {
        var start = GetDate(3, 4, 5);
        var end = GetDate(3, 4, 15);
        // Act
        var range = CreateRange(start, end);
        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
        Assert.Equal(11, range.Length);
        Assert.True(CheckCalendar(range));
    }

    [Fact]
    public void Create_Length()
    {
        var start = GetDate(3, 4, 5);
        TDate end = PlusDays(start, 9);
        // Act
        var range = CreateRange(start, 10);
        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
        Assert.Equal(10, range.Length);
        Assert.True(CheckCalendar(range));
    }
}

public partial class IDateRangeFacts<TDate, TRange, TDataSet> // Methods
{
    #region Contains()

    [Fact]
    public void IsSupersetOf_NullRange()
    {
        var start = GetDate(3, 4, 5);
        var end = GetDate(3, 4, 30);
        var range = CreateRange(start, end);
        // Act & Assert
        Assert.ThrowsAnexn("range", () => range.IsSupersetOf(null!));
    }

    [Theory]
    [InlineData(3, 4, 1, false)]
    [InlineData(3, 4, 2, false)]
    [InlineData(3, 4, 3, false)]
    [InlineData(3, 4, 4, false)]
    [InlineData(3, 4, 5, true)]
    [InlineData(3, 4, 6, true)]
    [InlineData(3, 4, 7, true)]
    [InlineData(3, 4, 8, true)]
    [InlineData(3, 4, 9, true)]
    [InlineData(3, 4, 10, true)]
    [InlineData(3, 4, 11, false)]
    [InlineData(3, 4, 12, false)]
    [InlineData(3, 4, 13, false)]
    public void Contains_Date(int y, int m, int d, bool inRange)
    {
        var start = GetDate(3, 4, 5);
        var end = GetDate(3, 4, 10);
        var range = CreateRange(start, end);
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(inRange, range.Contains(date));
    }

    [Fact]
    public void IsSupersetOf_Range()
    {
        var start = GetDate(3, 4, 5);
        var range1 = CreateRange(start, 1);
        var range2 = CreateRange(start, 2);
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
        Assert.True(range2.IsSupersetOf(range1));
    }

    [Fact]
    public void IsSupersetOf_Range_Overlapping()
    {
        var start1 = GetDate(3, 4, 4);
        var range1 = CreateRange(start1, 2);
        var start2 = GetDate(3, 4, 5);
        var range2 = CreateRange(start2, 2);
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
        Assert.False(range2.IsSupersetOf(range1));
    }

    [Fact]
    public void IsSupersetOf_Range_Disjoint()
    {
        var start1 = GetDate(3, 4, 4);
        var range1 = CreateRange(start1, 2);
        var start2 = GetDate(4, 4, 5);
        var range2 = CreateRange(start2, 2);
        // Act & Assert
        Assert.False(range1.IsSupersetOf(range2));
        Assert.False(range2.IsSupersetOf(range1));
    }

    #endregion

    #region Intersect()

    [Fact]
    public void Intersect_InvalidRange()
    {
        var date = GetDate(3, 4, 5);
        var range = CreateRange(date, 6);
        // Act & Assert
        Assert.ThrowsAnexn("range", () => range.Intersect(null!));
    }

    [Fact]
    public void Intersect_Subrange()
    {
        var start = GetDate(3, 4, 5);
        // range2 contient range1
        var range1 = CreateRange(start, 1);
        var range2 = CreateRange(start, 2);
        // Act & Assert
        Assert.Equal(range1, range2.Intersect(range1));
        Assert.Equal(range1, range1.Intersect(range2));
    }

    [Fact]
    public void Intersect_Overlapping()
    {
        // range2 contient range1.End
        // range1 contient range2.Start
        var range1 = CreateRange(GetDate(3, 4, 1), 4);
        var range2 = CreateRange(GetDate(3, 4, 3), 4);
        var rangeE = CreateRange(GetDate(3, 4, 3), 2);
        // Act & Assert
        Assert.Equal(rangeE, range1.Intersect(range2));
        Assert.Equal(rangeE, range2.Intersect(range1));
    }

    [Fact]
    public void Intersect_Disjoint()
    {
        var date1 = GetDate(3, 4, 5);
        var range1 = CreateRange(date1, 2);
        var date2 = GetDate(4, 4, 5);
        var range2 = CreateRange(date2, 2);
        // Act & Assert
        Assert.Null(range1.Intersect(range2));
    }

    #endregion

    #region Union()

    [Fact]
    public void Union_InvalidRange()
    {
        var date = GetDate(3, 4, 5);
        var range = CreateRange(date, 6);
        // Act & Assert
        Assert.ThrowsAnexn("range", () => range.Union(null!)!);
    }

    [Fact]
    public void Union_Disjoint()
    {
        var date1 = GetDate(3, 4, 5);
        var range1 = CreateRange(date1, 2);
        var date2 = GetDate(4, 4, 5);
        var range2 = CreateRange(date2, 2);
        // Act & Assert
        Assert.Null(range1.Union(range2));
    }

    [Fact]
    public void Union()
    {
        var range1 = CreateRange(GetDate(3, 4, 4), 3);
        var range2 = CreateRange(GetDate(3, 4, 5), 3);
        var rangeE = CreateRange(GetDate(3, 4, 4), 4);
        // Act & Assert
        Assert.Equal(rangeE, range1.Union(range2));
    }

    #endregion
}

public partial class IDateRangeFacts<TDate, TRange, TDataSet> // IEnumerable
{
    [Fact]
    public void Enumerate_IEnumerable()
    {
        var list =
            from i in Enumerable.Range(1, 33)
            where i >= 30
            select GetDate(3, i);
        TRange actual = CreateRange(GetDate(3, 1, 30), 4);
        // Act & Assert
        Assert.Equal(list, ToEnumerable(actual));

        static IEnumerable ToEnumerable(IEnumerable source)
        {
            foreach (object? item in source)
            {
                yield return item;
            }
        }
    }

    [Fact]
    public void Enumerate_Default()
    {
        // On choisit un intervalle à cheval sur deux années.
        var start = GetDate(3, 12, 31);
        var list = new List<TDate> { start }
            .Concat(from i in Enumerable.Range(1, 31) select GetDate(4, i));
        var actual = CreateRange(start, GetDate(4, 1, 31));
        // Act & Assert
        Assert.Equal(list, actual);
    }

    [Fact]
    public void Enumerate_InYear()
    {
        var list = from i in Enumerable.Range(1, 32)
                   where i >= 2
                   select GetDate(3, i);
        var actual = CreateRange(GetDate(3, 1, 2), 31);
        // Act & Assert
        Assert.Equal(list, actual);
    }

    [Fact]
    public void Enumerate_InMonth()
    {
        var list = from i in Enumerable.Range(1, 11)
                   where i >= 2
                   select GetDate(3, 4, i);
        var actual = CreateRange(GetDate(3, 4, 2), 10);
        // Act & Assert
        Assert.Equal(list, actual);
    }
}

public partial class IDateRangeFacts<TDate, TRange, TDataSet> // IEquatable
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetHashCode_SanityChecks(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Autrement, on ne pourrait pas créer range1/2.
        // WRONG: ShortScope est lié à DateRange, pas à IDateRange.
        if (y >= ShortScope.MaxYear) { return; }

        var start = GetDate(y, m, d);
        var range = CreateRange(start, 29);
        // Act & Assert
        Assert.Equal(range.GetHashCode(), range.GetHashCode());
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_Referential(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Autrement, on ne pourrait pas créer range1/2.
        if (y >= ShortScope.MaxYear) { return; }

        var start = GetDate(y, m, d);
        // Act
        var range = CreateRange(start, 29);
        // Assert
        Assert.True(range.Equals(range));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_StructuralA(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Autrement, on ne pourrait pas créer range1/2.
        if (y >= ShortScope.MaxYear) { return; }

        var start = GetDate(y, m, d);
        // Act
        var range1 = CreateRange(start, 29);
        var range2 = CreateRange(start, 29);
        // Assert
        Assert.True(range1.Equals(range2));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_StructuralB(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Autrement, on ne pourrait pas créer range1/2.
        if (y >= ShortScope.MaxYear) { return; }

        var start = GetDate(y, m, d);
        // Act
        var range1 = CreateRange(start, 1);
        var range2 = CreateRange(start, 2);
        // Assert
        Assert.False(range1.Equals(range2));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Equals_StructuralC(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Autrement, on ne pourrait pas créer range1/2.
        if (y >= ShortScope.MaxYear) { return; }

        var start = GetDate(y, m, d);
        TDate other = PlusDays(start, -1);
        // Act
        var range1 = CreateRange(start, 1);
        var range2 = CreateRange(other, 1);
        // Assert
        Assert.False(range1.Equals(range2));
    }

    [Fact]
    public void Equals_Null()
    {
        var start = GetDate(3, 4, 5);
        var range = CreateRange(start, 29);
        // Act & Assert
        // The order of statements is important otherwise Equals(null)
        // will fool the compiler, it will believe that "range" is null.
        // Simpler: disable nullables.
#nullable disable
        Assert.True(range.Equals((object)range));
        Assert.False(range.Equals((TRange)null!));
        Assert.False(range.Equals((object)null!));
        Assert.False(range.Equals(new object()));
#nullable restore
    }
}
