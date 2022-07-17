// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology;

using Zorglub.Time.Core.Schemas;
using Zorglub.Time.Hemerology.Scopes;

using static Zorglub.Time.Extensions.Unboxing;

public static class ZCatalogTests
{
    public const string GregorianKey = "Zorglub Gregorian";
    public const string JulianKey = "Zorglub Julian";

    public static readonly ZCalendar Gregorian =
        ZCatalog.Add(
            GregorianKey,
            MinMaxYearScope.WithMaximalRange(new GregorianSchema(), DayZero.NewStyle, onOrAfterEpoch: false));

    public static readonly ZCalendar Julian =
        ZCatalog.Add(
            JulianKey,
            MinMaxYearScope.WithMaximalRange(new JulianSchema(), DayZero.OldStyle, onOrAfterEpoch: true));

    // We want the two previous static fields to be initialized before anything else.
    static ZCatalogTests() { }

#if false
    // Si on doit revenir en arrière, ne pas oublier de remplacer dans le projet
    // tous les appels à ArmenianCalendar.Instance par CalendarCatalogTests.Armenian.
    // Idem avec CopticCalendar.
    //
    // Ne pas effacer. Peut-être bien qu'on devra faire qque chose de similaire
    // pour pouvoir tester les calendriers "paresseux" de type ZCalendar.

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
        var chr = ArmenianCalendar;
        // Assert
        Assert.Same(chr, s_GetCalendarFirstCall);
        Assert.Same(chr, s_GetCalendarSecondCall);
    }

    [Fact]
    public static void GetCalendarUnchecked_Pristine()
    {
        var chr = CopticCalendar;
        // Assert
        Assert.Same(chr, s_GetCalendarUncheckedFirstCall);
        Assert.Same(chr, s_GetCalendarUncheckedSecondCall);
    }

#endif

    #region CurrentKeys

    [Fact]
    public static void CurrentKeys_DoesNotContainUnknownKey() =>
        Assert.DoesNotContain("UnknownKey", ZCatalog.Keys);

    [Theory]
    [InlineData(GregorianKey)]
    [InlineData(JulianKey)]
    public static void CurrentKeys(string key) => Assert.Contains(key, ZCatalog.Keys);

    #endregion
    #region Add()

    [Fact]
    public static void Add_NullKey() =>
        Assert.ThrowsAnexn("name",
            () => ZCatalog.Add(null!, MinMaxYearScope.WithMaximalRange(new GregorianSchema(), default, false)));

    [Fact]
    public static void Add_KeyAlreadyExists() =>
        Assert.Throws<ArgumentException>("key",
            () => ZCatalog.Add(GregorianKey, MinMaxYearScope.WithMaximalRange(new GregorianSchema(), default, false)));

    //[Fact]
    //public static void Add_InvalidSchema()
    //{
    //    string key = "key";
    //    // Act & Assert
    //    Assert.ThrowsAnexn("schema",
    //        () => ZCatalog.Add(key, null!, default, false));
    //    OnKeyNotSet(key);
    //}

    [Fact]
    public static void Add()
    {
        string key = "Add";
        var epoch = DayZero.NewStyle;
        // Act
        var chr = ZCatalog.Add(key, MinMaxYearScope.WithMaximalRange(new GregorianSchema(), epoch, false));
        // Assert
        OnKeySet(key, epoch, chr);
    }

    [Fact]
    public static void Add_Box()
    {
        string key = "Add_Box";
        var epoch = DayZero.NewStyle;
        // Act
        var chr = (from x in GregorianSchema.GetInstance()
                   select ZCatalog.Add(key, MinMaxYearScope.WithMaximalRange(x, epoch, false))
                   ).Unbox();
        // Assert
        OnKeySet(key, epoch, chr);
    }

    #endregion
    #region TryAdd()

    [Fact]
    public static void TryAdd_NullKey() =>
        Assert.ThrowsAnexn("key",
            () => ZCatalog.TryAdd(
                null!, MinMaxYearScope.WithMaximalRange(new GregorianSchema(), default, false), out _));

    [Fact]
    public static void TryAdd_KeyAlreadyExists()
    {
        // Act
        // NB: on utilise volontairement une epoch et un schéma différents.
        bool created = ZCatalog.TryAdd(
            GregorianKey,
            MinMaxYearScope.WithMaximalRange(new JulianSchema(), DayZero.OldStyle, false),
            out ZCalendar? calendar);
        // Assert
        Assert.False(created);
        //Assert.Same(Gregorian, calendar);
        Assert.Null(calendar);
    }

    //[Fact]
    //public static void TryAdd_InvalidSchema()
    //{
    //    string key = "key";
    //    // Act & Assert
    //    Assert.ThrowsAnexn("schema",
    //        () => ZCatalog.TryAdd(key, null!, default, false, out _));
    //    OnKeyNotSet(key);
    //}

    [Fact]
    public static void TryAdd()
    {
        string key = "TryAdd";
        var epoch = DayZero.NewStyle;
        // Act
        bool created = ZCatalog.TryAdd(
            key,
            MinMaxYearScope.WithMaximalRange(new GregorianSchema(), epoch, false),
            out ZCalendar? calendar);
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
        bool created = ZCatalog.TryAdd(
            key,
            MinMaxYearScope.WithMaximalRange(new GregorianSchema(), epoch, false),
            out ZCalendar? calendar);
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);
    }

    [Fact]
    public static void TryAdd_Box()
    {
        string key = "TryAdd_Box";
        var epoch = DayZero.NewStyle;
        bool created = false;
        // Act
        var calendar = GregorianSchema.GetInstance().Select(TryAdd).Unbox();
        // Assert
        Assert.True(created);
        OnKeySet(key, epoch, calendar);

        ZCalendar? TryAdd(GregorianSchema x)
        {
            created = ZCatalog.TryAdd(key, MinMaxYearScope.WithMaximalRange(x, epoch, false), out var chr);

            return chr;
        }
    }

    #endregion
    #region GetCalendar()

    [Fact]
    public static void GetCalendar_InvalidKey() =>
        Assert.Throws<KeyNotFoundException>(() => ZCatalog.GetCalendar("UnknownKey"));

    [Fact]
    public static void GetCalendar()
    {
        Assert.Same(Gregorian, ZCatalog.GetCalendar(GregorianKey));
        Assert.Same(Julian, ZCatalog.GetCalendar(JulianKey));
    }

    #endregion
    #region GetCalendarUnchecked()

    [Fact]
    public static void GetCalendarUnchecked_InvalidId() =>
        Assert.Null(ZCatalog.GetCalendarUnchecked(Byte.MaxValue));

    [Fact]
    public static void GetCalendarUnchecked_Zero() =>
        Assert.Equal(ZCalendar.Gregorian, ZCatalog.GetCalendarUnchecked(0));

    [Fact]
    public static void GetCalendarUnchecked()
    {
        Assert.Same(Gregorian, ZCatalog.GetCalendarUnchecked(Gregorian.Id.Value));
        Assert.Same(Julian, ZCatalog.GetCalendarUnchecked(Julian.Id.Value));
    }

    #endregion

    //private static void OnKeyNotSet(string key)
    //{
    //    Assert.DoesNotContain(key, ZCatalog.Keys);
    //    Assert.Throws<KeyNotFoundException>(() => ZCatalog.GetCalendar(key));
    //}

    private static void OnKeySet(string key, DayNumber epoch, ZCalendar? calendar)
    {
        Assert.NotNull(calendar);
        Assert.Equal(key, calendar!.Key);
        Assert.Equal(epoch, calendar.Epoch);
        Assert.Contains(key, ZCatalog.Keys);
        Assert.Same(calendar, ZCatalog.GetCalendar(key));
    }
}
