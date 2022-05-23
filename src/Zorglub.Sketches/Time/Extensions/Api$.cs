// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    using Zorglub.Time.Simple;

    // Méthodes retirées car redondantes. Par ex, au lieu d'écrire
    // > var startOfYear = date.GetStartOfYear();
    // on peut utiliser
    // > var startOfYear = CalendarDate.AtStartOfYear(date.CalendarYear);
    // Bon, c'est vrai que c'est plus compliqué.
    // En plus on n'a pas à revalider les paramètres.

    /// <summary>
    /// Provides extension methods for calendrical objects.
    /// </summary>
    public static partial class ApiExtensions { }

    //public partial class ApiExtensions // CalendarDate
    //{
    //    /// <summary>
    //    /// Obtains the first day of the year to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDate GetStartOfYear(this CalendarDate @this) =>
    //        new(@this.Parts.StartOfYear, @this.Cuid);

    //    /// <summary>
    //    /// Obtains the last day of the year to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDate GetEndOfYear(this CalendarDate @this)
    //    {
    //        ref readonly var chr = ref @this.CalendarRef;
    //        var ymd = chr.Schema.GetEndOfYearParts(@this.Year);
    //        return new CalendarDate(ymd, chr.Id);
    //    }

    //    /// <summary>
    //    /// Obtains the first day of the month to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDate GetStartOfMonth(this CalendarDate @this) =>
    //        new(@this.Parts.StartOfMonth, @this.Cuid);

    //    /// <summary>
    //    /// Obtains the last day of the month to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDate GetEndOfMonth(this CalendarDate @this)
    //    {
    //        @this.Parts.Unpack(out int y, out int m);
    //        ref readonly var chr = ref @this.CalendarRef;
    //        var ymd = chr.Schema.GetEndOfMonthParts(y, m);
    //        return new CalendarDate(ymd, chr.Id);
    //    }
    //}

    //public partial class ApiExtensions // CalendarDay
    //{
    //    /// <summary>
    //    /// Obtains the first day of the year to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDay GetStartOfYear(this CalendarDay @this)
    //    {
    //        ref readonly var chr = ref @this.CalendarRef;
    //        int daysSinceEpoch = chr.Schema.GetStartOfYear(@this.Year);
    //        return new CalendarDay(daysSinceEpoch, chr.Id);
    //    }

    //    /// <summary>
    //    /// Obtains the last day of the year to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDay GetEndOfYear(this CalendarDay @this)
    //    {
    //        ref readonly var chr = ref @this.CalendarRef;
    //        int daysSinceEpoch = chr.Schema.GetEndOfYear(@this.Year);
    //        return new CalendarDay(daysSinceEpoch, chr.Id);
    //    }

    //    /// <summary>
    //    /// Obtains the first day of the month to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDay GetStartOfMonth(this CalendarDay @this)
    //    {
    //        var (y, m, _) = @this;
    //        ref readonly var chr = ref @this.CalendarRef;
    //        int daysSinceEpoch = chr.Schema.GetStartOfMonth(y, m);
    //        return new CalendarDay(daysSinceEpoch, chr.Id);
    //    }

    //    /// <summary>
    //    /// Obtains the last day of the month to which belongs the specified
    //    /// date.
    //    /// </summary>
    //    [Pure]
    //    public static CalendarDay GetEndOfMonth(this CalendarDay @this)
    //    {
    //        var (y, m, _) = @this;
    //        ref readonly var chr = ref @this.CalendarRef;
    //        int daysSinceEpoch = chr.Schema.GetEndOfMonth(y, m);
    //        return new CalendarDay(daysSinceEpoch, chr.Id);
    //    }
    //}

    //public partial class ApiExtensions // OrdinalDate
    //{
    //    /// <summary>
    //    /// Obtains the first day of the year to which belongs the specified
    //    /// ordinal date.
    //    /// </summary>
    //    [Pure]
    //    public static OrdinalDate GetStartOfYear(this OrdinalDate @this) =>
    //        new(@this.Parts.StartOfYear, @this.Cuid);

    //    /// <summary>
    //    /// Obtains the last day of the year to which belongs the specified
    //    /// ordinal date.
    //    /// </summary>
    //    [Pure]
    //    public static OrdinalDate GetEndOfYear(this OrdinalDate @this)
    //    {
    //        ref readonly var chr = ref @this.CalendarRef;
    //        var ydoy = chr.Schema.GetEndOfYearOrdinalParts(@this.Year);
    //        return new OrdinalDate(ydoy, chr.Id);
    //    }

    //    /// <summary>
    //    /// Obtains the first day of the month to which belongs the specified
    //    /// ordinal date.
    //    /// </summary>
    //    [Pure]
    //    public static OrdinalDate GetStartOfMonth(this OrdinalDate @this)
    //    {
    //        @this.Parts.Unpack(out int y, out int m);
    //        ref readonly var chr = ref @this.CalendarRef;
    //        var ydoy = chr.Schema.GetStartOfMonthOrdinalParts(y, m);
    //        return new OrdinalDate(ydoy, chr.Id);
    //    }

    //    /// <summary>
    //    /// Obtains the last day of the month to which belongs the specified
    //    /// ordinal date.
    //    /// </summary>
    //    [Pure]
    //    public static OrdinalDate GetEndOfMonth(this OrdinalDate @this)
    //    {
    //        @this.Parts.Unpack(out int y, out int m);
    //        ref readonly var chr = ref @this.CalendarRef;
    //        var ydoy = chr.Schema.GetEndOfMonthOrdinalParts(y, m);
    //        return new OrdinalDate(ydoy, chr.Id);
    //    }
    //}

    public partial class ApiExtensions // CalendarYear
    {
        /// <summary>
        /// Converts the specified year to a range of days.
        /// </summary>
        [Pure]
        [Obsolete("Use Range<OrdinalDate>.")]
        public static DateRange ToInterval(this CalendarYear @this) => DateRange.FromYear(@this);

        /// <summary>
        /// Interconverts the specified year to a day range within a different
        /// calendar.
        /// </summary>
        /// <remarks>
        /// This method always performs the conversion whether it's necessary
        /// or not. To avoid an expensive operation, it's better to check before
        /// that <paramref name="newCalendar"/> is actually different from the
        /// calendar of the current instance.
        /// </remarks>
        // On laissera probablement cette méthode de côté.
        [Pure]
        [Obsolete("Use Range<OrdinalDate>.")]
        public static DateRange WithCalendar(this CalendarYear @this, Calendar newCalendar) =>
            ToInterval(@this).WithCalendar(newCalendar);
    }

    public partial class ApiExtensions // CalendarMonth
    {
        /// <summary>
        /// Converts the specified month to a range of days.
        /// </summary>
        [Pure]
        [Obsolete("Use Range<CalendarDate>.")]
        public static DateRange ToInterval(this CalendarMonth @this) => DateRange.FromMonth(@this);

        /// <summary>
        /// Interconverts the specified month to a day range within a different
        /// calendar.
        /// </summary>
        /// <remarks>
        /// This method always performs the conversion whether it's necessary
        /// or not. To avoid an expensive operation, it's better to check before
        /// that <paramref name="newCalendar"/> is actually different from the
        /// calendar of the current instance.
        /// </remarks>
        [Pure]
        [Obsolete("Use Range<CalendarDate>.")]
        public static DateRange WithCalendar(this CalendarMonth @this, Calendar newCalendar) =>
            ToInterval(@this).WithCalendar(newCalendar);
    }
}
