// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete;

using System.Collections;

// Par abus de language, on appelle code aussi bien la séquence qu'un de ses
// éléments.

/// <summary>
/// Represents a non-empty finite sequence of codes (positive integers).
/// <para>The collection is read-only.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CodeArray : IReadOnlyList<int>
{
    private readonly int[] _codes;

    // Singleton {n}.
    public CodeArray(int n)
    {
        if (n < 0) Throw.ArgumentOutOfRange(nameof(n));

        Min = Max = n;
        _codes = new int[] { n };
    }

    // Constant {n, n, ...}.
    public CodeArray(int n, int count)
    {
        if (n < 0) Throw.ArgumentOutOfRange(nameof(n));
        if (count < 1) Throw.ArgumentOutOfRange(nameof(count));

        Min = Max = n;
        _codes = ArrayHelpers.Repeat(n, count);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeArray"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="codes"/> is null.</exception>
    public CodeArray(int[] codes)
    {
        Requires.NotNull(codes);

        if (codes.Length == 0 || !Array.TrueForAll(codes, n => n >= 0))
        {
            Throw.Argument(nameof(codes));
        }

        _codes = codes;

        Min = ArrayHelpers.Min(codes);
        Max = ArrayHelpers.Max(codes);
    }

    /// <summary>
    /// Gets the minimum value in the sequence.
    /// </summary>
    public int Min { get; }

    /// <summary>
    /// Gets the maximum value in the sequence.
    /// </summary>
    public int Max { get; }

    /// <summary>
    /// Returns true if the current sequence is constant; otherwise returns false.
    /// </summary>
    public bool Constant => Height == 0;

    /// <summary>
    /// Returns true if the current instance is convertible to a <see cref="BoolArray"/>;
    /// otherwise returns false.
    /// </summary>
    /// <remarks>
    /// <para>It simply means that <see cref="Height"/> is equal to 0 or 1.</para>
    /// </remarks>
    public bool Reducible => Height < 2;

    /// <summary>
    /// Returns true if the current instance is a <i>non-constant</i> sequence convertible to a
    /// <see cref="BoolArray"/>; otherwise returns false.
    /// </summary>
    public bool StrictlyReducible => Height == 1;

    /// <summary>
    /// Gets the difference between the maximum and the minimum values in the sequence.
    /// </summary>
    public int Height => Max - Min;

    // TODO: test. Arg validation.
    public CodeArray Slice(int start, int length)
    {
        var slice = new int[length];
        Array.Copy(_codes, start, slice, 0, length);
        return new CodeArray(slice);
    }

    #region IReadOnlyList<>

    /// <inheritdoc />
    public int Count => _codes.Length;

    /// <inheritdoc />
    public int this[int index] => _codes[index];

    /// <inheritdoc />
    [Pure]
    public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)_codes).GetEnumerator();

    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => _codes.GetEnumerator();

    #endregion
}

public partial class CodeArray // Conversions, manips
{
    // We say that a code is almost reducible if it is not reducible,
    // and if by just removing a single element the resulting code turns out
    // to be reducible.
    //
    // Ambiguous case: not reducible and Count == 2.
    // For instance, with the pair {4, 2}, we can truncate the
    // array to either {4} or {2}, both are reducible, but which one to pick?
    // We choose to keep the first one.
    //
    // When the operation is not successful, start = -1 and newCode = null.
    // When the operation is successful, start >= 0 and newCode is a
    // reducible code.
    //   start = 0,
    //   - newCode = code[0..^1].
    //   - the removed code is code[^1] (= Min or Max).
    //   start > 0,
    //   - newCode = code.Rotate(start)[0..^1].
    //   - the removed code is code[start - 1] (= Min or Max).
    //
    // Reverse operation.
    //   // Assuming that "code" is almost reducible.
    //   code.IsAlmostReducible(out var newCode, out int start);
    //   if (start == 0)
    //   {
    //     // code0 == code.
    //     var code0 = newCode.Append(code[^1]);
    //   }
    //   else
    //   {
    //     int i = start - 1;
    //     // code0 == code.
    //     var code0 = newCode.Append(code[i]).Rotate(newCode.Count - i);
    //   }
    [Pure]
    public bool IsAlmostReducible([NotNullWhen(true)] out CodeArray? newCode, out int start)
    {
        if (Reducible)
        {
            goto NOT_ALMOST_REDUCIBLE;
        }
        else if (Count == 2)
        {
            start = 0;
            newCode = new CodeArray(this[0]);
            return true;
        }
        else if (TryFindIndexOfExceptionalCode(out int index))
        {
            start = (index + 1) % Count;
            newCode = start == 0 ? this[0..^1] : Rotate(start)[0..^1];
            Debug.Assert(newCode.Reducible);
            return true;
        }
        else
        {
            // There is more than one "exceptional" code.
            goto NOT_ALMOST_REDUCIBLE;
        }

    NOT_ALMOST_REDUCIBLE:
        start = -1;
        newCode = null;
        return false;

        // KEEP both versions.
        // - The first one does the search using a single loop but is a bit
        //   cryptic.
        // - The second one contains the explanations and I'm very confident
        //   in the code.
#if true
        [Pure]
        bool TryFindIndexOfExceptionalCode(out int index)
        {
            // Cache the props before entering the loop.
            int min = Min;
            int max = Max;
            int maxThreshold = min + 1;
            int minThreshold = max - 1;

            bool koMin = false;
            bool koMax = false;
            int indexOfMin = -1;
            int indexOfMax = -1;
            for (int i = 0; i < _codes.Length; i++)
            {
                int c = _codes[i];

                if (koMax == false && c > maxThreshold)
                {
                    if (c == max && indexOfMax == -1)
                    {
                        indexOfMax = i;
                    }
                    else
                    {
                        koMax = true;
                    }
                }

                if (koMin == false && c < minThreshold)
                {
                    if (c == min && indexOfMin == -1)
                    {
                        indexOfMin = i;
                    }
                    else
                    {
                        koMin = true;
                    }
                }

                if (koMin && koMax)
                {
                    index = -1;
                    return false;
                }
            }

            index = koMin ? indexOfMax : indexOfMin;
            return true;
        }
#else
        [Pure]
        bool TryFindIndexOfExceptionalCode(out int index) =>
            TryFindIndexOfExceptionalMin(out index)
            || TryFindIndexOfExceptionalMax(out index);

        // Succeeds if we have a sequence of Min's or (Min + 1)'s plus
        // exactly one exceptional code, necessary equal to Max (the code
        // array is not reducible):
        // - Min's, (Min + 1)'s and one Max
        // - Min's and one Max
        [Pure]
        bool TryFindIndexOfExceptionalMax(out int index)
        {
            // Cache the props before entering the loop.
            int max = Max;
            int threshold = Min + 1;

            index = -1;
            for (int i = 0; i < _codes.Length; i++)
            {
                int c = _codes[i];
                // We already know that there is at least one code >= Min + 2,
                // what we don't know is if it is the only one.
                if (c > threshold)
                {
                    if (c == max && index == -1)
                    {
                        // First time.
                        index = i;
                    }
                    else
                    {
                        // Second time we encountered Max, or the current
                        // code (c) can't be THE exceptional code: there are
                        // at least two deviant codes: c and Max.
                        index = -1;
                        break;
                    }
                }
            }

            return index != -1;
        }

        // Succeeds if we have a sequence of (Max - 1)'s or Max's plus
        // exactly one exceptional code, necessary equal to Min (the code
        // array is not reducible):
        // - (Max - 1)'s, Max's and one Min
        // - Max's and one Min
        [Pure]
        bool TryFindIndexOfExceptionalMin(out int index)
        {
            // Cache the props before entering the loop.
            int min = Min;
            int threshold = Max - 1;

            index = -1;
            for (int i = 0; i < _codes.Length; i++)
            {
                int c = _codes[i];
                // We already know that there is at least one code <= Max - 2,
                // what we don't know is if it is the only one.
                if (c < threshold)
                {
                    if (c == min && index == -1)
                    {
                        // First time.
                        index = i;
                    }
                    else
                    {
                        // Second time we encountered Min, or the current
                        // code (c) can't be THE exceptional code: there are
                        // at least two deviant codes: Min and c.
                        index = -1;
                        break;
                    }
                }
            }

            return index != -1;
        }
#endif
    }

    // Only applicable when the sequence is reducible.
    [Pure]
    public BoolArray ToBoolArray()
    {
        if (Reducible == false) Throw.InvalidOperation();

        int min = Min;
        var arr = new bool[_codes.Length];
        for (int i = 0; i < _codes.Length; i++)
        {
            arr[i] = (_codes[i] - min) == 1;
        }

        return new BoolArray(arr);
    }

    // REVIEW: public?
    // NB: l'opération inverse n'a pas vraiment de sens.
    // En effet une forme "constante" correspond à plusieurs codes.
    // Par ex., (4, 1, 0) correspond aussi bien au singleton {4} qu'aux
    // suites {4, 4}, {4, 4, 4}, etc.
    // InvalidOperationException
    [Pure]
    internal QuasiAffineForm ToQuasiAffineForm() =>
        Constant ? new QuasiAffineForm(Min, 1, 0) : Throw.InvalidOperation<QuasiAffineForm>();

    [Pure]
    public CodeArray Rotate(int start)
    {
        if (start <= 0 || Count <= start) Throw.ArgumentOutOfRange(nameof(start));

        return new(ArrayHelpers.Rotate(_codes, start));
    }

    [Pure]
    public CodeArray Append(int value)
    {
        if (value < 0) Throw.ArgumentOutOfRange(nameof(value));

        var arr = new int[Count + 1];
        Array.Copy(_codes, arr, Count);
        arr[^1] = value;
        return new CodeArray(arr);
    }
}
