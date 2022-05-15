
Facts:
- A fact class MUST be abstract.
- Its methods MUST have distinct names (parameters do not count).
- Its methods MUST NOT be static.
- We SHOULD NOT test static methods.

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
