// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using global::Samples;

using Zorglub.Time.Core.Schemas;

public static class CalendarCatalogTests
{
    public const string MyJulianKey = "User Julian";
    public const string MyGregorianKey = "User Gregorian";

    public static readonly Calendar MyJulian = CalendarCatalog.Add(
        MyJulianKey, new JulianSchema(), DayZero.OldStyle, proleptic: false);

    public static readonly Calendar MyGregorian = CalendarCatalog.Add(
        MyGregorianKey, new GregorianSchema(), DayZero.NewStyle, proleptic: false);

    // We want the two previous static fields to be initialized before anything else.
    static CalendarCatalogTests() { }

    [Theory]
    [InlineData(MyGregorianKey)]
    [InlineData(MyJulianKey)]
    public static void Keys_UserKey(string key) =>
        Assert.Contains(key, CalendarCatalog.Keys);

    [Fact]
    public static void GetCalendar_UserKey()
    {
        Assert.Same(MyGregorian, CalendarCatalog.GetCalendar(MyGregorianKey));
        Assert.Same(MyJulian, CalendarCatalog.GetCalendar(MyJulianKey));
    }

    [Fact]
    public static void TryGetCalendar_UserKey()
    {
        Assert.True(CalendarCatalog.TryGetCalendar(MyGregorianKey, out var gr));
        Assert.Same(MyGregorian, gr);

        Assert.True(CalendarCatalog.TryGetCalendar(MyJulianKey, out var ju));
        Assert.Same(MyJulian, ju);
    }

    [Theory]
    [InlineData((int)Cuid.MaxSystem + 1)]
    [InlineData((int)Cuid.MinUser)]
    [InlineData((int)Cuid.Max)]
    [InlineData((int)Cuid.Invalid)]
    public static void GetSystemCalendar_InvalidId(int id) =>
        Assert.ThrowsAoorexn("id", () => CalendarCatalog.GetSystemCalendar((CalendarId)id));

    [Fact]
    public static void GetCalendarUnchecked_UserId()
    {
        Assert.Same(MyGregorian, CalendarCatalog.GetCalendarUnchecked((int)MyGregorian.Id));
        Assert.Same(MyJulian, CalendarCatalog.GetCalendarUnchecked((int)MyJulian.Id));
    }

    #region GetOrAdd()

    [Fact]
    public static void GetOrAdd_NullKey() =>
        Assert.ThrowsAnexn("key",
            () => CalendarCatalog.GetOrAdd(null!, new GregorianSchema(), default, false));

    [Fact]
    public static void GetOrAdd_KeyAlreadyExists()
    {
        // Act
        // NB: on utilise volontairement une epoch et un schéma différents.
        var chr = CalendarCatalog.GetOrAdd(MyGregorianKey, new JulianSchema(), DayZero.OldStyle, false);
        // Assert
        Assert.Equal(MyGregorian, chr);
    }

    [Fact]
    public static void GetOrAdd_InvalidSchema()
    {
        string key = "key";
        // Act & Assert
        Assert.ThrowsAnexn("schema", () => CalendarCatalog.GetOrAdd(key, null!, default, false));
        OnKeyNotSet(key);
    }

    [Fact]
    public static void GetOrAdd()
    {
        string key = "GetOrAdd";
        var epoch = DayZero.NewStyle;
        // Act
        var chr = CalendarCatalog.GetOrAdd(key, new GregorianSchema(), epoch, false);
        // Assert
        OnKeySet(key, epoch, chr);
    }

    #endregion
    #region Add()

    [Fact]
    public static void Add_NullKey() =>
        Assert.ThrowsAnexn("key",
            () => CalendarCatalog.Add(null!, new GregorianSchema(), default, false));

    [Fact]
    public static void Add_KeyAlreadyExists() =>
        Assert.Throws<ArgumentException>("key",
            () => CalendarCatalog.Add(MyGregorianKey, new GregorianSchema(), default, false));

    [Fact]
    public static void Add_InvalidSchema()
    {
        string key = "key";
        // Act & Assert
        Assert.ThrowsAnexn("schema",
            () => CalendarCatalog.Add(key, null!, default, false));
        OnKeyNotSet(key);
    }

    [Fact]
    public static void Add()
    {
        string key = "Add";
        var epoch = DayZero.NewStyle;
        // Act
        var chr = CalendarCatalog.Add(key, new GregorianSchema(), epoch, false);
        // Assert
        OnKeySet(key, epoch, chr);
    }

    [Fact]
    public static void Add_Box()
    {
        string key = "Add_Box";
        var epoch = DayZero.NewStyle;
        // Act
        var chr = GregorianSchema.GetInstance().CreateCalendar(key, epoch);
        // Assert
        OnKeySet(key, epoch, chr);
    }

    #endregion
    #region TryAdd()

    [Fact]
    public static void TryAdd_NullKey() =>
        Assert.ThrowsAnexn("key",
            () => CalendarCatalog.TryAdd(
                null!, new GregorianSchema(), default, false, out _));

    [Fact]
    public static void TryAdd_KeyAlreadyExists()
    {
        // Act
        // NB: on utilise volontairement une epoch et un schéma différents.
        bool created = CalendarCatalog.TryAdd(
            MyGregorianKey,
            new JulianSchema(),
            DayZero.OldStyle,
            false,
            out Calendar? calendar);
        // Assert
        Assert.False(created);
        Assert.Null(calendar);
    }

    [Fact]
    public static void TryAdd_InvalidSchema()
    {
        string key = "key";
        // Act & Assert
        Assert.ThrowsAnexn("schema",
            () => CalendarCatalog.TryAdd(key, null!, default, false, out _));
        OnKeyNotSet(key);
    }

    [Fact]
    public static void TryAdd()
    {
        string key = "TryAdd";
        var epoch = DayZero.NewStyle;
        // Act
        bool created = CalendarCatalog.TryAdd(
            key, new GregorianSchema(), epoch, false, out Calendar? calendar);
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);
    }

    [Fact]
    public static void TryAdd_EmptyKey()
    {
        string key = String.Empty;
        var epoch = DayZero.NewStyle;
        // Act
        bool created = CalendarCatalog.TryAdd(
            key, new GregorianSchema(), epoch, false, out Calendar? calendar);
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);
    }

    [Fact]
    public static void TryAdd_Box()
    {
        string key = "TryAdd_Box";
        var epoch = DayZero.NewStyle;
        // Act
        bool created = GregorianSchema.GetInstance()
            .TryCreateCalendar(key, epoch, out Calendar? calendar);
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);
    }

    #endregion

    [Theory, MemberData(nameof(EnumDataSet.CalendarIdData), MemberType = typeof(EnumDataSet))]
    public static void GetCalendarXXX_AreAllTheSame(CalendarId id)
    {
        // Act
        var call1 = CalendarCatalog.GetSystemCalendar(id);
        var call2 = CalendarCatalog.GetCalendar(id.ToCalendarKey());
        var call3 = CalendarCatalog.GetCalendarUnchecked((int)id);
        ref readonly var call4 = ref CalendarCatalog.GetCalendarUnsafe((int)id);
        // Assert
        Assert.Same(call1, call2);
        Assert.Same(call1, call3);
        Assert.Same(call1, call4);
    }

    #region Helpers

    private static void OnKeyNotSet(string key)
    {
        Assert.DoesNotContain(key, CalendarCatalog.Keys);
        Assert.Throws<KeyNotFoundException>(() => CalendarCatalog.GetCalendar(key));
    }

    private static void OnKeySet(string key, DayNumber epoch, Calendar? calendar)
    {
        OnKeySetCore(key, epoch, calendar);
        MaybeTestInitializationThreshold();
    }

    private static void OnKeySetCore(string key, DayNumber epoch, Calendar? calendar)
    {
        Assert.NotNull(calendar);
        Assert.Equal(key, calendar!.Key);
        Assert.Equal(epoch, calendar.Epoch);
        Assert.Contains(key, CalendarCatalog.Keys);
        Assert.Same(calendar, CalendarCatalog.GetCalendar(key));
    }

    private static int s_OnKeySetCount;

    // This is fragile and really not "Unit Test"y, but I haven't found
    // another way to test the limit of 64 user calendars.
    // FIXME: can we check that TestInitializationThreshold did actually run?
    private static void MaybeTestInitializationThreshold()
    {
        int
            TotalSystemCount = 1 + (int)Cuid.MaxSystem, // 7
            /* Calendars initialized by CalendarZoo */
            TotalCompendiumCount = 8,                   // 8
            LastOnKeySetCount = 6,                      // 6 calls to OnKeySet().
            /* 2 calendars initialized in this project: Gregorian and Julian. */
            CurrentUserCount = LastOnKeySetCount + TotalCompendiumCount + 2,   // 16
            CurrentCount = CurrentUserCount + TotalSystemCount, // 23
            TotalCount = CalendarCatalog.MaxNumberOfUserCalendars + TotalSystemCount;   // 71

        if (Interlocked.Increment(ref s_OnKeySetCount) != LastOnKeySetCount) { return; }
        // La ligne suivante est très certainement inutile.
        Interlocked.Increment(ref s_OnKeySetCount);

        Assert.Equal(7, TotalSystemCount);
        Assert.Equal(16, CurrentUserCount);
        Assert.Equal(23, CurrentCount);
        Assert.Equal(71, TotalCount);

        // If not yet initialized, do it.
        _ = CalendarZoo.Tropicalia;
        _ = CalendarZoo.Egyptian;
        _ = CalendarZoo.FrenchRepublican;
        _ = CalendarZoo.InternationalFixed;
        _ = CalendarZoo.Persian2820;
        _ = CalendarZoo.Positivist;
        _ = CalendarZoo.RevisedWorld;
        _ = CalendarZoo.World;

        Assert.Equal(CurrentCount, CalendarCatalog.Keys.Count);
        Assert.Equal(CurrentCount, CalendarCatalog.GetAllCalendars().Count);
        Assert.Equal(CurrentUserCount, CalendarCatalog.GetUserCalendars().Count);

        // Create as much calendars as we are allowed to.
        Parallel.ForEach(
            Enumerable.Range(0, CalendarCatalog.MaxNumberOfUserCalendars - CurrentUserCount),
            i =>
            {
                string key = $"Key-{i}";
                var epoch = DayZero.NewStyle;

                bool added = CalendarCatalog.TryAdd(
                    key, new GregorianSchema(), DayZero.NewStyle, false, out Calendar? calendar);

                Assert.True(added);
                OnKeySetCore(key, epoch, calendar);
            }
        );

        var keys = CalendarCatalog.Keys;
        Assert.Equal(TotalCount, keys.Count);
        Assert.Equal(TotalCount, CalendarCatalog.GetAllCalendars().Count);
        Assert.Equal(CalendarCatalog.MaxNumberOfUserCalendars, CalendarCatalog.GetUserCalendars().Count);

        // Check GetCalendar(key).
        var calendarsFromKey = keys.Select(CalendarCatalog.GetCalendar).Distinct();
        foreach (var calendar in calendarsFromKey) { Assert.NotNull(calendar); }
        Assert.Equal(TotalCount, calendarsFromKey.Count());

        // Check GetCalendarUnchecked(Cuid), only the user-defined calendars.
        var userCalendarsFromId = (
            from i in Enumerable.Range((int)Cuid.MinUser, CalendarCatalog.MaxNumberOfUserCalendars)
            select CalendarCatalog.GetCalendarUnchecked(i)).Distinct();
        foreach (var calendar in userCalendarsFromId) { Assert.NotNull(calendar); }
        Assert.Equal(CalendarCatalog.MaxNumberOfUserCalendars, userCalendarsFromId.Count());

        // Any subsequent call to an initialization method should fail.
        string key = "Key-OVERFLOW";

        Assert.Overflows(
            () => CalendarCatalog.Add(key, new GregorianSchema(), default, false));
        OnKeyNotSet(key);
        Assert.Overflows(
            () => CalendarCatalog.Add(MyGregorianKey, new GregorianSchema(), default, false));

        Assert.False(CalendarCatalog.TryAdd(key, new GregorianSchema(), default, false, out _));
        OnKeyNotSet(key);
        Assert.False(CalendarCatalog.TryAdd(MyGregorianKey, new GregorianSchema(), default, false, out _));

        Assert.Overflows(
            () => GregorianSchema.GetInstance().CreateCalendar(key, default));
        OnKeyNotSet(key);

        Assert.False(GregorianSchema.GetInstance().TryCreateCalendar(key, default, out Calendar? _));
        OnKeyNotSet(key);

        // The list of keys didn't change.
        Assert.Equal(keys, CalendarCatalog.Keys);
        Assert.Equal(TotalCount, CalendarCatalog.GetAllCalendars().Count);
        Assert.Equal(CalendarCatalog.MaxNumberOfUserCalendars, CalendarCatalog.GetUserCalendars().Count);
    }

    #endregion
}
