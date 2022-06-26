// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete
{
    using System.Collections;

    // REVIEW: use a BitArray or a BitVector32?

    /// <summary>
    /// Represents a non-empty finite sequence of booleans.
    /// <para>The collection is read-only.</para>
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed partial class BoolArray : IReadOnlyList<bool>
    {
        private readonly bool[] _arr;

        public BoolArray(bool b)
        {
            _arr = new bool[] { b };
        }

        public BoolArray(bool b, int count)
        {
            if (count < 1) Throw.ArgumentOutOfRange(nameof(count));

            _arr = ArrayHelpers.Repeat(b, count);
        }

        public BoolArray(bool[] arr)
        {
            Requires.NotNull(arr);

            if (arr.Length == 0) Throw.Argument(nameof(arr));

            _arr = arr;
        }

        #region IReadOnlyList<>

        /// <inheritdoc />
        public int Count => _arr.Length;

        /// <inheritdoc />
        public bool this[int index] => _arr[index];

        /// <inheritdoc />
        [Pure]
        public IEnumerator<bool> GetEnumerator() => ((IEnumerable<bool>)_arr).GetEnumerator();

        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => _arr.GetEnumerator();

        #endregion
    }

    public partial class BoolArray
    {
        /// <summary>
        /// Returns true if the value true is isolated in the sequence; otherwise returns false.
        /// </summary>
        /// <remarks>
        /// <para>The value true is said to be isolated if the sequence does NOT contain a
        /// subsequence {true, true}.</para>
        /// <para>Beware, this method returns false for the singleton {1}.</para>
        /// </remarks>
        [Pure]
        public bool IsTrueIsolated()
        {
            Debug.Assert(_arr.Length > 0);

            bool prev = _arr[0];

            int i = 1;
            while (i < _arr.Length)
            {
                bool curr = _arr[i];

                if (prev && curr) { return false; }

                i++;
                prev = curr;
            }

            return true;
        }

        /// <summary>
        /// Returns a new <see cref="BoolArray"/> with true and false exchanged.
        /// </summary>
        [Pure]
        public BoolArray Negate()
        {
            var newArr = new bool[_arr.Length];
            for (int i = 0; i < _arr.Length; i++)
            {
                newArr[i] = !_arr[i];
            }
            return new BoolArray(newArr);
        }

        [Pure]
        public SliceArray Slice()
        {
            Debug.Assert(_arr.Length > 0);

            var slices = new List<int>(_arr.Length);

            int len = 1;
            for (int i = 0; i < _arr.Length - 1; i++)
            {
                if (_arr[i])
                {
                    slices.Add(len);
                    len = 1;
                }
                else
                {
                    len++;
                }
            }

            // NB: since we use (_arr.Length - 1) in the above loop, it works
            // even if there is only one element in _arr.
            if (_arr[^1])
            {
                slices.Add(len);
                return new SliceArray(slices.ToArray(), complete: true);
            }
            else
            {
                slices.Add(++len);
                return new SliceArray(slices.ToArray(), complete: false);
            }
        }
    }
}
