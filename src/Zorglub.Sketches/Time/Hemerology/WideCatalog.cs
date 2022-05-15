// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Zorglub.Time.Core;
    using Zorglub.Time.Simple;

    /// <summary>
    /// Provides static methods to initialize a calendar or lookup an already
    /// initialized calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class WideCatalog
    {
        /// <summary>
        /// Represents the maximun value for the ident of a calendar.
        /// <para>This field is a constant equal to 255.</para>
        /// </summary>
        internal const int MaxId = Byte.MaxValue;

        internal const int MinUserId = CalendarCatalog.MaxId + 1;

        /// <summary>
        /// Represents the absolute maximun number of user-defined calendars
        /// without counting those created from a <see cref="Calendar"/>.
        /// <para>This field is a read-only field equal to 128.</para>
        /// </summary>
        public static readonly int MaxNumberOfUserCalendars = MaxId - MinUserId + 1;

        /// <summary>This field is initially set to 127.</summary>
        // IDs système : 0 à 63. On garantit la compatibilité avec Calendar en
        // réservant aussi les IDs 64 à 127; voir ToWideCalendar() et
        // ToSimpleCalendar().
        private static int s_LastIdent = MinUserId - 1;

        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by
        /// their internal IDs.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly WideCalendar?[] s_CalendarsById = new WideCalendar?[MaxId + 1];

        /// <summary>
        /// Represents the proleptic Gregorian calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly WideCalendar Gregorian =
            // IMPORTANT: this property MUST stay after s_CalendarsById, indeed
            // InitSystemCalendar() uses it.
            //
            // To ensure that default(WideDate) works properly, we must have a
            // calendar with ID equal to 0. This way date.Calendar does not throw
            // even if no calendar has been initialized by the user, it just
            // defaults to Gregorian.
            InitSystemCalendar(GregorianCalendar.Instance);

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lazy<WideCalendar>> s_CalendarsByKey =
            InitCalendarsByKey();

        internal static ICollection<string> Keys => s_CalendarsByKey.Keys;

        public static ReadOnlySet<string> ReservedKeys => CalendarCatalog.ReservedKeys;

        #region Initializers

        /// <summary>
        /// Initializes a system calendar.
        /// <para>This method does not add the newly created (wide) calendar
        /// to <see cref="s_CalendarsByKey"/>; this is something that is
        /// automatically done when we initialize the dictionary.</para>
        /// </summary>
        [Pure]
        internal static WideCalendar InitSystemCalendar(Calendar calendar)
        {
            Debug.Assert(!calendar.IsUserDefined);

            int id = (int)calendar.PermanentId;
            var key = calendar.Key;
            var chr = new WideCalendar(
                id,
                key,
                calendar.Schema,
                calendar.Epoch,
                // NB: we ignore the prop calendar.IsProleptic.
                widest: true,
                userDefined: false);

            return s_CalendarsById[id] = chr;
        }

        private static ConcurrentDictionary<string, Lazy<WideCalendar>> InitCalendarsByKey()
        {
            // First prime number >= max count of calendars (256).
            const int Capacity = 257;
            Debug.Assert(Capacity >= MaxId + 1);

            return new ConcurrentDictionary<string, Lazy<WideCalendar>>(
                concurrencyLevel: Environment.ProcessorCount,
                Capacity)
            {
                // Except in the Gregorian case, all calendars are lazily initialized.
                // Ensures that all system keys are taken.
                [Gregorian.Key] = new(Gregorian),
                [WideJulianCalendar.Key] = new(() => WideJulianCalendar.Instance),
                [WideArmenianCalendar.Key] = new(() => WideArmenianCalendar.Instance),
                [WideCopticCalendar.Key] = new(() => WideCopticCalendar.Instance),
                [WideEthiopicCalendar.Key] = new(() => WideEthiopicCalendar.Instance),
                [WideTabularIslamicCalendar.Key] = new(() => WideTabularIslamicCalendar.Instance),
                [WideZoroastrianCalendar.Key] = new(() => WideZoroastrianCalendar.Instance),
            };
        }

        #endregion
    }

    public partial class WideCatalog // Snapshots
    {
        // FIXME: lazy calendars.

        // We ignore lazy calendars not yet initialized.
        public static IEnumerable<WideCalendar> GetCurrentCalendars() =>
            from chr in s_CalendarsById where chr is not null select chr;

        // If "all" is false, we filter out (system) calendars that have not yet
        // been initialized.
        public static IReadOnlyDictionary<string, WideCalendar> TakeSnapshot(bool all = false)
        {
            var arr = s_CalendarsByKey.ToArray();

            var q = from p in arr
                    where all || p.Value.IsValueCreated
                    let chr = p.Value.Value
                    where chr is not TmpCalendar
                    select KeyValuePair.Create(p.Key, chr);
            var dict = new Dictionary<string, WideCalendar>(q);

            return new ReadOnlyDictionary<string, WideCalendar>(dict);
        }
    }

    public partial class WideCatalog // WideCalendar <-> Calendar
    {
        public static Calendar ToCalendar(this WideCalendar @this)
        {
            Requires.NotNull(@this);
            if (@this.Id > CalendarCatalog.MaxId)
            {
                Throw.Argument(nameof(@this));
            }

            // NB: un WideCalendar ayant un ID <= CalendarCatalog.MaxIdent
            // provient obligatoirement d'un Calendar.
            return CalendarCatalog.GetCalendarUnchecked(@this.Id);
        }

        // Converts a Calendar to a WideCalendar.
        public static WideCalendar ToWideCalendar(this Calendar @this)
        {
            Requires.NotNull(@this);

            return @this.IsUserDefined ? getOrAddUserCalendar(@this)
                : GetSystemCalendar(@this.PermanentId);

            [Pure]
            static WideCalendar getOrAddUserCalendar(Calendar calendar)
            {
                Debug.Assert(calendar.IsUserDefined);

                var chr = s_CalendarsByKey.GetOrAdd(
                    calendar.Key,
                    new Lazy<WideCalendar>(() => createUserCalendar(calendar))
                ).Value;

                // The only reliable way to verify that the result is the
                // expected calendar is by comparing the IDs.
                if (chr.Id != (int)calendar.Id)
                {
                    // Another calendar w/ the same key already exists.
                    Throw.KeyAlreadyExists(nameof(calendar), calendar.Key);
                }

                return chr;
            }

            [Pure]
            static WideCalendar createUserCalendar(Calendar calendar)
            {
                int cuid = (int)calendar.Id;

                var chr = new WideCalendar(
                    cuid,
                    calendar.Key, calendar.Schema, calendar.Epoch, calendar.IsProleptic,
                    userDefined: true);
                s_CalendarsById[cuid] = chr;

                return chr;
            }
        }
    }

    public partial class WideCatalog // Lookup
    {
        [Pure]
        public static WideCalendar GetCalendar(string key)
        {
            if (s_CalendarsByKey.TryGetValue(key, out Lazy<WideCalendar>? calendar))
            {
                var chr = calendar.Value;
                return chr is TmpCalendar ? Throw.KeyNotFound<WideCalendar>(key) : chr;
            }

            return Throw.KeyNotFound<WideCalendar>(key);
        }

        [Pure]
        public static bool TryGetCalendar(
            string key, [NotNullWhen(true)] out WideCalendar? calendar)
        {
            if (s_CalendarsByKey.TryGetValue(key, out Lazy<WideCalendar>? chr))
            {
                var tmp = chr.Value;
                if (tmp is TmpCalendar) { goto NOT_FOUND; }
                calendar = tmp;
                return true;
            }

        NOT_FOUND:
            calendar = null;
            return false;
        }

        // REVIEW: public? officially, a WideCalendar has no ID, only a key.
        [Pure]
        public static WideCalendar GetSystemCalendar(CalendarId id)
        {
            if (id.IsInvalid()) Throw.ArgumentOutOfRange(nameof(id));

            return s_CalendarsById[(int)id] ?? GetSystemCalendarUncached(id);
        }

#nullable disable warnings

        [Pure]
        internal static WideCalendar GetCalendarUnchecked(int cuid)
        {
            Debug.Assert(cuid <= MaxId);

            // What happens if we request a "lazy" system calendar? Nothing
            // bad hopefully. This method is only called by WideDate.Calendar,
            // and to be constructed a WideDate requires a WideCalendar first.
            // Of course, it will fail hard if we decide one day to add binary
            // serialization to WideDate; nevertheless see GetSystemCalendar().

            return s_CalendarsById[cuid];
        }

        [Pure]
        // CIL code size = 18 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref readonly WideCalendar GetCalendarUnsafe(int cuid)
        {
            Debug.Assert(cuid < s_CalendarsById.Length);

            ref WideCalendar chr = ref MemoryMarshal.GetArrayDataReference(s_CalendarsById);
            return ref Unsafe.Add(ref chr, (nint)(uint)cuid);
        }

#nullable restore warnings

        [Pure]
        internal static WideCalendar GetSystemCalendarUncached(CalendarId id) =>
            id switch
            {
                CalendarId.Gregorian => Gregorian,
                CalendarId.Julian => WideJulianCalendar.Instance,
                CalendarId.Armenian => WideArmenianCalendar.Instance,
                CalendarId.Coptic => WideCopticCalendar.Instance,
                CalendarId.Ethiopic => WideEthiopicCalendar.Instance,
                CalendarId.TabularIslamic => WideTabularIslamicCalendar.Instance,
                CalendarId.Zoroastrian => WideZoroastrianCalendar.Instance,
                _ => Throw.InvalidOperation<WideCalendar>(),
            };
    }

    public partial class WideCatalog // Add
    {
        [Pure]
        public static WideCalendar GetOrAdd(
            string key, ICalendricalSchema schema, DayNumber epoch, bool widest)
        {
            if (s_LastIdent >= MaxId && !s_CalendarsByKey.ContainsKey(key))
            {
                Throw.CatalogOverflow();
            }

            var tmp = new TmpCalendar(key, schema, epoch, widest);

            var lazy = new Lazy<WideCalendar>(() => CreateCalendar(tmp));
            var lazy1 = s_CalendarsByKey.GetOrAdd(key, lazy);

            var chr = lazy1.Value;
            if (chr is TmpCalendar)
            {
                if (ReferenceEquals(lazy1, lazy))
                {
                    s_CalendarsByKey.TryRemove(key, out _);
                }

                Throw.CatalogOverflow();
            }

            return chr;
        }

        [Pure]
        public static WideCalendar Add(
            string key, ICalendricalSchema schema, DayNumber epoch, bool widest)
        {
            if (s_LastIdent >= MaxId) Throw.CatalogOverflow();

            var tmp = new TmpCalendar(key, schema, epoch, widest);

            var lazy = new Lazy<WideCalendar>(() => CreateCalendar(tmp));

            if (s_CalendarsByKey.TryAdd(key, lazy) == false)
            {
                Throw.KeyAlreadyExists(nameof(key), key);
            }

            var chr = lazy.Value;
            if (chr is TmpCalendar)
            {
                s_CalendarsByKey.TryRemove(key, out _);

                Throw.CatalogOverflow();
            }

            return chr;
        }

        [Pure]
        public static bool TryAdd(
            string key, ICalendricalSchema schema, DayNumber epoch, bool widest,
            [NotNullWhen(true)] out WideCalendar? calendar)
        {
            if (s_LastIdent >= MaxId) { goto FAILED; }

            Requires.NotNull(key);
            Requires.NotNull(schema);

            TmpCalendar tmp;
            try { tmp = new TmpCalendar(key, schema, epoch, widest); }
            catch (ArgumentException) { goto FAILED; }

            var lazy = new Lazy<WideCalendar>(() => CreateCalendar(tmp));

            if (s_CalendarsByKey.TryAdd(key, lazy))
            {
                var chr = lazy.Value;
                if (chr is TmpCalendar)
                {
                    s_CalendarsByKey.TryRemove(key, out _);
                    goto FAILED;
                }
                else
                {
                    calendar = chr;
                    return true;
                }
            }

        FAILED:
            calendar = null;
            return false;
        }

        #region Helpers

        [Pure]
        private static WideCalendar CreateCalendar(TmpCalendar tmpCalendar)
        {
            Debug.Assert(tmpCalendar != null);

            int ident = Interlocked.Increment(ref s_LastIdent);
            if (ident > MaxId) { return tmpCalendar; }

            var chr = new WideCalendar(
                ident,
                tmpCalendar.Key,
                tmpCalendar.Schema,
                tmpCalendar.Epoch,
                tmpCalendar.Widest,
                userDefined: true);

            return s_CalendarsById[ident] = chr;
        }

        private sealed class TmpCalendar : WideCalendar
        {
            public TmpCalendar(
                string key,
                ICalendricalSchema schema,
                DayNumber epoch,
                bool widest)
                : base(Int32.MaxValue, key, schema, epoch, widest, userDefined: true)
            {
                Widest = widest;
            }

            [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Hides inherited member.")]
            internal new int Id => Throw.InvalidOperation<int>();

            public bool Widest { get; }
        }

        #endregion
    }
}
