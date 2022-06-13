// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core.Utilities
{
    using Zorglub.Time.Simple;

    // TODO(code): do not use plain exn. Add localized messages (french)?

    #region Developer Notes

    // The main problem with ThrowHelper is that the code coverage can't see if
    // the tests covered the two branches (with and without exception thrown).
    //
    // Even if it always throws, a method returning something should be
    // decorated with the attribute Pure. This way, we get a warning if, for
    // instance, we write
    // > ThrowHelpers.ArgumentNull<int>("paramName");
    // when we should have written
    // > ThrowHelpers.ArgumentNull(paramName");
    // This does not apply to the exception factory methods, as they are always
    // preceded by a "throw" (one can not throw void...).
    //
    // About the attribute "DoesNotReturn".
    //   "Do not inline methods that never return"
    //   https://github.com/dotnet/coreclr/pull/6103
    //
    // Extracts from the BCL
    // ---------------------
    //
    // https://source.dot.net/#System.Memory/System/ThrowHelper.cs
    // This pattern of easily inlinable "void Throw" routines that stack on top of NoInlining factory methods
    // is a compromise between older JITs and newer JITs (RyuJIT in .NET Core 1.1.0+ and .NET Framework in 4.6.3+).
    // This package is explicitly targeted at older JITs as newer runtimes expect to implement Span intrinsically for
    // best performance.
    //
    // The aim of this pattern is three-fold
    // 1. Extracting the throw makes the method preforming the throw in a conditional branch smaller and more inlinable
    // 2. Extracting the throw from generic method to non-generic method reduces the repeated codegen size for value types
    // 3a. Newer JITs will not inline the methods that only throw and also recognise them, move the call to cold section
    //     and not add stack prep and unwind before calling https://github.com/dotnet/coreclr/pull/6103
    // 3b. Older JITs will inline the throw itself and move to cold section; but not inline the non-inlinable exception
    //     factory methods - still maintaining advantages 1 & 2
    //// This file defines an internal static class used to throw exceptions in BCL code.
    // The main purpose is to reduce code size.
    //
    // https://source.dot.net/#System.Private.CoreLib/ThrowHelper.cs
    // The old way to throw an exception generates quite a lot IL code and assembly code.
    // Following is an example:
    //     C# source
    //          throw new ArgumentNullException(nameof(key), SR.ArgumentNull_Key);
    //     IL code:
    //          IL_0003:  ldstr      "key"
    //          IL_0008:  ldstr      "ArgumentNull_Key"
    //          IL_000d:  call       string System.Environment::GetResourceString(string)
    //          IL_0012:  newobj     instance void System.ArgumentNullException::.ctor(string,string)
    //          IL_0017:  throw
    //    which is 21bytes in IL.
    //
    // So we want to get rid of the ldstr and call to Environment.GetResource in IL.
    // In order to do that, I created two enums: ExceptionResource, ExceptionArgument to represent the
    // argument name and resource name in a small integer. The source code will be changed to
    //    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key, ExceptionResource.ArgumentNull_Key);
    //
    // The IL code will be 7 bytes.
    //    IL_0008:  ldc.i4.4
    //    IL_0009:  ldc.i4.4
    //    IL_000a:  call       void System.ThrowHelper::ThrowArgumentNullException(valuetype System.ExceptionArgument)
    //    IL_000f:  ldarg.0
    //
    // This will also reduce the Jitted code size a lot.
    //
    // Microsoft.Toolkit.Diagnostics
    // -----------------------------
    //
    // https://docs.microsoft.com/en-us/windows/communitytoolkit/developer-tools/throwhelper
    // https://github.com/CommunityToolkit/dotnet/blob/main/CommunityToolkit.Diagnostics/ThrowHelper.cs
    // https://github.com/CommunityToolkit/dotnet/blob/main/CommunityToolkit.Diagnostics/ThrowHelper.Generic.cs

    #endregion

    /// <summary>
    /// Provides static helpers to throw exceptions.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    [StackTraceHidden]
    internal static partial class ThrowHelpers { }

    internal partial class ThrowHelpers // Plain
    {
        /// <exception cref="ArgumentException"/>
        [DoesNotReturn]
        public static void Argument(string paramName) => throw GetArgumentExn(paramName);

        /// <exception cref="ArgumentException"/>
        [DoesNotReturn, Pure]
        public static T Argument<T>(string paramName) => throw GetArgumentExn(paramName);

        /// <exception cref="ArgumentNullException"/>
        [DoesNotReturn]
        public static void ArgumentNull(string paramName) => throw GetArgumentNullExn(paramName);

        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void ArgumentOutOfRange(string paramName) => throw GetArgumentOutOfRangeExn(paramName);

        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn, Pure]
        public static T ArgumentOutOfRange<T>(string paramName) => throw GetArgumentOutOfRangeExn(paramName);

        /// <exception cref="InvalidOperationException"/>
        [DoesNotReturn]
        public static void InvalidOperation() => throw GetInvalidOperationExn();

        /// <exception cref="InvalidOperationException"/>
        [DoesNotReturn, Pure]
        public static T InvalidOperation<T>() => throw GetInvalidOperationExn();

        /// <exception cref="NotSupportedException"/>
        [DoesNotReturn, Pure]
        public static T NotSupported<T>() => throw GetNotSupportedExn();

        #region Factories

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetArgumentExn(string paramName) => new(null, paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentNullException GetArgumentNullExn(string paramName) => new(paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetArgumentOutOfRangeExn(string paramName) => new(paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidOperationException GetInvalidOperationExn() => new();

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static NotSupportedException GetNotSupportedExn() => new();

        #endregion
    }

    internal partial class ThrowHelpers
    {
        /// <summary>The box is empty.</summary>
        /// <exception cref="InvalidOperationException"/>
        [DoesNotReturn, Pure]
        public static T EmptyBox<T>() => throw GetEmptyBoxExn();

        /// <summary>The control flow path reached a section of the code that should have been
        /// unreachable under any circumstances.</summary>
        /// <exception cref="InvalidOperationException"/>
        [DoesNotReturn, Pure]
        public static T Unreachable<T>() => throw GetUnreachableExn();

        /// <summary>The collection is read-only.</summary>
        /// <exception cref="NotSupportedException"/>
        [DoesNotReturn]
        public static void ReadOnlyCollection() => throw GetReadOnlyCollectionExn();

        /// <summary>The collection is read-only.</summary>
        /// <exception cref="NotSupportedException"/>
        [DoesNotReturn, Pure]
        public static T ReadOnlyCollection<T>() => throw GetReadOnlyCollectionExn();

        /// <summary>An item with the specified key could not be found.</summary>
        /// <exception cref="KeyNotFoundException"/>
        [DoesNotReturn, Pure]
        public static T KeyNotFound<T>(string key) => throw GetKeyNotFoundExn(key);

        #region Factories

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidOperationException GetEmptyBoxExn() =>
            new("The box is empty.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidOperationException GetUnreachableExn() =>
            new("The control flow path reached a section of the code that should have been unreachable under any circumstances.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static NotSupportedException GetReadOnlyCollectionExn() =>
            new("The collection is read-only.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static KeyNotFoundException GetKeyNotFoundExn(string key) =>
            new($"An item with the key \"{key}\" could not be found.");

        #endregion
    }

    internal partial class ThrowHelpers // ArgumentOutOfRangeException
    {
        /// <summary>The value of the year was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void YearOutOfRange(long year) => throw GetYearOutOfRangeExn(null, year);

        /// <summary>The value of the year was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void YearOutOfRange(int year) => throw GetYearOutOfRangeExn(null, year);

        /// <summary>The value of the year was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void YearOutOfRange(int year, string? paramName) => throw GetYearOutOfRangeExn(paramName, year);

        /// <summary>The value of the month of the year was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void MonthOutOfRange(int month) => throw GetMonthOutOfRangeExn(null, month);

        /// <summary>The value of the month of the year was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void MonthOutOfRange(int month, string? paramName) =>
            throw GetMonthOutOfRangeExn(paramName, month);

        /// <summary>The value of the day of the month was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void DayOutOfRange(int day) => throw GetDayOutOfRangeExn(null, day);

        /// <summary>The value of the day of the month was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void DayOutOfRange(int day, string? paramName) => throw GetDayOutOfRangeExn(paramName, day);

        /// <summary>The value of the day of the year was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void DayOfYearOutOfRange(int dayOfYear) => throw GetDayOfYearOutOfRangeExn(null, dayOfYear);

        /// <summary>The value of the day of the year was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void DayOfYearOutOfRange(int dayOfYear, string? paramName) =>
            throw GetDayOfYearOutOfRangeExn(paramName, dayOfYear);

        /// <summary>The value of the day of the week was out of range.</summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        public static void DayOfWeekOutOfRange(DayOfWeek dayOfWeek, string? paramName) =>
            throw GetDayOfWeekOutOfRangeExn(paramName, dayOfWeek);

        #region Factories

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetYearOutOfRangeExn(string? paramName, long year) =>
             new(paramName ?? nameof(year),
                 year,
                 $"The value of the year was out of range; value = {year}.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetYearOutOfRangeExn(string? paramName, int year) =>
             new(paramName ?? nameof(year),
                 year,
                 $"The value of the year was out of range; value = {year}.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetMonthOutOfRangeExn(string? paramName, int month) =>
             new(paramName ?? nameof(month),
                 month,
                 $"The value of the month of the year was out of range; value = {month}.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetDayOutOfRangeExn(string? paramName, int day) =>
             new(paramName ?? nameof(day),
                 day,
                 $"The value of the day of the month was out of range; value = {day}.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetDayOfYearOutOfRangeExn(string? paramName, int dayOfYear) =>
             new(paramName ?? nameof(dayOfYear),
                 dayOfYear,
                 $"The value of the day of the year was out of range; value = {dayOfYear}.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetDayOfWeekOutOfRangeExn(string? paramName, DayOfWeek dayOfWeek) =>
             new(paramName ?? nameof(dayOfWeek),
                 dayOfWeek,
                 $"The value of the day of the week must be in the range 0 through 6; value = {dayOfWeek}.");

        #endregion
    }

    internal partial class ThrowHelpers // ArgumentException
    {
        /// <summary>The binary data is not well-formed.</summary>
        /// <exception cref="ArgumentException"/>
        [DoesNotReturn]
        public static void BadBinaryInput() => throw GetBadBinaryInputExn();

        /// <exception cref="ArgumentException"/>
        [DoesNotReturn]
        public static int NonComparable(Type expected, object obj) => throw GetNonComparableExn(expected, obj);

        /// <summary>The box is empty.</summary>
        /// <exception cref="ArgumentException"/>
        [DoesNotReturn, Pure]
        public static T BadBox<T>(string paramName) => throw GetBadBoxExn(paramName);

        /// <exception cref="ArgumentException"/>
        [DoesNotReturn]
        public static void BadSchemaProfile(string paramName, CalendricalProfile expected, CalendricalProfile actual) =>
            throw GetBadSchemaProfileExn(paramName, expected, actual);

        /// <exception cref="ArgumentException"/>
        [DoesNotReturn]
        public static void BadCuid(string paramName, Cuid expected, Cuid actual) =>
            throw GetBadCuidExn(paramName, expected, actual);

        /// <exception cref="ArgumentException"/>
        [DoesNotReturn]
        public static void BadCuid(string paramName, int expected, int actual) =>
            throw GetBadCuidExn(paramName, expected, actual);

        /// <summary>An item with the specified key already exists.</summary>
        /// <exception cref="ArgumentException"/>
        [DoesNotReturn]
        public static void KeyAlreadyExists(string paramName, string key) =>
            throw GetKeyAlreadyExistsExn(paramName, key);

        #region Factories

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetBadBinaryInputExn() =>
            new("The binary data is not well-formed.", "data");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetNonComparableExn(Type expected, object obj) =>
            new($"The object should be of type {expected} but it is of type {obj.GetType()}.",
                nameof(obj));

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetBadBoxExn(string paramName) =>
            new("The box is empty.", paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetBadSchemaProfileExn(string paramName, CalendricalProfile expected, CalendricalProfile actual) =>
            new($"The schema profile should be equal to \"{expected}\" but it is equal to \"{actual}\".",
                paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetBadCuidExn(string paramName, Cuid expected, Cuid actual) =>
            new($"The calendar ID should be equal to \"{expected}\" but it is equal to \"{actual}\".",
                paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetBadCuidExn(string paramName, int expected, int actual) =>
            new($"The calendar ID should be equal to \"{expected}\" but it is equal to \"{actual}\".",
                paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetKeyAlreadyExistsExn(string paramName, string key) =>
            new($"An item with the key \"{key}\" already exists.",
                paramName);

        #endregion
    }

    internal partial class ThrowHelpers // OverflowException
    {
        /// <summary>The operation would overflow the range of supported months.</summary>
        /// <exception cref="OverflowException"/>
        [DoesNotReturn]
        public static void MonthOverflow() => throw GetMonthOverflowExn();

        /// <summary>The operation would overflow the range of supported dates.</summary>
        /// <exception cref="OverflowException"/>
        [DoesNotReturn]
        public static void DateOverflow() => throw GetDateOverflowExn();

        /// <summary>The operation would overflow the range of supported dates.</summary>
        /// <exception cref="OverflowException"/>
        [DoesNotReturn, Pure]
        public static T DateOverflow<T>() => throw GetDateOverflowExn();

        /// <summary>The operation would overflow the range of supported day numbers.</summary>
        /// <exception cref="OverflowException"/>
        [DoesNotReturn]
        public static void DayNumberOverflow() => throw GetDayNumberOverflowExn();

        /// <summary>The operation would overflow the range of supported day numbers.</summary>
        /// <exception cref="OverflowException"/>
        [DoesNotReturn, Pure]
        public static T DayNumberOverflow<T>() => throw GetDayNumberOverflowExn();

        /// <summary>The operation would overflow the range of supported ordinal numerals.</summary>
        /// <exception cref="OverflowException"/>
        [DoesNotReturn, Pure]
        public static T OrdOverflow<T>() => throw GetOrdOverflowExn();

        /// <summary>The operation would overflow the maximun number of calendars supported by the
        /// system.</summary>
        /// <exception cref="OverflowException"/>
        [DoesNotReturn, Pure]
        public static void CatalogOverflow() => throw GetCatalogOverflowExn();

        #region Factories

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static OverflowException GetMonthOverflowExn() =>
            new("The computation would overflow the range of supported months.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static OverflowException GetDateOverflowExn() =>
            new("The computation would overflow the range of supported dates.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static OverflowException GetDayNumberOverflowExn() =>
            new("The computation would overflow the range of supported day numbers.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static OverflowException GetOrdOverflowExn() =>
            new("The computation would overflow the range of supported ordinal numerals.");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static OverflowException GetCatalogOverflowExn() =>
            new("The operation would overflow the maximun number of calendars supported by the system.");

        #endregion
    }
}