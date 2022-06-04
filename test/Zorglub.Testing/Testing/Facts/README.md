
We call _test bundle_ a class of facts.

Test bundles:
- A test bundle MUST be abstract.
- Its methods MUST have distinct names (parameters do not count).
- Its methods MUST NOT be static.
- A test bundle providing DDT SHOULD derive from either CalendricalDataConsumer 
  or CalendarDataConsumer, even when it's not necessary. This is mostly important
  when testing calendars and related date types which have a bounded scopes.
  Moreover, this way, all test bundles share the same initialization process.
- Be careful when testing static methods: for instance TDate.FromDayNumber()
  only works with the _default_ calendar, which means that it should not be part
  of a test bundle.

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
