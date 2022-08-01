// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// REVIEW(code): size of the Z-catalog.
#define ZCATALOG_BIGGER

namespace Zorglub.Time.Extras
{
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Zorglub.Time.Hemerology.Scopes;
    using Zorglub.Time.Simple;

    using TmpCalendar = ZRegistry.TmpCalendar;

    // FIXME(code): sync with Calendar.
    // Snapshots: what about lazy calendars? Alos ensure that everything works
    // with lazy calendars (other types initialization).
    // GetSystemCalendar(CalendarId ident): public? officially, a ZCalendar has
    // no ID, only a key.
    // ZDate._cuid is now a full int, MaxId? Because of s_CalendarsById, it's
    // not that straightforward to do: we might have to resize this array and it
    // should be done in a thread-safe manner.

    /// <summary>
    /// Provides static methods to initialize a calendar or lookup an already initialized calendar.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class ZCatalog
    {
#if ZCATALOG_BIGGER
        /// <summary>
        /// Represents the maximum value for the ident of a calendar.
        /// <para>This field is a constant equal to 508.</para>
        /// </summary>
        // We choose MaxId so that (MaxId + 1) is a prime number;
        // see InitCalendarsByKey().
        private const int MaxId = 508;
#else
        /// <summary>
        /// Represents the maximum value for the ident of a calendar.
        /// <para>This field is a constant equal to 255.</para>
        /// </summary>
        private const int MaxId = Byte.MaxValue;
#endif

        /// <summary>
        /// Represents the minimum value for the ID of a user-defined calendar.
        /// <para>This field is equal to 128.</para>
        /// </summary>
        // IDs système : 0 à 63. On garantit la compatibilité avec Calendar en
        // réservant aussi les IDs 64 à 127; voir ToZCalendar() et ToCalendar().
        private static readonly int MinUserId = SimpleCatalog.MaxId + 1;

#if ZCATALOG_BIGGER
        /// <summary>
        /// Represents the absolute maximum number of user-defined calendars without counting those
        /// created from a <see cref="SimpleCalendar"/>.
        /// <para>This static property ALWAYS returns 896.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        /// <remarks>
        /// <para>It's very unlikely that this number will ever change, but we never know.
        /// Nevertheless, we guarantee that it will never be less than 128.</para>
        /// </remarks>
#else
        /// <summary>
        /// Represents the absolute maximum number of user-defined calendars without counting those
        /// created from a <see cref="Calendar"/>.
        /// <para>This static property ALWAYS returns 128.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        /// <remarks>
        /// <para>It's very unlikely that this number will ever change, but we never know.
        /// Nevertheless, we guarantee that it will never be less than 128.</para>
        /// </remarks>
#endif
        public static int MaxNumberOfUserCalendars { get; } = MaxId - MinUserId + 1;

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
            // InitSystemCalendar() uses this array to initialize it.
            // It MUST also be defined before s_CalendarsByKey because
            // InitCalendarsByKey() uses it.
            //
            // To ensure that default(ZDate) works properly, we must have a
            // calendar with ID equal to 0. This way date.Calendar does not throw
            // even if no calendar has been initialized by the user, it just
            // defaults to Gregorian.
            InitSystemCalendar(SimpleCalendar.Gregorian);

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lazy<ZCalendar>> s_CalendarsByKey =
            InitCalendarsByKey();

        private static readonly ZRegistry s_Registry = new(s_CalendarsByKey, s_CalendarsById, MinUserId);

        // We ignore lazy calendars not yet initialized.
        internal static ICollection<string> Keys => s_CalendarsByKey.Keys;

        public static ReadOnlySet<string> ReservedKeys => SimpleCatalog.ReservedKeys;

        #region Initializers

        /// <summary>
        /// Initializes a system calendar.
        /// <para>This method does not add the newly created calendar to
        /// <see cref="s_CalendarsByKey"/>; this is something that is automatically done when we
        /// initialize the dictionary.</para>
        /// </summary>
        [Pure]
        internal static ZCalendar InitSystemCalendar(SimpleCalendar calendar)
        {
            Debug.Assert(calendar.IsUserDefined == false);

            int id = (int)calendar.PermanentId;
            // NB: for ZDate to work properly, the Gregorian calendar must use
            // a scope equivalent to GregorianProlepticScope (see the ctor)
            // which is the case with calendar.Scope.
            var chr = new ZCalendar(id, calendar.Key, calendar.Scope, userDefined: false);

            return s_CalendarsById[id] = chr;
        }

        [Pure]
        private static ConcurrentDictionary<string, Lazy<ZCalendar>> InitCalendarsByKey()
        {
#if ZCATALOG_BIGGER
            // First prime number >= MaxId + 1 = 509.
            const int Capacity = 509;
#else
            // First prime number >= MaxId + 1 = 256.
            const int Capacity = 257;
#endif
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
        [Pure]
        public static IEnumerable<ZCalendar> GetCurrentCalendars() =>
            from chr in s_CalendarsById where chr is not null select chr;

        // If "all" is false, we filter out (system) calendars that have not yet
        // been initialized.
        [Pure]
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
        [Pure]
        public static SimpleCalendar ToCalendar(this ZCalendar @this)
        {
            Requires.NotNull(@this);
            int cuid = @this.Id;
            if (cuid > SimpleCatalog.MaxId) Throw.Argument(nameof(@this));

            // NB: un ZCalendar ayant un ID <= SimpleCatalog.MaxId
            // provient obligatoirement d'un Calendar.
            // REVIEW(code): while what we just said is true, does it really mean
            // that this calendar does truely exist?
            return SimpleCatalog.GetCalendarUnchecked(cuid);
        }

        // Converts a Calendar to a ZCalendar.
        [Pure]
        public static ZCalendar ToZCalendar(this SimpleCalendar @this)
        {
            Requires.NotNull(@this);

            return @this.IsUserDefined ? GetOrAddUserCalendar(@this)
                : GetSystemCalendar(@this.PermanentId);

            // TODO(code): use GetOrAdd().
            [Pure]
            static ZCalendar GetOrAddUserCalendar(SimpleCalendar calendar)
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
            static ZCalendar CreateUserCalendar(SimpleCalendar calendar)
            {
                int cuid = (int)calendar.Id;
                var chr = new ZCalendar(cuid, calendar.Key, calendar.Scope, userDefined: true);

                return s_CalendarsById[cuid] = chr;
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

            // Except in the Gregorian case, system calendars are only added to
            // s_CalendarsById on demand.
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
        public static ZCalendar GetOrAdd(string key, CalendarScope scope) =>
            s_Registry.GetOrAdd(key, scope);

        [Pure]
        public static ZCalendar Add(string key, CalendarScope scope) =>
            s_Registry.Add(key, scope);

        [Pure]
        public static bool TryAdd(
            string key, CalendarScope scope,
            [NotNullWhen(true)] out ZCalendar? calendar) =>
            s_Registry.TryAdd(key, scope, out calendar);
    }
}
