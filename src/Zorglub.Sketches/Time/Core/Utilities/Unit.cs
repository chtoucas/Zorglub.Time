// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    // We do not actually use this type, but it can be handy in combination
    // with Box<>.
    // See https://en.wikipedia.org/wiki/Unit_type

    /// <summary>
    /// Represents a Unit type.
    /// <para><see cref="Unit"/> is an immutable struct.</para>
    /// </summary>
    [Serializable]
    public readonly struct Unit :
        IEqualityOperators<Unit, Unit>,
        IEqualityOperators<Unit, ValueTuple>
    {
        /// <summary>
        /// Represents the singleton instance of the <see cref="Unit"/> struct.
        /// <para>This field is read-only.</para>
        /// </summary>
        public static readonly Unit Value;

        /// <summary>
        /// Returns a string representation of the current instance.
        /// </summary>
        [Pure]
        public override string ToString() => "()";

#pragma warning disable IDE0060 // Remove unused parameter (Style)

        /// <summary>
        /// Always returns true.
        /// </summary>
        public static bool operator ==(Unit left, Unit right) => true;

        /// <summary>
        /// Always returns false.
        /// </summary>
        public static bool operator !=(Unit left, Unit right) => false;

        /// <summary>
        /// Always returns true.
        /// </summary>
        public static bool operator ==(Unit left, ValueTuple right) => true;

        /// <summary>
        /// Always returns true.
        /// </summary>
        public static bool operator ==(ValueTuple left, Unit right) => true;

        /// <summary>
        /// Always returns false.
        /// </summary>
        public static bool operator !=(Unit left, ValueTuple right) => false;

        /// <summary>
        /// Always returns false.
        /// </summary>
        public static bool operator !=(ValueTuple left, Unit right) => false;

#pragma warning restore IDE0060

        /// <summary>
        /// Always returns true.
        /// </summary>
        [Pure]
        public bool Equals(Unit other) => true;

        /// <summary>
        /// Always returns true.
        /// </summary>
        [Pure]
        public bool Equals(ValueTuple other) => true;

        /// <inheritdoc />
        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Unit || obj is ValueTuple;

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => 0;
    }
}
