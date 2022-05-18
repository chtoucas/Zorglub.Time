// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    // See https://github.com/CommunityToolkit/dotnet/blob/main/CommunityToolkit.Diagnostics/Guard.cs
    // See also https://docs.microsoft.com/en-us/dotnet/api/microsoft.assumes?view=visualstudiosdk-2022

    /// <summary>
    /// Provides helper methods to specify preconditions on a method.
    /// <para>If a condition does not hold, an exception is thrown.</para>
    /// </summary>
    [DebuggerStepThrough]
    internal static class Requires
    {
        /// <summary>
        /// Validates that the specified value is not null.
        /// </summary>
        /// <typeparam name="T">The type of reference value type being tested.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.
        /// </exception>
        // CIL code size = 16 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNull<T>(
            [NotNull] T? obj,
            [CallerArgumentExpression("obj")] string paramName = "")
            where T : notnull
        {
            // NB: there is also ArgumentNullException.ThrowIfNull().
            if (obj is not null) return;

            Throw.ArgumentNull(paramName);
        }

        /// <summary>
        /// Validates that the specified value is a member of the enum <see cref="DayOfWeek"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="dayOfWeek"/> was not
        /// a known member of the enum <see cref="DayOfWeek"/>.</exception>
        // CIL code size = 16 bytes <= 32 bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Defined(
            DayOfWeek dayOfWeek,
            [CallerArgumentExpression("dayOfWeek")] string paramName = "")
        {
            if (DayOfWeek.Sunday <= dayOfWeek && dayOfWeek <= DayOfWeek.Saturday) return;

            Throw.DayOfWeekOutOfRange(dayOfWeek, paramName);
        }

        /// <summary>
        /// Validates that the specified schema has the <paramref name="expected"/> profile.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if <paramref name="schema"/> did not have
        /// the expected profile.</exception>
        public static void Profile(
            CalendricalSchema schema,
            CalendricalProfile expected,
            [CallerArgumentExpression("schema")] string paramName = "")
        {
            Debug.Assert(schema != null);

            if (schema.Profile == expected) return;

            Throw.BadSchemaProfile(paramName, expected, schema.Profile);
        }
    }
}
