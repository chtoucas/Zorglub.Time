// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Zorglub.Time.Hemerology.Scopes;
    using Zorglub.Time.Simple;

    // FIXME(code): to be rewritten. Sync with Calendar.
    // Snapshots: what about lazy calendars?
    // GetSystemCalendar(CalendarId ident): public? officially, a ZCalendar has
    // no ID, only a key.
    // ZDate._cuid is now a full int, MaxId? Because of s_CalendarsById, it's
    // not that straightforward to do: we might have to resize this array and it
    // should be done in a thread-safe manner. I guess we should use a
    // concurrent list (ConcurrentBag?) but the usfeulness of s_CalendarsById is
    // questionable (it was created to allow for fast lookup).

    /// <summary>
    /// Provides static methods to initialize a calendar or lookup an already initialized calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class ZCatalog
    {
        /// <summary>
        /// Represents the maximun value for the ident of a calendar.
        /// <para>This field is a constant equal to 255.</para>
        /// </summary>
        private const int MaxId = Byte.MaxValue;

        private const int MinUserId = CalendarCatalog.MaxId + 1;

        /// <summary>
        /// Represents the absolute maximun number of user-defined calendars without counting those
        /// created from a <see cref="Calendar"/>.
        /// <para>This field is a read-only field equal to 128.</para>
        /// </summary>
        public static readonly int MaxNumberOfUserCalendars = MaxId - MinUserId + 1;

        /// <summary>This field is initially set to 127.</summary>
        // IDs système : 0 à 63. On garantit la compatibilité avec Calendar en
        // réservant aussi les IDs 64 à 127; voir ToZCalendar() et ToCalendar().
        private static int s_LastIdent = MinUserId - 1;

        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by their internal IDs.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly ZCalendar?[] s_CalendarsById = new ZCalendar?[MaxId + 1];

        /// <summary>
        /// Represents the proleptic Gregorian calendar.
        /// <para>This field is read-only.</para>
        /// </summary>
        internal static readonly ZCalendar Gregorian =
            // IMPORTANT: this property MUST stay after s_CalendarsById, indeed
            // InitSystemCalendar() uses it.
            //
            // To ensure that default(ZDate) works properly, we must have a
            // calendar with ID equal to 0. This way date.Calendar does not throw
            // even if no calendar has been initialized by the user, it just
            // defaults to Gregorian.
            InitSystemCalendar(GregorianCalendar.Instance);

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lazy<ZCalendar>> s_CalendarsByKey =
            InitCalendarsByKey();

        internal static ICollection<string> Keys => s_CalendarsByKey.Keys;

        public static ReadOnlySet<string> ReservedKeys => CalendarCatalog.ReservedKeys;

        #region Initializers

        /// <summary>
        /// Initializes a system calendar.
        /// <para>This method does not add the newly created (wide) calendar to
        /// <see cref="s_CalendarsByKey"/>; this is something that is automatically done when we
        /// initialize the dictionary.</para>
        /// </summary>
        [Pure]
        internal static ZCalendar InitSystemCalendar(Calendar calendar)
        {
            Debug.Assert(!calendar.IsUserDefined);

            int id = (int)calendar.PermanentId;

            // NB: all system calendars are proleptic; we ignore the prop
            // calendar.IsProleptic.
            var scope = MinMaxYearScope.WithMaximalRange(
                calendar.Schema, calendar.Epoch, onOrAfterEpoch: false);

            var chr = new ZCalendar(id, calendar.Key, scope, userDefined: false);

            return s_CalendarsById[id] = chr;
        }

        private static ConcurrentDictionary<string, Lazy<ZCalendar>> InitCalendarsByKey()
        {
            // First prime number >= max count of calendars (256).
            const int Capacity = 257;
            Debug.Assert(Capacity >= MaxId + 1);

            return new ConcurrentDictionary<string, Lazy<ZCalendar>>(
                concurrencyLevel: Environment.ProcessorCount,
                Capacity)
            {
                // Except in the Gregorian case, all calendars are lazily initialized.
                // Ensures that all system keys are taken.
                [Gregorian.Key] = new(Gregorian),
                [JulianZCalendar.Key] = new(() => JulianZCalendar.Instance),
                [ArmenianZCalendar.Key] = new(() => ArmenianZCalendar.Instance),
                [CopticZCalendar.Key] = new(() => CopticZCalendar.Instance),
                [EthiopicZCalendar.Key] = new(() => EthiopicZCalendar.Instance),
                [TabularIslamicZCalendar.Key] = new(() => TabularIslamicZCalendar.Instance),
                [ZoroastrianZCalendar.Key] = new(() => ZoroastrianZCalendar.Instance),
            };
        }

        #endregion
    }

    public partial class ZCatalog // Snapshots
    {
        // We ignore lazy calendars not yet initialized.
        public static IEnumerable<ZCalendar> GetCurrentCalendars() =>
            from chr in s_CalendarsById where chr is not null select chr;

        // If "all" is false, we filter out (system) calendars that have not yet
        // been initialized.
        public static IReadOnlyDictionary<string, ZCalendar> TakeSnapshot(bool all = false)
        {
            var arr = s_CalendarsByKey.ToArray();

            var q = from p in arr
                    where all || p.Value.IsValueCreated
                    let chr = p.Value.Value
                    where chr is not TmpCalendar
                    select KeyValuePair.Create(p.Key, chr);
            var dict = new Dictionary<string, ZCalendar>(q);

            return new ReadOnlyDictionary<string, ZCalendar>(dict);
        }
    }

    public partial class ZCatalog // ZCalendar <-> Calendar
    {
        public static Calendar ToCalendar(this ZCalendar @this)
        {
            Requires.NotNull(@this);
            if (@this.Id > CalendarCatalog.MaxId) Throw.Argument(nameof(@this));

            // NB: un ZCalendar ayant un ID <= CalendarCatalog.MaxIdent
            // provient obligatoirement d'un Calendar.
            return CalendarCatalog.GetCalendarUnchecked(@this.Id);
        }

        // Converts a Calendar to a ZCalendar.
        public static ZCalendar ToZCalendar(this Calendar @this)
        {
            Requires.NotNull(@this);

            return @this.IsUserDefined ? GetOrAddUserCalendar(@this)
                : GetSystemCalendar(@this.PermanentId);

            [Pure]
            static ZCalendar GetOrAddUserCalendar(Calendar calendar)
            {
                Debug.Assert(calendar.IsUserDefined);

                var chr = s_CalendarsByKey.GetOrAdd(
                    calendar.Key,
                    new Lazy<ZCalendar>(() => CreateUserCalendar(calendar))
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
            static ZCalendar CreateUserCalendar(Calendar calendar)
            {
                int cuid = (int)calendar.Id;

                var scope = MinMaxYearScope.WithMaximalRange(
                    calendar.Schema, calendar.Epoch, onOrAfterEpoch: !calendar.IsProleptic);

                var chr = new ZCalendar(cuid, calendar.Key, scope, userDefined: true);
                s_CalendarsById[cuid] = chr;

                return chr;
            }
        }
    }

    public partial class ZCatalog // Lookup
    {
        [Pure]
        public static ZCalendar GetCalendar(string key)
        {
            if (s_CalendarsByKey.TryGetValue(key, out Lazy<ZCalendar>? calendar))
            {
                var chr = calendar.Value;
                return chr is TmpCalendar ? Throw.KeyNotFound<ZCalendar>(key) : chr;
            }

            return Throw.KeyNotFound<ZCalendar>(key);
        }

        [Pure]
        public static bool TryGetCalendar(
            string key, [NotNullWhen(true)] out ZCalendar? calendar)
        {
            if (s_CalendarsByKey.TryGetValue(key, out Lazy<ZCalendar>? chr))
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

        [Pure]
        public static ZCalendar GetSystemCalendar(CalendarId ident)
        {
            if (ident.IsInvalid()) Throw.ArgumentOutOfRange(nameof(ident));

            return s_CalendarsById[(int)ident] ?? GetSystemCalendarUncached(ident);
        }

#nullable disable warnings

        [Pure]
        internal static ZCalendar GetCalendarUnchecked(int cuid)
        {
            Debug.Assert(cuid < s_CalendarsById.Length);

            // What happens if we request a "lazy" system calendar? Nothing
            // bad hopefully. This method is only called by ZDate.Calendar,
            // and, to be constructed, a ZDate requires a ZCalendar first.
            // Of course, it will fail hard if we decide one day to add binary
            // serialization to ZDate; nevertheless see GetSystemCalendar().

            return s_CalendarsById[cuid];
        }

        [Pure]
        // CIL code size = 18 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref readonly ZCalendar GetCalendarUnsafe(int cuid)
        {
            Debug.Assert(cuid < s_CalendarsById.Length);

            ref ZCalendar chr = ref MemoryMarshal.GetArrayDataReference(s_CalendarsById);
            return ref Unsafe.Add(ref chr, (nint)(uint)cuid);
        }

#nullable restore warnings

        [Pure]
        internal static ZCalendar GetSystemCalendarUncached(CalendarId id) =>
            id switch
            {
                CalendarId.Gregorian => Gregorian,
                CalendarId.Julian => JulianZCalendar.Instance,
                CalendarId.Armenian => ArmenianZCalendar.Instance,
                CalendarId.Coptic => CopticZCalendar.Instance,
                CalendarId.Ethiopic => EthiopicZCalendar.Instance,
                CalendarId.TabularIslamic => TabularIslamicZCalendar.Instance,
                CalendarId.Zoroastrian => ZoroastrianZCalendar.Instance,
                _ => Throw.InvalidOperation<ZCalendar>(),
            };
    }

    public partial class ZCatalog // Add
    {
        [Pure]
        public static ZCalendar GetOrAdd(string key, MinMaxYearScope scope)
        {
            if (s_LastIdent >= MaxId && s_CalendarsByKey.ContainsKey(key) == false)
            {
                Throw.CatalogOverflow();
            }

            var tmp = new TmpCalendar(key, scope);

            var lazy = new Lazy<ZCalendar>(() => CreateCalendar(tmp));
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
        public static ZCalendar Add(string key, MinMaxYearScope scope)
        {
            if (s_LastIdent >= MaxId) Throw.CatalogOverflow();

            var tmp = new TmpCalendar(key, scope);

            var lazy = new Lazy<ZCalendar>(() => CreateCalendar(tmp));

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
            string key, MinMaxYearScope scope,
            [NotNullWhen(true)] out ZCalendar? calendar)
        {
            if (s_LastIdent >= MaxId) { goto FAILED; }

            Requires.NotNull(key);
            Requires.NotNull(scope);

            TmpCalendar tmp;
            try { tmp = new TmpCalendar(key, scope); }
            catch (ArgumentException) { goto FAILED; }

            var lazy = new Lazy<ZCalendar>(() => CreateCalendar(tmp));

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

        //
        // Helpers
        //

        [Pure]
        private static ZCalendar CreateCalendar(TmpCalendar tmpCalendar)
        {
            Debug.Assert(tmpCalendar != null);

            int ident = Interlocked.Increment(ref s_LastIdent);
            if (ident > MaxId) { return tmpCalendar; }

            var chr = new ZCalendar(
                ident,
                tmpCalendar.Key,
                tmpCalendar.Scope,
                userDefined: true);

            return s_CalendarsById[ident] = chr;
        }

        private sealed class TmpCalendar : ZCalendar
        {
            public TmpCalendar(string key, MinMaxYearScope scope)
                : base(Int32.MaxValue, key, scope, userDefined: true) { }

            [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Hides inherited member.")]
            internal new int Id => Throw.InvalidOperation<int>();
        }
    }
}
