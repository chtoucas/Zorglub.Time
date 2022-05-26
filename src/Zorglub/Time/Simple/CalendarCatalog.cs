// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Zorglub.Time.Core;

    // All calendars are "apparent" singletons: whether a calendar has been fully
    // constructed or not before, a call to (Try)GetCalendar() ALWAYS returns
    // the same instance.
    // More or less, CalendarCatalog behaves like a concurrent keyed collection
    // whose keys are readable and writable but not updatable.
    //
    // Internally, we use three collections:
    // - s_CalendarsByKey: all calendars indexed by their key.
    //   Not the fastest, but enforces the unicity of keys -and- ensures that
    //   ops are thread-safe.
    // - s_CalendarsById: all calendars indexed by their ID.
    // - s_SystemCalendars: system calendars indexed by their ID.
    //   Immutable part of s_CalendarsById.
    //
    // WARNING: a call to a method/prop from this class CAN NOT appear during
    // the initialization (static or instance) of a pre-defined Calendar,
    // otherwise we'll have a serious type initialization problem (null
    // references). If this becomes a problem (too many system calendars), we
    // could register the calendars lazily --- but be very careful with this,
    // many things rely on the fact that system calendars are not lazy.

    /// <summary>
    /// Provides static methods to initialize or lookup singleton instances of
    /// <see cref="Calendar"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static partial class CalendarCatalog
    {
        /// <summary>
        /// Represents the absolute maximun value for the ID of a calendar.
        /// <para>This field is a constant equal to 127.</para>
        /// </summary>
        internal const int MaxId = (int)Cuid.Max;

        /// <summary>
        /// Represents the minimum value for the ID of a user-defined calendar.
        /// <para>This field is a constant equal to 64.</para>
        /// </summary>
        internal const int MinUserId = (int)Cuid.MinUser;

        /// <summary>
        /// Represents the absolute maximun number of user-defined calendars.
        /// <para>This field is a read-only field equal to 64.</para>
        /// </summary>
        /// <remarks>
        /// <para>It's very unlikely that this number will ever change, but we
        /// never know. Nevertheless, we guarantee that it will never be less
        /// than 64.</para>
        /// </remarks>
        public static readonly int MaxNumberOfUserCalendars = MaxId - MinUserId + 1;

        /// <summary>This field is initially set to 63.</summary>
        private static int s_LastIdent = MinUserId - 1;

        /// <summary>
        /// Represents the (immutable) array of system calendars, indexed by
        /// their internal IDs.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Calendar[] s_SystemCalendars = InitSystemCalendars();

        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by
        /// their internal IDs.
        /// <para>A value is null if the calendar has not yet been fully
        /// constructed (obviously).</para>
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Calendar?[] s_CalendarsById = InitCalendarsById(s_SystemCalendars);

        /// <summary>
        /// Represents the dictionary of (lazy) calendars, indexed by their keys.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lazy<Calendar>> s_CalendarsByKey =
            InitCalendarsByKey(s_SystemCalendars);

        /// <summary>
        /// Gets the list of keys of all fully constructed calendars at the time
        /// of the request.
        /// <para>This collection may also contain a few bad keys, those paired
        /// with a calendar with ID <see cref="Cuid.Invalid"/>.</para>
        /// </summary>
        internal static ICollection<string> Keys =>
            // We do not provide a public equivalent to this property.
            // First, it may contain keys paired with an "invalid" calendar.
            // Second, ConcurrentDictionary.Keys (and therefore Keys) returns a
            // snapshot of the collection of keys which might be surprising as
            // collection properties are expected to return live collection.
            // Furthermore it kind of disclose the internal implementation.
            // Third, we have an indirect but better(?) alternative: TakeSnapshot().
            s_CalendarsByKey.Keys;

        /// <summary>
        /// Gets the collection of reserved keys.
        /// </summary>
        public static ReadOnlySet<string> ReservedKeys { get; } =
            new ReadOnlySet<string>(from chr in s_SystemCalendars select chr.Key);

        /// <summary>
        /// Gets the collection of system calendars.
        /// </summary>
        public static IReadOnlyCollection<Calendar> SystemCalendars { get; } =
            Array.AsReadOnly(s_SystemCalendars);

        #region Initializers

        private static Calendar[] InitSystemCalendars()
        {
            var arr = new Calendar[1 + (int)Cuid.MaxSystem];

            Add(GregorianCalendar.Instance);
            Add(JulianCalendar.Instance);
            Add(ArmenianCalendar.Instance);
            Add(CopticCalendar.Instance);
            Add(EthiopicCalendar.Instance);
            Add(TabularIslamicCalendar.Instance);
            Add(ZoroastrianCalendar.Instance);

            return arr;

            void Add(Calendar chr) => arr[(int)chr.PermanentId] = chr;
        }

        private static Calendar?[] InitCalendarsById(Calendar[] systemCalendars)
        {
            var arr = new Calendar?[MaxId + 1];
            Array.Copy(systemCalendars, arr, systemCalendars.Length);
            return arr;
        }

        private static ConcurrentDictionary<string, Lazy<Calendar>>
            InitCalendarsByKey(Calendar[] systemCalendars)
        {
            // First prime number >= max nbr of calendars (128 = MaxId + 1).
            const int Capacity = 131;
            Debug.Assert(Capacity > MaxId);

            var dict = new ConcurrentDictionary<string, Lazy<Calendar>>(
                // If I'm not mistaken, this is the default concurrency level.
                concurrencyLevel: Environment.ProcessorCount,
                Capacity);

            foreach (var chr in systemCalendars)
            {
                // Indexer instead of TryAdd(): unconditional add.
                dict[chr.Key] = new Lazy<Calendar>(chr);
            }
            return dict;
        }

        #endregion
    }

    public partial class CalendarCatalog // Snapshots
    {
        /// <summary>
        /// Obtains the collection of all calendars at the time of the request.
        /// </summary>
        public static IReadOnlyCollection<Calendar> GetAllCalendars()
        {
            // Fast track.
            if (s_LastIdent < MinUserId) { return SystemCalendars; }

            int sys = s_SystemCalendars.Length;
            int usr = CountUserCalendars();

            // Same as s_CalendarById but without the null's.
            // NB: the code works even if s_CalendarsById changed in the
            // meantime. We return a snapshot of s_CalendarsById at the point
            // of time when we called CountUserCalendars().
            var arr = new Calendar[sys + usr];
            // Copy system calendars.
            Array.Copy(s_CalendarsById, arr, sys);
            // Copy user-defined calendars.
            Array.Copy(s_CalendarsById, MinUserId, arr, sys, usr);

            return Array.AsReadOnly(arr);
        }

        /// <summary>
        /// Obtains the collection of user-defined calendars at the time of the
        /// request.
        /// </summary>
        public static IReadOnlyCollection<Calendar> GetUserCalendars()
        {
            int usr = CountUserCalendars();

            // Fast track.
            if (usr == 0) { return Array.Empty<Calendar>(); }

            var arr = new Calendar[usr];
            Array.Copy(s_CalendarsById, MinUserId, arr, 0, usr);

            return Array.AsReadOnly(arr);
        }

        /// <summary>
        /// Takes a snapshot of the collection of calendars indexed by their key.
        /// </summary>
        public static IReadOnlyDictionary<string, Calendar> TakeSnapshot()
        {
            // Take a snapshot of s_CalendarsByKey.
            var arr = s_CalendarsByKey.ToArray();

            var dict = new Dictionary<string, Calendar>(arr.Length);
            foreach (var x in arr)
            {
                var chr = x.Value.Value;
                if (chr.Id == Cuid.Invalid) { continue; }
                dict.Add(x.Key, chr);
            }

            return new ReadOnlyDictionary<string, Calendar>(dict);
        }

        /// <summary>
        /// Counts the number of user-defined calendars at the time of the request.
        /// </summary>
        private static int CountUserCalendars() => Math.Min(s_LastIdent, MaxId) - MinUserId + 1;
    }

    public partial class CalendarCatalog // Lookup
    {
        /// <summary>
        /// Looks up a calendar by its unique key.
        /// </summary>
        /// <remarks>
        /// <para>Repeated calls to this method with the same parameter ALWAYS
        /// return the same instance.</para>
        /// <para>See also <seealso cref="TakeSnapshot"/>.</para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">A calendar with the specified
        /// <paramref name="key"/> could not be found.</exception>
        [Pure]
        public static Calendar GetCalendar(string key)
        {
            if (s_CalendarsByKey.TryGetValue(key, out Lazy<Calendar>? calendar))
            {
                var chr = calendar.Value;
                return chr.Id == Cuid.Invalid ? Throw.KeyNotFound<Calendar>(key) : chr;
            }

            return Throw.KeyNotFound<Calendar>(key);
        }

        /// <summary>
        /// Attempts to look up a calendar by its unique key.
        /// </summary>
        /// <remarks>
        /// <para>See also <seealso cref="TakeSnapshot"/>.</para>
        /// </remarks>
        [Pure]
        public static bool TryGetCalendar(string key, [NotNullWhen(true)] out Calendar? calendar)
        {
            if (s_CalendarsByKey.TryGetValue(key, out Lazy<Calendar>? chr))
            {
                var tmp = chr.Value;
                if (tmp.Id == Cuid.Invalid) { goto NOT_FOUND; }
                calendar = tmp;
                return true;
            }

        NOT_FOUND:
            calendar = null;
            return false;
        }

        /// <summary>
        /// Looks up a <i>system</i> calendar by its unique identifier.
        /// </summary>
        /// <exception cref="AoorException">The specified ID is not valid.</exception>
        /// <remarks>
        /// <para>Repeated calls to this method with the same parameter ALWAYS return the same
        /// instance.</para>
        /// </remarks>
        [Pure]
        public static Calendar GetSystemCalendar(CalendarId id)
        {
            // REVIEW(code): simply use id.IsInvalid()?
            int index = (int)id;
            if (index < 0 || (uint)index >= (uint)s_SystemCalendars.Length)
            {
                Throw.ArgumentOutOfRange(nameof(id));
            }

            return s_SystemCalendars[index];
        }

#nullable disable warnings
        // If I didn't mess things up, we only access non-null array elements.

        // Both methods get called quite a lot, so even a tiny difference can
        // prove to be important.
        // The idea is that they are only called by fully constructed
        // calendrical objects. The immediate consequence is that the ID is a
        // valid array index (non-null value). Of course, it works because we
        // can't remove a calendar after it has been initialized. Later, if we
        // decide to lazily initialize the system calendars, a call to
        // GetCalendarUncached() is necessary. For instance, new CalendarDate()
        // works even if the calendar has not been initialized, in which case
        // CalendarDate.Calendar would be null.

        /// <summary>
        /// Looks up a calendar by its unique identifier.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>This method does NOT validate its parameter.</item>
        /// <item>This method only works with in-memory calendars .</item>
        /// </list>
        /// <para>Repeated calls to this method with the same parameter ALWAYS
        /// return the same instance.</para>
        /// </remarks>
        [Pure]
        internal static Calendar GetCalendarUnchecked(int cuid)
        {
            Debug.Assert(cuid < s_CalendarsById.Length);

            return s_CalendarsById[cuid];
        }

        /// <summary>
        /// Looks up a calendar by its unique identifier.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>This method does NOT validate its parameter.</item>
        /// <item>This method only works with in-memory calendars .</item>
        /// <item>This method returns a read-only reference to the calendar.</item>
        /// </list>
        /// <para>Repeated calls to this method with the same parameter ALWAYS return the same
        /// instance.</para>
        /// </remarks>
        [Pure]
        // CIL code size = 18 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref readonly Calendar GetCalendarUnsafe(int cuid)
        {
            Debug.Assert(cuid < s_CalendarsById.Length);

            // We can remove all array bound checks. The actual perf boost is
            // really tiny, nevertheless the generated code is much smaller.
            // WARNING: it removes array variance checks.
            // See https://github.com/CommunityToolkit/dotnet/blob/main/CommunityToolkit.HighPerformance/Extensions/ArrayExtensions.1D.cs
            ref Calendar chr = ref MemoryMarshal.GetArrayDataReference(s_CalendarsById);
            return ref Unsafe.Add(ref chr, (nint)(uint)cuid);
        }

#nullable restore warnings
    }

    public partial class CalendarCatalog // Add
    {
        /// <summary>
        /// Creates a calendar from a (unique) key, a reference epoch and a
        /// calendrical schema, then adds it to the system.
        /// </summary>
        /// <remarks>
        /// <para>If a calendar with the same key already exists, this method
        /// ignores the other parameters and returns it, without changing the
        /// internal state of this class. See also
        /// <seealso cref="ReservedKeys"/>.</para>
        /// <para>There is a hard limit on the total number of calendars one can
        /// create; see <see cref="MaxNumberOfUserCalendars"/>.</para>
        /// <para>It is the duty of the caller to ensure that repeated calls to
        /// this method are coherent.</para>
        /// <para>This method is thread-safe.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">One of the parameters is
        /// null.</exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/>
        /// contains at least one month whose length is strictly less than
        /// <see cref="Calendar.MinMinDaysInMonth"/>.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1, 9999]
        /// (non-proleptic case) or [-9998, 9999] (proleptic case).</exception>
        /// <exception cref="OverflowException">The key was not already taken
        /// but the system already reached the maximum number of calendars it
        /// can handle.</exception>
        [Pure]
        public static Calendar GetOrAdd(
            string key, SystemSchema schema, DayNumber epoch, bool proleptic)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            // This should only be done when the key is not already taken.
            if (s_LastIdent >= MaxId && !s_CalendarsByKey.ContainsKey(key))
            {
                Throw.CatalogOverflow();
            }

            Calendar tmp = ValidateParameters(key, schema, epoch, proleptic);

            // The callback won't be executed until we query Value.
            var lazy = new Lazy<Calendar>(() => CreateCalendar(tmp));

            var lazy1 = s_CalendarsByKey.GetOrAdd(key, lazy);

            // If the key is already taken, obviously lazy1.Value returns the
            // pre-existing calendar, but the important point here is that
            // CreateCalendar() was not called and therefore s_LastIdent did
            // NOT change. Beware, below this is NOT equivalent to lazy.Value.
            var chr = lazy1.Value;

            if (chr.Id == Cuid.Invalid)
            {
                // Clean up.
                // Beware, it is possible to end up here even if lazy1.Value
                // returned a pre-existing calendar. Indeed, there is a very
                // short window of time during which a TmpCalendar is still
                // referenced by s_CalendarsByKey.
                // The next check ensures that we only try to unlist the local
                // instance of TmpCalendar.
                // Actually, this is not mandatory, we could try to remove the
                // same key twice...
                if (ReferenceEquals(lazy1, lazy))
                {
                    s_CalendarsByKey.TryRemove(key, out _);
                }

                Throw.CatalogOverflow();
            }

            return chr;
        }

        /// <summary>
        /// Creates a calendar from a (unique) key, a reference epoch and a
        /// calendrical schema, then adds it to the system.
        /// </summary>
        /// <remarks>
        /// <para>There is a hard limit on the total number of calendars one can
        /// create; see <see cref="MaxNumberOfUserCalendars"/>.</para>
        /// <para>It is the duty of the caller to ensure that repeated calls to
        /// this method are coherent.</para>
        /// <para>This method is thread-safe.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">One of the parameters is
        /// null.</exception>
        /// <exception cref="ArgumentException">A calendar with the same key
        /// already exists. See also <seealso cref="ReservedKeys"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="schema"/>
        /// contains at least one month whose length is strictly less than
        /// <see cref="Calendar.MinMinDaysInMonth"/>.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1, 9999]
        /// (non-proleptic case) or [-9998, 9999] (proleptic case).</exception>
        /// <exception cref="OverflowException">The system already reached the
        /// maximum number of calendars it can handle.</exception>
        [Pure]
        public static Calendar Add(
            string key, SystemSchema schema, DayNumber epoch, bool proleptic)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            if (s_LastIdent >= MaxId) Throw.CatalogOverflow();

            Calendar tmp = ValidateParameters(key, schema, epoch, proleptic);

            // The callback won't be executed until we query Value.
            var lazy = new Lazy<Calendar>(() => CreateCalendar(tmp));

            if (s_CalendarsByKey.TryAdd(key, lazy) == false)
            {
                Throw.KeyAlreadyExists(nameof(key), key);
            }

            // NB: CreateCalendar() is only called NOW.
            var chr = lazy.Value;

            if (chr.Id == Cuid.Invalid)
            {
                // Clean up.
                s_CalendarsByKey.TryRemove(key, out _);

                Throw.CatalogOverflow();
            }

            return chr;
        }

        /// <summary>
        /// Attempts to create a calendar from a (unique) key, a reference epoch
        /// and a calendrical schema, then adds it to the system.
        /// </summary>
        /// <remarks>
        /// <para>If a calendar with the same key already exists, this method
        /// returns null without changing the internal state of this class. See
        /// also <seealso cref="ReservedKeys"/>.</para>
        /// <para>There is a hard limit on the total number of calendars one can
        /// create; see <see cref="MaxNumberOfUserCalendars"/>.</para>
        /// <para>It is the duty of the caller to ensure that repeated calls to
        /// this method are coherent.</para>
        /// <para>This method is thread-safe.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static bool TryAdd(
            string key, SystemSchema schema, DayNumber epoch, bool proleptic,
            [NotNullWhen(true)] out Calendar? calendar)
        {
            // Fail fast. It also guards the method against brute-force attacks.
            if (s_LastIdent >= MaxId) { goto FAILED; }

            // Null parameters validation.
            Requires.NotNull(key);
            Requires.NotNull(schema);

            // Other parameters validation (exceptions catched).
            Calendar tmp;
            try { tmp = ValidateParameters(key, schema, epoch, proleptic); }
            catch (ArgumentException) { goto FAILED; }

            // The callback won't be executed until we query Value.
            var lazy = new Lazy<Calendar>(() => CreateCalendar(tmp));

            if (s_CalendarsByKey.TryAdd(key, lazy))
            {
                // NB: CreateCalendar() is only called NOW.
                var chr = lazy.Value;

                if (chr.Id == Cuid.Invalid)
                {
                    // Clean up.
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

        /// <summary>
        /// Pre-validates the parameters.
        /// </summary>
        /// <returns>A calendar with an ID <see cref="Cuid.Invalid"/>.</returns>
        private static Calendar ValidateParameters(
                string key,
                SystemSchema schema,
                DayNumber epoch,
                bool proleptic)
        {
            // FIXME(code): Invalid was a bad idea. If the Calendar ctor check
            // the value, it will fail hard. It would be the case if for
            // instance we initialized a date,
            // > var date = new CalendarDate(..., id);
            return new Calendar(Cuid.Invalid, key, schema, epoch, proleptic);
        }

        /// <summary>
        /// Creates a new <see cref="Calendar"/> instance from the specified
        /// temporary calendar then add it to <see cref="s_CalendarsById"/>.
        /// <para>Returns a calendar with ID <see cref="Cuid.Invalid"/> if
        /// the system already reached the maximum number of calendars it can
        /// handle.</para>
        /// </summary>
        [Pure]
        private static Calendar CreateCalendar(Calendar tmpCalendar)
        {
            Debug.Assert(tmpCalendar != null);

            // Lazy<T> :
            //   Quelque soit le nombre de fils d'exécution en cours,
            //   CreateCalendar() est exécutée en différé et au plus une fois
            //   (pour une même clé), et ce quand on demande la prop Value.
            //   En conséquence, s_LastIdent ne sera incrémenté qu'une fois.
            //   Bien entendu, avec deux clés différentes, le problème ne se
            //   pose pas.
            int ident = Interlocked.Increment(ref s_LastIdent);
            if (ident > MaxId)
            {
                // We don't throw an OverflowException yet.
                // This will give the caller the opportunity to do some cleanup.
                // Objects of this type only exist for a very short period of
                // time within <see cref="s_CalendarsByKey"/> and MUST be
                // filtered out anytime we query a value from this dictionary.
                return tmpCalendar;
            }

            Debug.Assert(ident <= Byte.MaxValue);

            // Notes:
            // - the cast to Cuid should always succeed.
            // - the ctor won't throw since we already validated the params
            //   when we created tmpCalendar.
            var chr = new Calendar(
                (Cuid)ident,
                tmpCalendar.Key,
                tmpCalendar.Schema,
                tmpCalendar.Epoch,
                tmpCalendar.IsProleptic);

            Debug.Assert(ident < s_CalendarsById.Length);

            return s_CalendarsById[ident] = chr;
        }

        #endregion
    }
}
