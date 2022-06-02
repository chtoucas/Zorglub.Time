
Facts:
- A facts class MUST be abstract.
- Its methods MUST have distinct names (parameters do not count).
- Its methods MUST NOT be static.
- A facts class providing DDT SHOULD derive from either CalendricalDataConsumer 
  or CalendarDataConsumer, even when it's not necessary. This is mostly important
  when testing calendars and related date types which have a bounded scopes.
  Moreover, this way, all facts classes share the same initialization process.
- Be careful when testing static methods: for instance TDate.FromDayNumber()
  only works with the _default_ calendar, which means that it should not be part
  of the facts class.

When overriding a test method, we can omit the attribute `Fact`.
More interestingly, we could define a virtual empty test method without the 
attribute `Fact` in a base class and only override it when there is actually
something to test.
```
public class Base {
    public virtual void TestMethod() { }
}
public class Derived : Base {
    [Fact]
    public override void TestMethod() { ... }
}
```

Arithmetic and Math
-------------------

TODO(fact): a bit messy... move most tests to `CalendarMath...Facts`.
Therefore we should use `CalendarMath...Facts` for all types of calendars, not just the 
Gregorian one.

We have two types of tests: DDT and limits (overflow, min/max values).

### Schemas
- `ICalendricalArithmeticFacts`
  - DDT for `ICalendricalArithmetic`.
- `CalendricalArithmeticFacts`
  - Limits for `ICalendricalArithmetic`.
  - DDT for `ICalendricalArithmetic` (inherited from `ICalendricalArithmeticFacts`)
    
### Calendars
- `CalendarMathFacts`
  - Limits and DDT when the non-standard methods are _unambiguous_.
  - DDT for the non-standard methods of `OrdinalDate`.
  - DDT for the non-standard methods of `CalendarDate`.
  - Limits and DDT for all methods/ops of `CalendarMonth`.
- `CalendarMathAdvancedFacts`
  - Limits and DDT when the non-standard methods are _ambiguous_
  - Limits and DDT for all methods of `CalendarMonth`.

The two classes use different datasets.

### Date Types
- `IDateFacts`
  - Limits for the standard methods/ops on dates.
- `IDateArithmeticFacts`
  - DDT for the standard methods/ops on dates.
- `IDateOrdinalArithmeticFacts`
  - DDT for the standard methods/ops on ordinal dates.

We distinguish ordinary dates from ordinal dates and we don't include the tests
in `IDateFacts` because, for ordinal dates, it would require a conversion. In
particular `IDateOrdinalArithmeticFacts` uses a dataset specialised to
ordinal dates.

Simple date types.
- `CalendarDateFacts`
  - Limits for the standard methods/ops (inherited from `IDateFacts`).
- `OrdinalDateFacts`
  - Limits for the standard methods/ops (inherited from `IDateFacts`).
- `CalendarYearFacts`
  - Limits and DDT for the standard methods/ops.
