// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Zorglub.Time.Core;
using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Core.Utilities;

using static Zorglub.Time.Extensions.Unboxing;

public static class WideCatalogTests
{
    public const string GregorianKey = "Wide Gregorian";
    public const string JulianKey = "Wide Julian";

    private static readonly JulianSchema s_JulianSchema = new();
    private static readonly GregorianSchema s_GregorianSchema = new();

    private static WideCalendar? s_Gregorian;
    public static WideCalendar Gregorian => s_Gregorian ??= InitGregorian();

    private static WideCalendar? s_Julian;
    public static WideCalendar Julian => s_Julian ??= InitJulian();

    [Pure]
    private static WideCalendar InitGregorian()
    {
        var chr = WideCatalog.Add(GregorianKey, s_GregorianSchema, DayZero.NewStyle, widest: true);
        Interlocked.CompareExchange(ref s_Gregorian, chr, null);
        return s_Gregorian!;
    }

    [Pure]
    private static WideCalendar InitJulian()
    {
        var chr = WideCatalog.Add(JulianKey, s_JulianSchema, DayZero.OldStyle, widest: true);
        Interlocked.CompareExchange(ref s_Julian, chr, null);
        return s_Julian!;
    }

#if false
    // Si on doit revenir en arrière, ne pas oublier de remplacer dans le projet
    // tous les appels à ArmenianCalendar.Instance par CalendarCatalogTests.Armenian.
    // Idem avec CopticCalendar.
    //
    // Ne pas effacer. Peut-être bien qu'on devra faire qque chose de similaire
    // pour pouvoir tester les calendriers "paresseux" de type WideCalendar.

    private static readonly Calendar s_GetCalendarFirstCall;
    private static readonly Calendar s_GetCalendarSecondCall;
    public static readonly CopticCalendar CopticCalendar;

    private static readonly Calendar s_GetCalendarUncheckedFirstCall;
    private static readonly Calendar s_GetCalendarUncheckedSecondCall;
    public static readonly ArmenianCalendar ArmenianCalendar;

#pragma warning disable CA1810 // Initialize reference type static fields inline (Performance) 👈 Tests

    static CalendarCatalogTests()
    {
        // VERY IMPORTANT: this is the ONLY place in the test project where
        // we ever call ArmenianCalendar.Instance or CopticCalendar.Instance.
        // Methods GetCalendarXXX() get called in CalendarTests<>, but AFTER
        // CalendarRegistryFixture has been initialized (instance test methods
        // and CalendarUT there is set to the "Proleptic" prop defined here).
        // Outside this project, GetCalendarXXX() might be called via a
        // calendrical object, but it is OK because we only test these objs
        // with the Gregorian calendar, not the Armenian or Coptic calendars.

        // This is fragile and not very "Unit Test"y, but it helps us to
        // achieve full test coverage.

        // Pristine situation:
        // GetCalendar() or GetCalendarUnchecked()
        //   -> GetCalendarUncached()
        //   -> call XXXCalendar.Proleptic
        //   -> call ctor()
        //   -> add instance to the registry

        // Requirement: the Armenian calendar has not yet been initialized.
        // 1. First call to GetCalendar(): GetCalendarUncached().
        s_GetCalendarFirstCall = CalendarCatalog.GetSystemCalendar(CalendarId.Armenian);
        // 2. Second call to GetCalendar() or GetCalendarUnchecked(): table lookup.
        s_GetCalendarSecondCall = CalendarCatalog.GetSystemCalendar(CalendarId.Armenian);
        // 3. Static prop already initialized during step 1.
        ArmenianCalendar = ArmenianCalendar.Instance;

        // Requirement: the Coptic calendar has not yet been initialized.
        // 1. First call to GetCalendarUnchecked(): GetCalendarUncached().
        s_GetCalendarUncheckedFirstCall = CalendarCatalog.GetCalendarUnchecked((int)CalendarId.Coptic);
        // 2. Second call to GetCalendarUnchecked() or GetCalendar(): table lookup.
        s_GetCalendarUncheckedSecondCall = CalendarCatalog.GetCalendarUnchecked((int)CalendarId.Coptic);
        // 3. Static prop already initialized during step 1.
        CopticCalendar = CopticCalendar.Instance;
    }

#pragma warning restore CA1810

    [Fact]
    public static void GetSystemCalendar_Pristine()
    {
        // Arrange
        var chr = ArmenianCalendar;
        // Assert
        Assert.Same(chr, s_GetCalendarFirstCall);
        Assert.Same(chr, s_GetCalendarSecondCall);
    }

    [Fact]
    public static void GetCalendarUnchecked_Pristine()
    {
        // Arrange
        var chr = CopticCalendar;
        // Assert
        Assert.Same(chr, s_GetCalendarUncheckedFirstCall);
        Assert.Same(chr, s_GetCalendarUncheckedSecondCall);
    }

#endif

    #region CurrentKeys

    [Fact]
    public static void CurrentKeys_DoesNotContainUnknownKey() =>
        Assert.DoesNotContain("UnknownKey", WideCatalog.Keys);

    [Theory(Skip = "???")]
    [InlineData(GregorianKey)]
    [InlineData(JulianKey)]
    public static void CurrentKeys(string key) => Assert.Contains(key, WideCatalog.Keys);

    #endregion
    #region Add()

    [Fact]
    public static void Add_NullKey() =>
        Assert.ThrowsAnexn("name",
            () => WideCatalog.Add(null!, s_GregorianSchema, default, false));

    [Fact(Skip = "???")]
    public static void Add_KeyAlreadyExists() =>
        Assert.Throws<ArgumentException>("key",
            () => WideCatalog.Add(GregorianKey, s_GregorianSchema, default, false));

    [Fact]
    public static void Add_InvalidSchema()
    {
        // Arrange
        string key = "key";
        // Act & Assert
        Assert.ThrowsAnexn("schema",
            () => WideCatalog.Add(key, null!, default, false));
        OnKeyNotSet(key);
    }

    [Fact]
    public static void Add()
    {
        // Arrange
        string key = "Add";
        var epoch = DayZero.NewStyle;
        // Act
        var chr = WideCatalog.Add(key, s_GregorianSchema, epoch, false);
        // Assert
        OnKeySet(key, epoch, chr);
    }

    [Fact]
    public static void Add_Box()
    {
        // Arrange
        string key = "Add_Box";
        var epoch = DayZero.NewStyle;
        // Act
        var chr = (from x in GregorianSchema.GetInstance()
                   select WideCatalog.Add(key, x, epoch, false)
                   ).Unbox();
        // Assert
        OnKeySet(key, epoch, chr);
    }

    #endregion
    #region TryAdd()

    [Fact]
    public static void TryAdd_NullKey() =>
        Assert.ThrowsAnexn("key",
            () => WideCatalog.TryAdd(
                null!, s_GregorianSchema, default, false, out _));

    [Fact(Skip = "???")]
    public static void TryAdd_KeyAlreadyExists()
    {
        // Act
        // NB: on utilise volontairement une epoch et un schéma différents.
        bool created = WideCatalog.TryAdd(
            GregorianKey,
            s_JulianSchema,
            DayZero.OldStyle,
            false,
            out WideCalendar? calendar);
        // Assert
        Assert.False(created);
        //Assert.Same(Gregorian, calendar);
        Assert.Null(calendar);
    }

    [Fact]
    public static void TryAdd_InvalidSchema()
    {
        // Arrange
        string key = "key";
        // Act & Assert
        Assert.ThrowsAnexn("schema",
            () => WideCatalog.TryAdd(key, null!, default, false, out _));
        OnKeyNotSet(key);
    }

    [Fact]
    public static void TryAdd()
    {
        // Arrange
        string key = "TryAdd";
        var epoch = DayZero.NewStyle;
        // Act
        bool created = WideCatalog.TryAdd(
            key, s_GregorianSchema, epoch, false, out WideCalendar? calendar);
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);
    }

    [Fact]
    public static void TryAdd_EmptyKey()
    {
        // Arrange
        string key = String.Empty;
        var epoch = DayZero.NewStyle;
        // Act
        bool created = WideCatalog.TryAdd(
            key, s_GregorianSchema, epoch, false, out WideCalendar? calendar);
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);
    }

    [Fact]
    public static void TryAdd_Box()
    {
        // Arrange
        string key = "TryAdd_Box";
        var epoch = DayZero.NewStyle;
        bool created = false;
        // Act
        var calendar = GregorianSchema.GetInstance().Select(TryAdd).Unbox();
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);

        WideCalendar? TryAdd(GregorianSchema x)
        {
            created = WideCatalog.TryAdd(key, x, epoch, false, out var chr);

            return chr;
        }
    }

    #endregion
    #region GetCalendar()

    [Fact]
    public static void GetCalendar_InvalidKey() =>
        Assert.Throws<KeyNotFoundException>(() => WideCatalog.GetCalendar("UnknownKey"));

    [Fact]
    //[Fact(Skip = "???")]
    public static void GetCalendar()
    {
        Assert.Same(Gregorian, WideCatalog.GetCalendar(GregorianKey));
        Assert.Same(Julian, WideCatalog.GetCalendar(JulianKey));
    }

    #endregion
    #region GetCalendarUnchecked()

    [Fact]
    public static void GetCalendarUnchecked_InvalidId() =>
        Assert.Null(WideCatalog.GetCalendarUnchecked(Byte.MaxValue));

    [Fact]
    public static void GetCalendarUnchecked_Zero() =>
        Assert.Equal(WideCalendar.Gregorian, WideCatalog.GetCalendarUnchecked(0));

    [Fact]
    //[Fact(Skip = "???")]
    public static void GetCalendarUnchecked()
    {
        Assert.Same(Gregorian, WideCatalog.GetCalendarUnchecked(Gregorian.Id));
        Assert.Same(Julian, WideCatalog.GetCalendarUnchecked(Julian.Id));
    }

    #endregion

    //private static TryFunc<ICalendricalSchema, WideCalendar?> TryCreate(string key) =>
    //    (ICalendricalSchema schema, out WideCalendar? result) =>
    //    {
    //        return WideCatalog.TryAdd(key, schema, default, false, out result);
    //    };

    private static void OnKeyNotSet(string key)
    {
        Assert.DoesNotContain(key, WideCatalog.Keys);
        Assert.Throws<KeyNotFoundException>(() => WideCatalog.GetCalendar(key));
    }

    private static void OnKeySet(string key, DayNumber epoch, WideCalendar? calendar)
    {
        OnKeySetCore(key, epoch, calendar);
        MaybeTestInitializationThreshold();
    }

    private static void OnKeySetCore(string key, DayNumber epoch, WideCalendar? calendar)
    {
        Assert.NotNull(calendar);
        Assert.Equal(key, calendar!.Key);
        Assert.Equal(epoch, calendar.Epoch);
        Assert.Contains(key, WideCatalog.Keys);
        Assert.Same(calendar, WideCatalog.GetCalendar(key));
    }

    private static int s_Count = 2;

    // This is fragile and really not "Unit Test"y, but I haven't found
    // another way to test the 256 limit.
    // FIXME: see CalendarCatalogTests.MaybeTestInitializationThreshold().
    private static void MaybeTestInitializationThreshold()
    {
        const int MaxMaxCount = 1 + WideCatalog.MaxId;
        const int MaxCount = 8;

        if (Interlocked.Increment(ref s_Count) != MaxCount) { return; }
        Interlocked.Increment(ref s_Count);

        Assert.Equal(MaxCount, WideCatalog.Keys.Count);

        // Create as much calendars as we are allowed to.
        Parallel.ForEach(
            Enumerable.Range(1 + MaxCount, MaxMaxCount - MaxCount),
            i =>
            {
                string key = $"Key-{i}";
                var epoch = DayZero.NewStyle + i;

                bool created = WideCatalog.TryAdd(
                    key, s_GregorianSchema, epoch, false, out WideCalendar? calendar);

                Assert.True(created);
                OnKeySetCore(key, epoch, calendar);
            }
        );

        // Check CurrentKeys.
        var allKeys = WideCatalog.Keys;
        Assert.Equal(MaxMaxCount, allKeys.Count);

        // Check GetCalendar().
        var calendarsFromKey = allKeys.Select(WideCatalog.GetCalendar).Distinct();
        foreach (var calendar in calendarsFromKey) { Assert.NotNull(calendar); }
        Assert.Equal(MaxMaxCount, calendarsFromKey.Count());

        // Check GetCalendar().
        var calendarsFromId = (from i in Enumerable.Range(0, 256)
                               select WideCatalog.GetCalendarUnchecked((byte)i)).Distinct();
        foreach (var calendar in calendarsFromId) { Assert.NotNull(calendar); }
        Assert.Equal(MaxMaxCount, calendarsFromId.Count());

        // Any subsequent call to an initialization method should fail.
        string key = "Key-OVERFLOW";

        Assert.Overflows(
            () => WideCatalog.Add(key, s_GregorianSchema, default, false));
        OnKeyNotSet(key);
        Assert.Throws<ArgumentException>(
            () => WideCatalog.Add(GregorianKey, s_GregorianSchema, default, false));

        Assert.False(WideCatalog.TryAdd(key, s_GregorianSchema, default, false, out _));
        OnKeyNotSet(key);
        Assert.False(WideCatalog.TryAdd(GregorianKey, s_GregorianSchema, default, false, out _));

        Assert.Overflows(
            () =>
            {
                var q = from x in GregorianSchema.GetInstance()
                        select WideCatalog.Add(key, x, default, false);
                return q.Unbox();
            });
        OnKeyNotSet(key);

        Assert.Empty(GregorianSchema.GetInstance().Select(TryCreate(key)));
        OnKeyNotSet(key);

        // The list of keys didn't change.
        Assert.Equal(allKeys, WideCatalog.Keys);

        static Func<ICalendricalSchema, WideCalendar?> TryCreate(string key) =>
            (ICalendricalSchema schema) =>
            {
                _ = WideCatalog.TryAdd(key, schema, default, false, out var chr);
                return chr;
            };
    }
}
