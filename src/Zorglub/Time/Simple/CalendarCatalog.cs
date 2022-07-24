// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Simple
{
    using System.Linq;
    using System.Runtime.InteropServices;

    using Zorglub.Time.Core;

    // All calendars are "apparent" singletons: whether a calendar has been fully
    // constructed or not before, a call to (Try)GetCalendar() ALWAYS returns
    // the same instance.
    //
    // Internally, we use three collections:
    // - s_Registry: all calendars indexed by their key. Not the fastest, but
    //   enforces the unicity of keys -and- ensures that ops are thread-safe.
    // - s_Calendars: all calendars indexed by their ID.
    // - s_SystemCalendars: system calendars indexed by their ID, i.e. the
    //   immutable part of s_Calendars.
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
        // WARNING: the order of the static fields matters.

        /// <summary>
        /// Represents the (immutable) array of system calendars, indexed by their internal IDs.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Calendar[] s_SystemCalendars = InitializeSystemCalendars();

        /// <summary>
        /// Represents the array of fully constructed calendars, indexed by their internal IDs.
        /// <para>A value is null if the calendar has not yet been fully constructed (obviously).
        /// </para>
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly Calendar?[] s_Calendars = InitializeCalendars(s_SystemCalendars);

        /// <summary>
        /// Represents the registry.
        /// <para>This field is read-only.</para>
        /// </summary>
        private static readonly CalendarRegistry s_Registry =
            new(s_SystemCalendars)
            {
                CalendarCreated = chr => s_Calendars[(int)chr.Id] = chr,
            };

        /// <summary>
        /// Gets the minimum value for the ID of a user-defined calendar.
        /// <para>This property ALWAYS returns 64.</para>
        /// </summary>
        internal static int MinUserId
        {
            get
            {
                Debug.Assert(s_Registry.MinId == (int)Cuid.MinUser);
                return s_Registry.MinId;
            }
        }

        /// <summary>
        /// Gets the absolute maximum value for the ID of a calendar.
        /// <para>This property ALWAYS returns 127.</para>
        /// </summary>
        internal static int MaxId
        {
            get
            {
                Debug.Assert(s_Registry.MaxId == (int)Cuid.Max);
                return s_Registry.MaxId;
            }
        }

        /// <summary>
        /// Gets the absolute maximum number of user-defined calendars.
        /// <para>This static property ALWAYS returns 64.</para>
        /// </summary>
        public static int MaxNumberOfUserCalendars => s_Registry.MaxNumberOfUserCalendars;

        /// <summary>
        /// Returns true if the catalog is full; otherwise returns false.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static bool IsFull => s_Registry.IsFull;

        /// <summary>
        /// Gets the list of keys of all fully constructed calendars at the time of the request.
        /// <para>This collection may contain dirty keys, those paired with a calendar with an ID
        /// equal to <see cref="Cuid.Invalid"/>.</para>
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        internal static ICollection<string> Keys =>
            // We do not provide a public equivalent to this property.
            // First, it may contain dirty keys.
            // Second, ConcurrentDictionary.Keys (and therefore Keys) returns a
            // snapshot of the collection of keys which might be surprising as
            // collection properties are expected to return live collection.
            // Furthermore it kind of discloses the internal implementation.
            // Third, we have an indirect (better?) alternative: TakeSnapshot().
            s_Registry.Keys;

        /// <summary>
        /// Gets the collection of reserved keys.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static ReadOnlySet<string> ReservedKeys { get; } =
            new ReadOnlySet<string>(from chr in s_SystemCalendars select chr.Key);

        /// <summary>
        /// Gets the collection of system calendars.
        /// <para>This static property is thread-safe.</para>
        /// </summary>
        public static IReadOnlyCollection<Calendar> SystemCalendars { get; } =
            Array.AsReadOnly(s_SystemCalendars);

        [Pure]
        private static Calendar[] InitializeSystemCalendars()
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

        [Pure]
        private static Calendar?[] InitializeCalendars(Calendar[] systemCalendars)
        {
            var arr = new Calendar?[1 + (int)Cuid.Max];
            Array.Copy(systemCalendars, arr, systemCalendars.Length);
            return arr;
        }
    }

    public partial class CalendarCatalog // Snapshots
    {
        /// <summary>
        /// Obtains the collection of all calendars at the time of the request.
        /// </summary>
        [Pure]
        public static IReadOnlyCollection<Calendar> GetAllCalendars()
        {
            // Fast track.
            if (s_Registry.IsPristine) { return SystemCalendars; }

            // Same as s_CalendarById but without the null's.
            // NB: the code works even if s_Calendars changed in the
            // meantime. We return a snapshot of s_Calendars at the point
            // of time when we compute "usr".
            int usr = s_Registry.CountUserCalendars();
            int sys = s_SystemCalendars.Length;
            var arr = new Calendar[sys + usr];
            // Copy system calendars.
            Array.Copy(s_Calendars, arr, sys);
            // Copy user-defined calendars.
            Array.Copy(s_Calendars, MinUserId, arr, sys, usr);

            return Array.AsReadOnly(arr);
        }

        /// <summary>
        /// Obtains the collection of user-defined calendars at the time of the request.
        /// </summary>
        [Pure]
        public static IReadOnlyCollection<Calendar> GetUserCalendars()
        {
            // Fast track.
            if (s_Registry.IsPristine) { return Array.Empty<Calendar>(); }

            int usr = s_Registry.CountUserCalendars();
            var arr = new Calendar[usr];
            Array.Copy(s_Calendars, MinUserId, arr, 0, usr);

            return Array.AsReadOnly(arr);
        }

        /// <summary>
        /// Takes a snapshot of the collection of calendars indexed by their key.
        /// </summary>
        [Pure]
        public static IReadOnlyDictionary<string, Calendar> TakeSnapshot() =>
            s_Registry.TakeSnapshot();
    }

    public partial class CalendarCatalog // Lookup
    {
        /// <summary>
        /// Looks up a calendar by its unique key.
        /// </summary>
        /// <remarks>
        /// <para>Repeated calls to this method with the same parameter ALWAYS return the same
        /// instance.</para>
        /// <para>See also <seealso cref="TakeSnapshot"/>.</para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">A calendar with the specified
        /// <paramref name="key"/> could not be found.</exception>
        [Pure]
        public static Calendar GetCalendar(string key) => s_Registry.GetCalendar(key);

        /// <summary>
        /// Attempts to look up a calendar by its unique key.
        /// </summary>
        /// <remarks>
        /// <para>See also <seealso cref="TakeSnapshot"/>.</para>
        /// </remarks>
        [Pure]
        public static bool TryGetCalendar(string key, [NotNullWhen(true)] out Calendar? calendar) =>
            s_Registry.TryGetCalendar(key, out calendar);

        /// <summary>
        /// Looks up a <i>system</i> calendar by its unique identifier.
        /// </summary>
        /// <exception cref="AoorException">The specified ID is not valid.</exception>
        /// <remarks>
        /// <para>Repeated calls to this method with the same parameter ALWAYS return the same
        /// instance.</para>
        /// </remarks>
        [Pure]
        public static Calendar GetSystemCalendar(CalendarId ident)
        {
            // REVIEW(code): just use ident.IsInvalid()?
            int index = (int)ident;
            if (index < 0 || (uint)index >= (uint)s_SystemCalendars.Length)
            {
                Throw.ArgumentOutOfRange(nameof(ident));
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
        /// <para>Repeated calls to this method with the same parameter ALWAYS return the same
        /// instance.</para>
        /// </remarks>
        [Pure]
        internal static Calendar GetCalendarUnchecked(int cuid)
        {
            Debug.Assert(cuid < s_Calendars.Length);

            return s_Calendars[cuid];
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
            Debug.Assert(cuid < s_Calendars.Length);

            // Array bound elimination.
            // We can remove all array bound checks. The actual perf boost is
            // really tiny, nevertheless the generated code is much smaller.
            // WARNING: it removes array variance checks.
            // See https://github.com/CommunityToolkit/dotnet/blob/main/CommunityToolkit.HighPerformance/Extensions/ArrayExtensions.1D.cs
            // and https://tooslowexception.com/getting-rid-of-array-bound-checks-ref-returns-and-net-5/
            ref Calendar chr = ref MemoryMarshal.GetArrayDataReference(s_Calendars);
            return ref Unsafe.Add(ref chr, (nint)(uint)cuid);
        }

#nullable restore warnings
    }

    public partial class CalendarCatalog // Add
    {
        /// <summary>
        /// Creates a calendar from a (unique) key, a reference epoch and a calendrical schema, then
        /// adds it to the system.
        /// </summary>
        /// <remarks>
        /// <para>If a calendar with the same key already exists, this method ignores the other
        /// parameters and returns it, without changing the internal state of this class. See also
        /// <seealso cref="ReservedKeys"/>.</para>
        /// <para>There is a hard limit on the total number of calendars one can create; see
        /// <see cref="MaxNumberOfUserCalendars"/>.</para>
        /// <para>It is the duty of the caller to ensure that repeated calls to this method are
        /// coherent.</para>
        /// <para>This method is thread-safe.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1..9999] (non-proleptic case)
        /// or [-9998..9999] (proleptic case).</exception>
        /// <exception cref="OverflowException">The key was not already taken but the system already
        /// reached the maximum number of calendars it can handle.</exception>
        [Pure]
        public static Calendar GetOrAdd(
            string key, SystemSchema schema, DayNumber epoch, bool proleptic) =>
            s_Registry.GetOrAdd(key, schema, epoch, proleptic);

        /// <summary>
        /// Creates a calendar from a (unique) key, a reference epoch and a calendrical schema, then
        /// adds it to the system.
        /// </summary>
        /// <remarks>
        /// <para>There is a hard limit on the total number of calendars one can create; see
        /// <see cref="MaxNumberOfUserCalendars"/>.</para>
        /// <para>It is the duty of the caller to ensure that repeated calls to this method are
        /// coherent.</para>
        /// <para>This method is thread-safe.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        /// <exception cref="ArgumentException">A calendar with the same key already exists. See
        /// also <seealso cref="ReservedKeys"/>.</exception>
        /// <exception cref="ArgumentException">The range of supported years by
        /// <paramref name="schema"/> does not contain the interval [1..9999] (non-proleptic case)
        /// or [-9998..9999] (proleptic case).</exception>
        /// <exception cref="OverflowException">The system already reached the maximum number of
        /// calendars it can handle.</exception>
        [Pure]
        public static Calendar Add(
            string key, SystemSchema schema, DayNumber epoch, bool proleptic) =>
            s_Registry.Add(key, schema, epoch, proleptic);

        /// <summary>
        /// Attempts to create a calendar from a (unique) key, a reference epoch and a calendrical
        /// schema, then adds it to the system.
        /// </summary>
        /// <remarks>
        /// <para>If a calendar with the same key already exists, this method returns null without
        /// changing the internal state of this class. See also <seealso cref="ReservedKeys"/>.</para>
        /// <para>There is a hard limit on the total number of calendars one can create; see
        /// <see cref="MaxNumberOfUserCalendars"/>.</para>
        /// <para>It is the duty of the caller to ensure that repeated calls to this method are
        /// coherent.</para>
        /// <para>This method is thread-safe.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.</exception>
        [Pure]
        public static bool TryAdd(
            string key, SystemSchema schema, DayNumber epoch, bool proleptic,
            [NotNullWhen(true)] out Calendar? calendar) =>
            s_Registry.TryAdd(key, schema, epoch, proleptic, out calendar);
    }
}
