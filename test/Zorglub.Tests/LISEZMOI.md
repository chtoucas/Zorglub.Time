
https://bartoszsypytkowski.com/writing-high-performance-f-code/

https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/operator-overloading#creating-new-operators
!, $, %, &, *, +, -, ., /, <, =, >, ?, @, ^, |

À classer
---------

Checked ops
https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-operators-checked.html

collection initializer?
https://stackoverflow.com/questions/5341030/f-collection-initializer-syntax

static prop inherited? FQN
use get()?

Module Checked

Xunit
- MemberData referencing another type member
- MemberType

FsCheck
-------
    
https://stackoverflow.com/questions/61506982/fscheck-not-using-registered-arbs-gens
https://stackoverflow.com/questions/36191290/when-implementing-property-based-testing-when-should-i-use-an-input-generator-o
https://stackoverflow.com/questions/57772719/generating-custom-data-in-fscheck
https://github.com/OmanF/gilded_rose/blob/master/GildedRoseTests/Tests.fs
https://stackoverflow.com/questions/36355460/calling-default-fscheck-generator-from-a-custom-generator-of-the-same-type

Use Gen.elements? Hum apparently no.
https://blog.ploeh.dk/2015/09/08/ad-hoc-arbitraries-with-fscheckxunit/
https://blog.ploeh.dk/2020/04/27/an-f-implementation-of-the-maitre-d-kata/

https://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work-3/

https://draptik.github.io/posts/2022/01/12/fsharp-writing-parameterized-xunit-tests/
https://blog.miguelbernard.com/input-generators-in-property-based-tests-with-fscheck

https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/

https://stackoverflow.com/questions/26949539/fscheck-test-change-the-range-of-values-used-for-testing
https://stackoverflow.com/questions/22719569/how-do-i-register-an-arbitrary-instance-in-fscheck-and-have-xunit-use-it
https://stackoverflow.com/questions/46827520/c-xunit-fscheck-writing-a-simple-property-based-test-using-a-custom-generato

https://github.com/fscheck/FsCheck/issues/391
https://github.com/fscheck/FsCheck/blob/master/tests/FsCheck.Test/Gen.fs#L358

https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-fsharp-with-dotnet-test

https://forums.fsharp.org/t/using-custom-generators-in-fscheck-xunit-variant/992

TODO
----

### F# struct equality?

https://stackoverflow.com/questions/28142655/iequatable-in-f-operator-performance-and-structural-equality
----> https://github.com/dotnet/fsharp/issues/526#issuecomment-119693551
https://stackoverflow.com/questions/4168220/implement-c-sharp-equality-operator-from-f

### FsCheck
http://www.nichesoftware.co.nz/2017/06/24/equality-testing-with-fscheck.html
http://jackfoxy.com/combining-fscheck-properties-in-a-single-test/

//let equalityProperty<'a when 'a : equality and 'a :> IEquatable<'a>> (x: 'a) (y: 'a) =
//    x = y
//    && x.Equals(y)
//    && x.Equals(y :> obj)

//let greaterThanProperty<'a when 'a : comparison and 'a :> IComparable<'a>> (x: 'a) (y: 'a) =
//    x > y
//    && x.CompareTo(y) > 0

//let private intGenerator =
//    Arb.Default.Int32()
//    |> Arb.filter(fun x -> -1_000_000 <= x && x <= 1_000_000)
//    |> Arb.toGen

let private twoIntsArbitrary =
    //gen { let! x = intGenerator
    //      let! y = intGenerator
    //      return x, y }
    Gen.choose (-1_000_000, 1_000_000)
    |> Gen.two
    |> Arb.fromGen
