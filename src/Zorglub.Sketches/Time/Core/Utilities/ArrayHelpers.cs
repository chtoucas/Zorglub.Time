// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    /// <summary>
    /// Provides helpers for <see cref="Array"/>.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    internal static class ArrayHelpers
    {
        [Pure]
        public static T[] Repeat<T>(T value, int count)
        {
            var arr = new T[count];
            for (int i = 0; i < count; i++) { arr[i] = value; }
            return arr;
        }

        [Pure]
        public static T[] Rotate<T>(T[] array, int start)
        {
            Debug.Assert(array != null);
            // Not mandatory, but then we would have to use the true math modulo.
            // We also exclude 0, since Rotate() would do nothing.
            Debug.Assert(start > 0);
            // This one too is not mandatory, but it seems more natural to
            // satisfy this condition too.
            Debug.Assert(start < array.Length);

            var len = array.Length;
            var arr = new T[len];
            for (int i = 0; i < len; i++)
            {
                arr[i] = array[(i + start) % len];
            }
            return arr;
        }

        [Pure]
        public static int[] ConvertToCumulativeArray(int[] array)
        {
            Debug.Assert(array != null);

            var len = array.Length;
            int[] arr = new int[len + 1];

            arr[0] = 0;
            for (int i = 0; i < len; i++)
            {
                arr[i + 1] = arr[i] + array[i];
            }
            return arr;
        }

        [Pure]
        public static int Min(int[] array)
        {
            Debug.Assert(array != null);
            Debug.Assert(array.Length > 0);

            int min = array[0];
            int i = 0;
            while (++i < array.Length)
            {
                int n = array[i];
                if (n < min) { min = n; }
            }
            return min;
        }

        [Pure]
        public static int Max(int[] array)
        {
            Debug.Assert(array != null);
            Debug.Assert(array.Length > 0);

            int max = array[0];
            int i = 0;
            while (++i < array.Length)
            {
                int n = array[i];
                if (n > max) { max = n; }
            }
            return max;
        }
    }
}
