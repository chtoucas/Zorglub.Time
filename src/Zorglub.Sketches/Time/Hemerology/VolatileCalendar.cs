// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Hemerology
{
    using System.Threading;

    using Zorglub.Time.Core;

    // https://stackoverflow.com/questions/2047591/compacting-a-weakreference-dictionary
    // https://gist.github.com/qwertie/3867055
    // Alt impl: WeakReference? see comments in Dispose().
    //   ConditionalWeakTable
    //     https://github.com/dotnet/runtime/issues/12255
    //   https://source.dot.net/#WindowsBase/WeakReferenceList.cs,b3d8afef5cd18c91
    //   https://stackoverflow.com/questions/15593/practical-use-of-system-weakreference
    //   http://www.philosophicalgeek.com/2014/09/03/practical-uses-of-weakreference/
    //   https://www.codeproject.com/Articles/1058549/Prefer-WeakReference-to-WeakReference-T
    //   https://www.codeproject.com/articles/664282/understanding-weak-references-in-net
    //   https://www.codeproject.com/articles/43042/weakreferences-gchandles-and-weakarrays
    //   https://www.codeproject.com/articles/35152/weakreferences-as-a-good-caching-mechanism
    // ID
    //   https://www.nimaara.com/generating-ids-in-csharp/

    internal sealed class VolatileIdProvider
    {
        private static int s_LastIdent = WideCatalog.MaxId;

        // FIXME: au bout d'un moment, ça va planter.
        // ThreadStatic
        // ConcurrentQueue
#pragma warning disable CA1822 // Mark members as static (Performance)
        public int Next()
#pragma warning restore CA1822 // Mark members as static
        {
            int ident = Interlocked.Increment(ref s_LastIdent);
            if (ident > UInt16.MaxValue) Throw.InvalidOperation();

            return ident;
        }
    }

    /// <summary>
    /// Represents a transient (wide) calendar.
    /// <para>A transient calendar is not part of the <see cref="WideCatalog"/>
    /// system. As such, one cannot use
    /// <see cref="WideCatalog.GetCalendar(string)"/> or
    /// <see cref="WideCatalog.TryGetCalendar(string, out WideCalendar?)"/>.</para>
    /// </summary>
    public sealed class VolatileCalendar : WideCalendar, IDisposable
    {
        private static readonly Dictionary<int, VolatileCalendar> s_TransientCalendars = new();

        private static readonly VolatileIdProvider _idProvider = new();

        private bool _disposed;

        public VolatileCalendar(
            string name,
            ICalendricalSchema schema,
            DayNumber epoch,
            bool widest)
            : this(
                  _idProvider.Next(),
                  name,
                  schema,
                  epoch,
                  widest)
        { }

        private VolatileCalendar(
            int ident,
            string name,
            ICalendricalSchema schema,
            DayNumber epoch,
            bool widest)
            : base(ident, name, schema, epoch, widest, userDefined: true)
        {
            IsVolatile = true;
            s_TransientCalendars.Add(ident, this);
        }

        /// <summary>
        /// Looks up a transient calendar by its internal ID.
        /// </summary>
        internal static VolatileCalendar ForId(int cuid)
        {
            Debug.Assert(cuid > WideCatalog.MaxId);

            return s_TransientCalendars[cuid];

            //if (s_TransientCalendars.TryGetValue(cuid, out VolatileCalendar? chr))
            //{
            //    return chr;
            //}

            //Throw.InvalidOperation<VolatileCalendar>();
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            // When deterministically called, we try to completely delete any
            // reference to the current instance.
            // If someone forgets to use the "using" statement, I don't think
            // we can do anything; the reference will never be GCed because of
            // the reference to it found in s_TransientCalendars.

            if (!_disposed)
            {
                s_TransientCalendars.Remove(Id);

                _disposed = true;
            }
        }

        #endregion
    }
}
