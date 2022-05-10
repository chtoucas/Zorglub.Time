// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Core;

using Zorglub.Time.Core.Utilities;

using static Zorglub.Time.Extensions.BoxExtensions;

// We keep these tests to test the QEP.

public static partial class BoxTests
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes (Performance) 👈 Tests
    private sealed class AnyT { }
    private sealed class AnyResult { }
#pragma warning restore CA1812

    private static class MyUri
    {
        public static readonly Uri Value = new("http://www.narvalo.org");
        public static readonly Box<Uri> Empty = Box<Uri>.Empty;
        public static Box<Uri> Some => Box.Create(Value);
    }

    /// <summary>
    /// Represents a singleton reference type.
    /// </summary>
    private sealed class MyResult
    {
        public static Box<MyResult> BinderToEmpty<T>(T _) => Box<MyResult>.Empty;
        public static Box<MyResult> BinderToValue<T>(T _) => Some;

        private MyResult() { }

        public static MyResult Value => Instance.Value;

        public static Box<MyResult> Some { get; } = Box.Create(Instance.Value);

        private static class Instance
        {
            public static readonly MyResult Value = new();
        }
    }

    private static class Funk<T, TResult>
        where T : class
        where TResult : notnull
    {
        public static readonly Func<T, TResult> Null = null!;
        public static readonly Func<T, TResult> Any = _ => throw new InvalidOperationException();
    }

    // Functions in the Kleisli category.
    private static class Kunc<T, TResult>
        where T : class
        where TResult : class
    {
        public static readonly Func<T, Box<TResult>> Null = null!;
        public static readonly Func<T, Box<TResult>> Any = _ => throw new InvalidOperationException();
    }

    private static class Funk<T1, T2, TResult>
        where T1 : class
        where T2 : class
        where TResult : class
    {
        public static readonly Func<T1, T2, TResult> Null = null!;
        public static readonly Func<T1, T2, TResult> Any = (x, y) => throw new InvalidOperationException();
    }
}

public partial class BoxTests
{
    [Fact]
    public static void Create_NullValue() =>
        Assert.Empty(Box.Create((Uri?)null));

    [Fact]
    public static void SelectAllowsUnboxing()
    {
        // Arrange
        var obj = new object();
        object? result = null;
        // Act
        _ = from x in Box.Create(obj) select Unbox(x);
        // Assert
        Assert.Same(obj, result);

        string Unbox(object input)
        {
            result = input;
            return "whatever";
        }
    }
}

public partial class BoxTests // Monad + QEP
{
    #region Bind()

    [Fact]
    public static void Bind_NullBinder_WhenEmpty() =>
        Assert.ThrowsAnexn("binder", () => MyUri.Empty.Bind(Kunc<Uri, MyResult>.Null));

    [Fact]
    public static void Bind_NullBinder_WhenSome() =>
        Assert.ThrowsAnexn("binder", () => MyUri.Some.Bind(Kunc<Uri, MyResult>.Null));

    [Fact]
    public static void Bind_BinderReturningEmpty_WhenEmpty() =>
        Assert.Empty(MyUri.Empty.Bind(MyResult.BinderToEmpty));

    [Fact]
    public static void Bind_BinderReturningEmpty_WhenSome() =>
        Assert.Empty(MyUri.Some.Bind(MyResult.BinderToEmpty));

    [Fact]
    public static void Bind_BinderReturningSome_WhenEmpty() =>
        Assert.Empty(MyUri.Empty.Bind(MyResult.BinderToValue));

    [Fact]
    public static void Bind_BinderReturningSome_WhenSome() =>
        Assert.Some(MyResult.Value, MyUri.Some.Bind(MyResult.BinderToValue));

    [Fact]
    public static void Bind_WhenSome() =>
        Assert.Some(MyUri.Value.AbsoluteUri, MyUri.Some.Bind(x => Box.Create(x.AbsoluteUri)));

    #endregion
    #region Select()

    [Fact]
    public static void Select_SelectorReturningNull_WhenEmpty() =>
        Assert.Empty(from x in MyUri.Empty select (MyResult?)null);

    [Fact]
    public static void Select_SelectorReturningNull_WhenSome() =>
        Assert.Empty(from x in MyUri.Some select (MyResult?)null);

    [Fact]
    public static void Select_WhenSome() =>
        Assert.Some(MyUri.Value.AbsoluteUri, from x in MyUri.Some select x.AbsoluteUri);

    [Fact]
    public static void Select_WhenSome_WhenTrySelectorSucceeds()
    {
        Assert.Some(MyUri.Value.AbsoluteUri, MyUri.Some.Select(TrySelector));

        static string? TrySelector(Uri uri) => uri.AbsoluteUri;
    }

    [Fact]
    public static void Select_WhenSome_WhenTrySelectorFails()
    {
        Assert.Empty(MyUri.Some.Select(TrySelector));

        static string? TrySelector(Uri uri) => null;
    }

    #endregion
    #region Where()

    [Fact]
    public static void Where_NullPredicate_WhenEmpty() =>
        Assert.ThrowsAnexn("predicate", () => MyUri.Empty.Where(null!));

    [Fact]
    public static void Where_NullPredicate_WhenSome() =>
        Assert.ThrowsAnexn("predicate", () => MyUri.Some.Where(null!));

    [Fact]
    public static void Where_WhenEmpty()
    {
        Assert.Empty(MyUri.Empty.Where(Funk<Uri, bool>.Any));

        Assert.Empty(from x in MyUri.Empty where true select x);
        Assert.Empty(from x in MyUri.Empty where false select x);
    }

    [Fact]
    public static void Where_WhenSome()
    {
        // Some.Where(false) -> Empty
        Assert.Empty(Box.Create("xxx").Where(x => x == "yyy"));
        Assert.Empty(from x in Box.Create("xxx") where x == "yyy" select x);

        // Some.Where(true) -> Some
        Assert.Some("xxx", Box.Create("xxx").Where(x => x == "xxx"));
        Assert.Some("xxx", from x in Box.Create("xxx") where x == "xxx" select x);
    }

    #endregion
    #region SelectMany()

    [Fact]
    public static void SelectMany_NullSelector_WhenEmpty() =>
        Assert.ThrowsAnexn("binder", () => MyUri.Empty.SelectMany(Kunc<Uri, AnyT>.Null, Funk<Uri, AnyT, AnyResult>.Any));

    [Fact]
    public static void SelectMany_NullSelector_WhenSome() =>
        Assert.ThrowsAnexn("binder", () => MyUri.Some.SelectMany(Kunc<Uri, AnyT>.Null, Funk<Uri, AnyT, AnyResult>.Any));

    [Fact]
    public static void SelectMany_NullResultSelector_WhenEmpty() =>
        Assert.ThrowsAnexn("zipper", () => MyUri.Empty.SelectMany(Kunc<Uri, AnyT>.Any, Funk<Uri, AnyT, AnyResult>.Null));

    [Fact]
    public static void SelectMany_NullResultSelector_WhenSome() =>
        Assert.ThrowsAnexn("zipper", () => MyUri.Some.SelectMany(Kunc<Uri, AnyT>.Any, Funk<Uri, AnyT, AnyResult>.Null));

    [Fact]
    public static void SelectMany_SelectorReturningEmpty_WhenEmpty()
    {
        Assert.Empty(Box<string>.Empty.SelectMany(x => Box<string>.Empty, (x, y) => x + y));
        Assert.Empty(from x in Box<string>.Empty from y in Box<string>.Empty select x + y);
    }

    [Fact]
    public static void SelectMany_SelectorReturningEmpty_WhenSome()
    {
        Assert.Empty(Box.Create("xxx").SelectMany(v => Box<string>.Empty, (x, y) => x + y));
        Assert.Empty(from x in Box.Create("xxx") from y in Box<string>.Empty select x + y);
    }

    [Fact]
    public static void SelectMany_SelectorReturningSome_WhenEmpty()
    {
        Assert.Empty(Box<string>.Empty.SelectMany(v => Box.Create(v + "yyy"), (x, y) => x + y));
        Assert.Empty(from x in Box<string>.Empty from y in Box.Create(x + "yyy") select x + y);
    }

    [Fact]
    public static void SelectMany_SelectorReturningSome_WhenSome()
    {
        Assert.Some("xxxxxxyyy", Box.Create("xxx").SelectMany(v => Box.Create(v + "yyy"), (x, y) => x + y));
        Assert.Some("xxxxxxyyy", from x in Box.Create("xxx") from y in Box.Create(x + "yyy") select x + y);
    }

    #endregion
}
