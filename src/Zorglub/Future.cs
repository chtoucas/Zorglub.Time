#pragma warning disable IDE0073 // Require file header (Style)

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#region License
// The MIT License (MIT)
//
// Copyright (c) .NET Foundation and Contributors
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

// Tests fail to run if we enable "static virtual".
// Might have worked if we used .NET 7 Preview.
//#define STATIC_VIRTUAL

#pragma warning disable CA1000 // Do not declare static members on generic types (Design) 👈 PreviewFeatures
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) 👈 PreviewFeatures
#pragma warning disable IDE0130 // Namespace does not match folder structure 👈 PreviewFeatures

#if !NET7_0_OR_GREATER

namespace System.Numerics // Generic Math
{
    /// <summary>Defines a mechanism for comparing two values to determine equality.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TOther">The type that will be compared with <typeparamref name="TSelf" />.</typeparam>
    /// <typeparam name="TResult">The type that is returned as a result of the comparison.</typeparam>
    public interface IEqualityOperators<TSelf, TOther, TResult>
        where TSelf : IEqualityOperators<TSelf, TOther, TResult>?
    {
        /// <summary>Compares two values to determine equality.</summary>
        /// <param name="left">The value to compare with <paramref name="right" />.</param>
        /// <param name="right">The value to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        static abstract TResult operator ==(TSelf? left, TOther? right);

        /// <summary>Compares two values to determine inequality.</summary>
        /// <param name="left">The value to compare with <paramref name="right" />.</param>
        /// <param name="right">The value to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        static abstract TResult operator !=(TSelf? left, TOther? right);
    }

    /// <summary>Defines a mechanism for comparing two values to determine relative order.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TOther">The type that will be compared with <typeparamref name="TSelf" />.</typeparam>
    /// <typeparam name="TResult">The type that is returned as a result of the comparison.</typeparam>
    public interface IComparisonOperators<TSelf, TOther, TResult>
        : IEqualityOperators<TSelf, TOther, TResult>
        where TSelf : IComparisonOperators<TSelf, TOther, TResult>?
    {
        /// <summary>Compares two values to determine which is less.</summary>
        /// <param name="left">The value to compare with <paramref name="right" />.</param>
        /// <param name="right">The value to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        static abstract TResult operator <(TSelf left, TOther right);

        /// <summary>Compares two values to determine which is less or equal.</summary>
        /// <param name="left">The value to compare with <paramref name="right" />.</param>
        /// <param name="right">The value to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        static abstract TResult operator <=(TSelf left, TOther right);

        /// <summary>Compares two values to determine which is greater.</summary>
        /// <param name="left">The value to compare with <paramref name="right" />.</param>
        /// <param name="right">The value to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        static abstract TResult operator >(TSelf left, TOther right);

        /// <summary>Compares two values to determine which is greater or equal.</summary>
        /// <param name="left">The value to compare with <paramref name="right" />.</param>
        /// <param name="right">The value to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        static abstract TResult operator >=(TSelf left, TOther right);
    }

    /// <summary>Defines a mechanism for getting the minimum and maximum value of a type.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IMinMaxValue<TSelf>
        where TSelf : IMinMaxValue<TSelf>?
    {
        /// <summary>Gets the minimum value of the current type.</summary>
        static abstract TSelf MinValue { get; }

        /// <summary>Gets the maximum value of the current type.</summary>
        static abstract TSelf MaxValue { get; }
    }

    /// <summary>Defines a mechanism for computing the sum of two values.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TOther">The type that will be added to <typeparamref name="TSelf" />.</typeparam>
    /// <typeparam name="TResult">The type that contains the sum of <typeparamref name="TSelf" /> and <typeparamref name="TOther" />.</typeparam>
    public interface IAdditionOperators<TSelf, TOther, TResult>
        where TSelf : IAdditionOperators<TSelf, TOther, TResult>?
    {
        /// <summary>Adds two values together to compute their sum.</summary>
        /// <param name="left">The value to which <paramref name="right" /> is added.</param>
        /// <param name="right">The value which is added to <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        static abstract TResult operator +(TSelf left, TOther right);

#if STATIC_VIRTUAL
        /// <summary>Adds two values together to compute their sum.</summary>
        /// <param name="left">The value to which <paramref name="right" /> is added.</param>
        /// <param name="right">The value which is added to <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <exception cref="OverflowException">The sum of <paramref name="left" /> and <paramref name="right" /> is not representable by <typeparamref name="TResult" />.</exception>
        static virtual TResult operator checked +(TSelf left, TOther right) => left + right;
#endif
    }

    /// <summary>Defines a mechanism for computing the difference of two values.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TOther">The type that will be subtracted from <typeparamref name="TSelf" />.</typeparam>
    /// <typeparam name="TResult">The type that contains the difference of <typeparamref name="TOther" /> subtracted from <typeparamref name="TSelf" />.</typeparam>
    public interface ISubtractionOperators<TSelf, TOther, TResult>
        where TSelf : ISubtractionOperators<TSelf, TOther, TResult>?
    {
        /// <summary>Subtracts two values to compute their difference.</summary>
        /// <param name="left">The value from which <paramref name="right" /> is subtracted.</param>
        /// <param name="right">The value which is subtracted from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        static abstract TResult operator -(TSelf left, TOther right);

#if STATIC_VIRTUAL
        /// <summary>Subtracts two values to compute their difference.</summary>
        /// <param name="left">The value from which <paramref name="right" /> is subtracted.</param>
        /// <param name="right">The value which is subtracted from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        /// <exception cref="OverflowException">The difference of <paramref name="right" /> subtracted from <paramref name="left" /> is not representable by <typeparamref name="TResult" />.</exception>
        static virtual TResult operator checked -(TSelf left, TOther right) => left - right;
#endif
    }

    /// <summary>Defines a mechanism for incrementing a given value.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IIncrementOperators<TSelf>
        where TSelf : IIncrementOperators<TSelf>?
    {
        /// <summary>Increments a value.</summary>
        /// <param name="value">The value to increment.</param>
        /// <returns>The result of incrementing <paramref name="value" />.</returns>
        static abstract TSelf operator ++(TSelf value);

#if STATIC_VIRTUAL
        /// <summary>Increments a value.</summary>
        /// <param name="value">The value to increment.</param>
        /// <returns>The result of incrementing <paramref name="value" />.</returns>
        /// <exception cref="OverflowException">The result of incrementing <paramref name="value" /> is not representable by <typeparamref name="TSelf" />.</exception>
        static virtual TSelf operator checked ++(TSelf value) => ++value;
#endif
    }

    /// <summary>Defines a mechanism for decrementing a given value.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IDecrementOperators<TSelf>
        where TSelf : IDecrementOperators<TSelf>?
    {
        /// <summary>Decrements a value.</summary>
        /// <param name="value">The value to decrement.</param>
        /// <returns>The result of decrementing <paramref name="value" />.</returns>
        static abstract TSelf operator --(TSelf value);

#if STATIC_VIRTUAL
        /// <summary>Decrements a value.</summary>
        /// <param name="value">The value to decrement.</param>
        /// <returns>The result of decrementing <paramref name="value" />.</returns>
        /// <exception cref="OverflowException">The result of decrementing <paramref name="value" /> is not representable by <typeparamref name="TSelf" />.</exception>
        static virtual TSelf operator checked --(TSelf value) => --value;
#endif
    }

    /// <summary>Defines a mechanism for computing the unary negation of a value.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TResult">The type that contains the result of negating <typeparamref name="TSelf" />.</typeparam>
    public interface IUnaryNegationOperators<TSelf, TResult>
        where TSelf : IUnaryNegationOperators<TSelf, TResult>?
    {
        /// <summary>Computes the unary negation of a value.</summary>
        /// <param name="value">The value for which to compute its unary negation.</param>
        /// <returns>The unary negation of <paramref name="value" />.</returns>
        static abstract TResult operator -(TSelf value);

#if STATIC_VIRTUAL
        /// <summary>Computes the unary negation of a value.</summary>
        /// <param name="value">The value for which to compute its unary negation.</param>
        /// <returns>The unary negation of <paramref name="value" />.</returns>
        /// <exception cref="OverflowException">The unary negation of <paramref name="value" /> is not representable by <typeparamref name="TResult" />.</exception>
        static virtual TResult operator checked -(TSelf value) => -value;
#endif
    }

    /// <summary>Defines a mechanism for computing the quotient of two values.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TOther">The type that will divide <typeparamref name="TSelf" />.</typeparam>
    /// <typeparam name="TResult">The type that contains the quotient of <typeparamref name="TSelf" /> and <typeparamref name="TOther" />.</typeparam>
    public interface IDivisionOperators<TSelf, TOther, TResult>
        where TSelf : IDivisionOperators<TSelf, TOther, TResult>?
    {
        /// <summary>Divides two values together to compute their quotient.</summary>
        /// <param name="left">The value which <paramref name="right" /> divides.</param>
        /// <param name="right">The value which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided-by <paramref name="right" />.</returns>
        static abstract TResult operator /(TSelf left, TOther right);

#if STATIC_VIRTUAL
        /// <summary>Divides two values together to compute their quotient.</summary>
        /// <param name="left">The value which <paramref name="right" /> divides.</param>
        /// <param name="right">The value which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided-by <paramref name="right" />.</returns>
        /// <exception cref="OverflowException">The quotient of <paramref name="left" /> divided-by <paramref name="right" /> is not representable by <typeparamref name="TResult" />.</exception>
        static virtual TResult operator checked /(TSelf left, TOther right) => left / right;
#endif
    }
}

namespace System.Diagnostics.CodeAnalysis // "required"
{
    /// <summary>
    /// Specifies that this constructor sets all required members for the current type, and callers
    /// do not need to set any required members themselves.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class SetsRequiredMembersAttribute : Attribute { }
}

namespace System.Runtime.CompilerServices // "required"
{
    /// <summary>Specifies that a type has required members or that a member is required.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RequiredMemberAttribute : Attribute { }

    /// <summary>
    /// Indicates that compiler support for a particular feature is required for the location where this attribute is applied.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public sealed class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string featureName)
        {
            FeatureName = featureName;
        }

        /// <summary>
        /// The name of the compiler feature.
        /// </summary>
        public string FeatureName { get; }

        /// <summary>
        /// If true, the compiler can choose to allow access to the location where this attribute is applied if it does not understand <see cref="FeatureName"/>.
        /// </summary>
        public bool IsOptional { get; init; }

        /// <summary>
        /// The <see cref="FeatureName"/> used for the ref structs C# feature.
        /// </summary>
        public const string RefStructs = nameof(RefStructs);

        /// <summary>
        /// The <see cref="FeatureName"/> used for the required members C# feature.
        /// </summary>
        public const string RequiredMembers = nameof(RequiredMembers);
    }
}

#endif

namespace Zorglub.Time.Core.Utilities
{
    using System.Numerics;

    /// <summary>Defines a mechanism for comparing two values to determine equality.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TOther">The type that will be compared with <typeparamref name="TSelf" />.</typeparam>
    public interface IEqualityOperators<TSelf, TOther> :
        IEqualityOperators<TSelf, TOther, bool>,
        IEquatable<TOther>
        where TSelf : IEqualityOperators<TSelf, TOther>?
    { }

    /// <summary>Defines a mechanism for comparing two values to determine relative order.</summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    /// <typeparam name="TOther">The type that will be compared with <typeparamref name="TSelf" />.</typeparam>
    public interface IComparisonOperators<TSelf, TOther> :
        IComparisonOperators<TSelf, TOther, bool>,
        IEqualityOperators<TSelf, TOther>,
        IComparable<TOther>,
        IComparable
        where TSelf : IComparisonOperators<TSelf, TOther>?
    { }
}
