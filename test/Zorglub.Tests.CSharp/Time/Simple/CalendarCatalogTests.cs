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
    public const string MyGregorianKey = "User Gregorian";

    public static readonly Calendar MyGregorian = CalendarCatalog.Add(
        MyGregorianKey, new GregorianSchema(), DayZero.NewStyle, proleptic: false);

    static CalendarCatalogTests() { }

    [Theory]
    [InlineData((int)Cuid.MaxSystem + 1)]
    [InlineData((int)Cuid.MinUser)]
    [InlineData((int)Cuid.Max)]
    [InlineData((int)Cuid.Invalid)]
    public static void GetSystemCalendar_InvalidId(int id) =>
        Assert.ThrowsAoorexn("id", () => CalendarCatalog.GetSystemCalendar((CalendarId)id));

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
}
