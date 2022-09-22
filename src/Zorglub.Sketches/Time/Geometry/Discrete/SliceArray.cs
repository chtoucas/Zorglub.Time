// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Discrete;

using System.Collections;
using System.Linq;

/// <summary>
/// Represents a non-empty finite sequence of slices, or more correctly their lengths.
/// <para>A well-formed sequence of slices is a sequence (empty or not) of complete slices maybe
/// followed by a terminal slice; see the remarks for an explanation.</para>
/// <para>The collection is read-only.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
/// <remarks>
/// <para>Slices are never empty, and they are either complete or truncated:</para>
/// <list type="bullet">
/// <item>A <i>complete</i> slice is a finite sequence of zeroes (possibly empty) followed by
/// one 1; the shortest complete slice is the singleton {1}.</item>
/// <item>A <i>truncated</i> slice is a non-empty finite sequence of zeroes; the shortest
/// truncated slice is the singleton {0}. <i>Only the last slice can be truncated.</i></item>
/// <item>The first slice in a sequence is said to be <i>initial</i> if, and only if, it is
/// complete.
/// <para>There is only one case where there is no initial slice, it is whenthe first slice is
/// truncated (which happens to be also the last one).</para>
/// </item>
/// <item>The last slice in a sequence is said to be <i>terminal</i> if, and only if, it is
/// truncated.</item>
/// <item>An <i>internal</i> slice is a slice that is neither initial nor terminal; otherwise it
/// is said to be <i>external</i>.</item>
/// <item>An external slice is said to be <i>major</i> when it is strictly longer than the
/// shortest internal slice; otherwise it is said to be <i>minor</i>.
/// <para>If there are no internal slices, only the longest external slice is major, unless they
/// have the same length, in which case both are major.</para>
/// </item>
/// </list>
/// </remarks>
public sealed partial class SliceArray : IReadOnlyList<int>, IEquatable<SliceArray>
{
    /// <summary>
    /// Represents the array of (lengths of) slices.
    /// <para>This field is read-only.</para>
    /// </summary>
    private readonly int[] _slices;

    /// <summary>
    /// Initializes a new instance of the <see cref="SliceArray"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="slices"/> is null.</exception>
    public SliceArray(int[] slices, bool complete)
    {
        Requires.NotNull(slices);

        if (slices.Length == 0
            // No empty slice: all elements must be > 0.
            || !Array.TrueForAll(slices, n => n > 0))
        {
            Throw.Argument(nameof(slices));
        }

        _slices = slices;
        Complete = complete;
    }

    /// <summary>
    /// Returns true if the last slice is complete; otherwise returns false.
    /// <para>The last slice is NOT terminal.</para>
    /// </summary>
    public bool Complete { get; }

    /// <summary>
    /// Returns true if the last slice is truncated; otherwise returns false.
    /// <para>The last slice is terminal.</para>
    /// </summary>
    private bool Truncated => !Complete;

    /// <summary>
    /// Gets the initial slice.
    /// </summary>
    private int InitialSlice { get { Debug.Assert(Count > 1 || Complete); return _slices[0]; } }

    /// <summary>
    /// Gets the terminal slice.
    /// </summary>
    private int TerminalSlice { get { Debug.Assert(Truncated); return _slices[^1]; } }

    #region IReadOnlyList<>

    /// <inheritdoc />
    public int Count => _slices.Length;

    /// <inheritdoc />
    public int this[int index] => _slices[index];

    /// <inheritdoc />
    [Pure]
    public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)_slices).GetEnumerator();

    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => _slices.GetEnumerator();

    #endregion
}

public partial class SliceArray
{
    /// <summary>
    /// Obtains the number of internal slices.
    /// </summary>
    [Pure]
    public int CountInternalSlices()
    {
        Debug.Assert(Count > 0);

        return Count == 1
            // One external slice.
            ? 0
            // Two slices or more: an initial slice + zero or more internal
            // slice(s) (+ terminal slice).
            : Count - 1 - (Complete ? 0 : 1);
    }

    /// <summary>
    /// Converts the current instance to a <see cref="CodeArray"/> while removing the minor
    /// external slices.
    /// <para>When the initial slice is minor, its length is given in an output parameter;
    /// otherwise <paramref name="g"/> is set to 0.</para>
    /// </summary>
    [Pure]
    public CodeArray RemoveMinorExternals(out int g)
    {
        Debug.Assert(Count > 0);

        // Degenerate case of first kind.
        // One external slice, initial or terminal.
        if (Count == 1)
        {
            g = 0;
            return new CodeArray(_slices[0]);
        }

        return
            // An initial slice and at least one internal slice.
            Complete ? ConvertWhenComplete(out g)
            // Degenerate case of second kind.
            // Two external slices, no internal slice.
            : Count == 2 ? ConvertWhenDegenerate(out g)
            // Two external slices and at least one internal slice.
            : ConvertWhenTruncated(out g);
    }

    [Pure]
    private CodeArray ConvertWhenComplete(out int g)
    {
        Debug.Assert(Complete);
        Debug.Assert(Count >= 2);

        int init = InitialSlice;
        int[] interns = _slices[1..];

        if (init > ArrayHelpers.Min(interns))
        {
            g = 0;
            return new CodeArray(_slices);
        }
        else
        {
            g = init;
            return new CodeArray(interns);
        }
    }

    [Pure]
    private CodeArray ConvertWhenDegenerate(out int g)
    {
        Debug.Assert(Truncated);
        Debug.Assert(Count == 2);

        (int init, int term) = (InitialSlice, TerminalSlice);

        if (init < term)
        {
            g = init;
            return new CodeArray(term);
        }
        else
        {
            g = 0;
            return init == term ? new CodeArray(_slices) : new CodeArray(init);
        }
    }

    [Pure]
    private CodeArray ConvertWhenTruncated(out int g)
    {
        Debug.Assert(Truncated);
        Debug.Assert(Count > 2);

        int[] interns = _slices[1..^1];
        int min = ArrayHelpers.Min(interns);

        (int init, int term) = (InitialSlice, TerminalSlice);

        if (init > min)
        {
            g = 0;
            return new CodeArray(term > min ? _slices : _slices[0..^1]);
        }
        else
        {
            g = init;
            return new CodeArray(term > min ? _slices[1..] : interns);
        }
    }
}

public partial class SliceArray // IEquatable
{
    /// <inheritdoc />
    [Pure]
    public bool Equals(SliceArray? other) =>
        other is not null
        && Complete == other.Complete
        && Enumerable.SequenceEqual(_slices, other._slices);

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is SliceArray a && Equals(a);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _slices.GetHashCode();
}
