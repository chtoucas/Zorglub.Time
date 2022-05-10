// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the empty iterator.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    [DebuggerNonUserCode, DebuggerDisplay("Count = 0")]
    internal sealed class EmptyIterator<T> : IEnumerator<T>
    {
        public static readonly IEnumerator<T> Instance = new EmptyIterator<T>();

        private EmptyIterator() { }

        // No one should ever call this property.
        [ExcludeFromCodeCoverage]
        public T Current => Throw.InvalidOperation<T>();

        [ExcludeFromCodeCoverage]
        object? IEnumerator.Current => default;

        [Pure] public bool MoveNext() => false;

        void IEnumerator.Reset() { }
        void IDisposable.Dispose() { }
    }
}
