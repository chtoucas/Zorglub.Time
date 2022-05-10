// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Extensions
{
    // We provide two methods to <i>explicitely</i> unbox a boxed object.
    // Explicitely because one can do it indirectly, e.g. via Select().
    // Unboxing should only be done by developers of new core types, this is
    // the main reason why we prefer extension methods to instance methods ---
    // Box<T> has only one public method (Select), if Unbox() and TryUnbox()
    // were instance methods, it would be like encouraging developers to use
    // them, defeating the "raison d'être" of this class.
    //
    // For better alternatives, see BoxExtensions in Simple.

    /// <summary>
    /// Provides extension methods for <see cref="Box{T}"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public static class Unboxing
    {
        /// <summary>
        /// Obtains the enclosed object.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        /// <exception cref="InvalidOperationException">This box is empty.</exception>
        [Pure]
        public static T Unbox<T>(this Box<T> @this!!)
            where T : class
        {
            return @this.IsEmpty ? Throw.EmptyBox<T>() : @this.Content;
        }

        /// <summary>
        /// Attempts to obtain the enclosed object.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is null.</exception>
        [Pure]
        public static bool TryUnbox<T>(this Box<T> @this!!, [NotNullWhen(true)] out T? obj)
            where T : class
        {
            if (@this.IsEmpty)
            {
                obj = null;
                return false;
            }
            else
            {
                obj = @this.Content;
                return true;
            }
        }
    }
}
