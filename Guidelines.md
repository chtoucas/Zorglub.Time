Coding Guidelines
=================

https://nietras.com/2021/06/14/csharp-10-record-struct/

TODOs
-----

- FIXME
- TODO
- HACK
- REVIEW

Design
------

Our default is to seal the classes.
Add the attr `[Pure]` to all methods returning something.
Add the attr `[DoesNotReturn]` for methods that always throw.
Static field vs static prop: for structs I prefer static readonly field.
In a -public- sealed class use "override sealed" instead of "override" (this way
we can spot overridable methods in the file PublicAPI).

https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/framework-design-guidelines-digest.md

Naming
------

Static factory methods for a type XXX:
- `Create()`
- `From...()` conversion
- `At...()`
Adjustment of a field of a value type:
- `With...()`
Bool prop: prefix `Is`, sometines `Has`.

Static class: if it ressembles an enum or a discriminated union, e.g. Dayzero or
CalendarEpochs, use the singular; otherwise use the plural.

Preview Features
----------------

We define two compiler constants:
- `FEATURE_STATIC_ABSTRACT` to enable or disable section of codes depending
  on the "static abstract" feature.

We don't use the attr 'RequiresPreviewFeatures'.

Style
-----

Rather than
> if (!Method(...)) { ... }
write
> if (Method(...) == false) { ... }

https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md

XML doc
-------

Virtual methods (interfaces, abstract classes, etc.): no section <remarks>

https://github.com/dotnet/dotnet-api-docs/wiki

Structs
-------

I prefer fields to properties, this way we can see clearly its size.

Check-list
- readonly?
- IEquatable
- IComparable?
- ToString()
- Deconstruct()?

Nullable Reference Objects
--------------------------

Two possibilities:
```
> public bool TryGetValue([NotNullWhen(true)] out T? value)
> public bool TryGetValue([MaybeNullWhen(false)] out T value)
```
Both are equivalent but they are semantically different:
- NotNullWhen(bool) signifies that a parameter is not null even if
the type allows it.
- MaybeNullWhen(bool) signifies that a parameter could be null even
if the type disallows it.
In general,
- for reference types, I prefer to use NotNullWhen
- for value types, I prefer to use MaybeNullWhen
- for generic types with the notnull constraint, to me it makes more
sense to use MaybeNullWhen.Indeed, from a caller point of view, we
cannot write (error CS1503)
```
    box.TryGetValue(out int? value);
```
Caller perspective:
```
> box.TryGetValue(out int val);
> box.TryGetValue(out object? val);
> box.TryGetValue(out T? val);
```
and if we ignore the output param(don't know why but anyway):
```
> box.TryGetValue(out int _);
> box.TryGetValue(out object _);
> box.TryGetValue(out T _);
```

Performance
-----------

https://github.com/dotnet/docs/blob/main/docs/csharp/write-safe-efficient-code.md

### MethodImplOptions

MethodImplOptions.AggressiveInlining? YES
MethodImplOptions.AggressiveOptimization? NO

https://github.com/dotnet/runtime/issues/27370
https://stackoverflow.com/questions/61601225/net-core-what-does-methodimploptions-aggressiveoptimization-exactly-do

### Math ops

Use bit shifting instead of multiplication or division by a power of 2.
Shifting is always faster and the "division" is euclidian whether the dividend
is positive or negative which is not the case with the C# division operator.

Prefer unsigned division by a **constant** to signed division. This is really
not something that we want to do everywhere as it makes the code much less
readable.

Recurring Tasks
---------------

- Check #pragma and SuppressMessage

Tests
-----


Unicode Symbols
---------------

﹍
☐ (ballot box)
👈 👉
⋂ 	⋃
⊂ 	⊃ 	⊆ 	⊇
