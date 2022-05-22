About F#
========

TODO
----

### Tests

- How should we test inequality: random or custom data? <- custom data.
- DataSet and FQ. Sometimes, I use ClassData to avoid this problem.
- Today, les tests peuvent échouer si entre les deux appels à Today() ou 
  UtcToday(), on a changé de jour...

### F\#

Out params
https://stackoverflow.com/questions/28691162/the-f-equivalent-of-cs-out
https://stackoverflow.com/questions/64834220/f-use-c-sharp-methods-with-out-parameter-within-arrays-and-void-return

- Really understand the difference between `^a` and `'a`.
- Static member in interface
  https://github.com/fsharp/fsharp/blob/master/src/fsharp/FSharp.Core/collections.fsi
- Type abbreviations

https://simendsjo.me/fsharp-intro/

### Xunit

Theory:
https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/

AssemblyFixture (see Xunit samples) for easier testing of CalendarCatalog.

Skippable facts.
In the end, I prefer to pre-filter the data rather than dynamically skipping tests.
Requirements:
- ability to skip a test dynamically
- do not display a message (optional)
- compatible with Xunit v3
  - Assert.Skip(string reason)
  - Assert.SkipUnless(bool condition, string reason)
  - Assert.SkipWhen(bool condition, string reason)
xunit.extensibility.execution
https://github.com/AArnott/Xunit.SkippableFact/
https://github.com/xunit/samples.xunit/tree/main/DynamicSkipExample
https://github.com/xunit/xunit/issues/2073
https://github.com/xunit/assert.xunit/commit/e6a6d5d22bbc7097f8decad5b3c8cac8cf3fb386

### Other

- Mocking? I liked Moq, but I don't know if it would be useful here: just a 
  handful of rather simple fakes which I might even replace by F# object expressions.
- [dotMemory Unit](https://www.jetbrains.com/dotmemory/unit/)
- [AltCover](https://www.nuget.org/packages/altcover/)
- Dummy Data Generator? FsCheck does the job already.

https://github.com/dariusz-wozniak/List-of-Testing-Tools-and-Frameworks-for-.NET


Tests
-----

We test the C# libraries using F#.
Advantages:
- concise syntax
- more flexibility when naming functions/methods, no need for DisplayName
- FsCheck
  - merge several tests while still keeping them separated... 
    For this to work, we must use the `.&.` combinator and add a label to the
    subproperties via `|@`. In fact, we are using FsCheck + Xunit, and 
    `dotnet test` makes it difficult to print anything to the console, therefore
    I prefer to use Xunit asserts. This way we get the line where an assertion
    failed.
  - instead of choosing random data, we let FsCheck do it for us.
    The real benefice is that it forces us to specify the data, it's no longer 
    random.
Disadvantages:
- it's too easy to forget the () in a let binding. The consequence is that the
  test will not run.
- no Xunit analyzer.
- even if the bridge from C# to F# is much better these days, it's still not 
  possible to refactor a piece of code written in C# and see the F# code 
  depending on it updated magically. 
  Our C# code ought to be quite stable, otherwise it gives more work to the 
  developer.

Ceci étant dit, une grande partie de l'infrastructure de tests est écrite en C#.
À ce propos, quand une méthode abstraite a l'attribut `Fact`, il n'est pas 
nécessaire de répéter cet attribut dans la méthode dérivée.

On utilise principalement Xunit et FsCheck.
Unquote ? La librairie est très facile à utiliser, en cas d'erreur on obtient 
des messages bien plus faciles à comprendre, et quand on teste une exception
il n'est plus nécessaire d'utiliser une fonction anonyme. Il faut quand même
faire attention car, dans une expression Unquote, l'égalité y est structurelle, 
ce qui n'est pas nécessairement ce qu'on souhaite tester. 

Au final, la quasi-totalité des tests étant formée de tests d'égalité, de vérité 
ou d'exception, Unquote n'apporte pas grand chose.
En pratique, on n'utilise Unquote que dans le cas de tests complexes et 
lorsqu'on souhaite débugger un test.

### Xunit

Traits: see file "Zorglub.Testing.Traits.cs".

### Typical Layout of a Test Module

#### Value Types
- TestCommon
- Prelude: 
  - default value
  - constructor
  - deconstructor
  - ToString()
  - properties
- Factories
- Conversions
- Adjustements
- Equality
- Comparison
- Math
- Postlude: non-unit tests

### Traps

It's easy to forget `()`. It would be nice if we couldn't apply the attr `Fact`
`Theory` or `Property` to a variable that is not a function or method.
Maybe we could write a test that checks that?

When testing inequality do NOT write:
```
[<Property>]
let ``Equality when both operands are distinct`` (x: Range<int>) (x: Range<int>) = x <> y &&& (...)
```
By using a global arbitrary for `Range<int>` and filtering the data with (x <> y),
we test nothing since we filter using the operator we wish to test.
The common pattern is to define an arbitrary, usually called `xyArbitrary`, just
for that purpose.

### Tester une librairie C# en F# ?

On ne peut pas tester les opérateurs C# `++` et `--` ?

On ne peut pas tester directement les opérateurs C# `!=`, `<=` et `>=`, 
néanmoins on peut utiliser `op_Inequality`, `op_LessThanOrEqual` et
`op_GreaterThanOrEqual`.

### FsCheck

Critiques :
- la documentation laisse à désirer ;
- fonctionnement un peu trop magique à mon goût ;
- `Arb.register()` ne fonctionne pas toujours quand on utilise aussi Xunit ;

Questions :
- quelle est la différence entre `Arb.from<int>` et `Arb.Default.Int32()` ?

Alternatives ?
- [Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog)
  See https://github.com/haf/expecto/issues/136#issuecomment-292935855
- [CsCheck](https://github.com/AnthonyLloyd/CsCheck)
- [Hypothesis](https://github.com/HypothesisWorks)

Autres possibilités mais uniquement pour générer des données aléatoirement
- Bogus 
- GenFu
- AutoFixture

#### Record vs SCU 
I mostly use record structs instead of single-case discriminated unions.
I use SCUs to test the core date parts but only to keep a record of how it can
be done.
Small records: Struct and IsReadOnly.

#### Alternatives to `Arb.register()`
At function level.
> [<Property(Arbitrary = [| typeof<Arbitraries> |] )>]
> let ``a test`` (x: XXX) =
At module level.
> [<Properties(Arbitrary = [| typeof<_Arbitraries> |] )>]
> module ...
At assembly level.
> [<assembly: Properties( Arbitrary = [| typeof<Arbitraries> |] )>] do()

It seems to be a problem when using FsCheck with Xunit.
https://github.com/fscheck/FsCheck/issues/390
https://blog.ploeh.dk/2015/09/08/ad-hoc-arbitraries-with-fscheckxunit/

#### Problems?
https://github.com/fscheck/FsCheck/issues/413

#### References
https://blog.ploeh.dk/2016/06/28/roman-numerals-via-property-based-tdd/

#### Integration with Xunit
https://fscheck.github.io/FsCheck/RunningTests.html#Using-FsCheck-Xunit

Sortie console
dotnet test --logger:"console;verbosity=detailed"
https://stackoverflow.com/questions/61295484/logging-to-output-with-fscheck-xunit
https://github.com/fscheck/FsCheck/issues/508

https://stackoverflow.com/questions/38839721/how-do-i-implement-multiple-argument-generation-using-fscheck/38841255


F# vs C#
--------

No F# analyzers.

### Modules
Modules are like static class but only better; if I'm not mistaken modules are 
compiled to static classes.
Better:
- simple syntax
- naturally nestable

### Static fields

https://stackoverflow.com/questions/60535718/is-it-possible-to-define-public-static-readonly-field-in-f

### Classes
I can't get to remember the syntax for classes or interfaces... I find it 
unnatural and kind of obscure.

No protected member.
https://stackoverflow.com/questions/2390515/why-isnt-there-a-protected-access-modifier-in-f

Init-only properties
https://github.com/fsharp/fslang-suggestions/issues/904

### Structural Equality
`NonStructuralComparison`
Caveats: it's supports primitive types, it does not work for enums like e.g.
DayOfWeek.

Good news: Xunit asserts use the right Equals

https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-operators-nonstructuralcomparison.html

### Extension Methods
Il ne semble pas possible d'utiliser une méthode d'extension définie en C# sur 
une énumération. Raison ? On ne peut définir en F# des méthodes d'extension que 
pour des types ouverts.
https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/type-extensions

### Function Overloading
Il n'est pas possible de nommer de la même manière deux fonctions ayant juste
des paramètres différents. Solution : méthodes dans une classe ou "discriminated 
union".
https://stackoverflow.com/questions/501069/functions-with-generic-parameter-types

### Method Overloading
Si une méthode en surcharge une autre avec des paramètres "out", on doit utiliser
une variable "mutable".

### Warnings & Errors
`#nowarn` applies to the __entire__ file

Les messages d'erreur sont souvent cryptiques et n'aident pas beaucoup.

FS0405: protected internal method ?

### Pattern matching
Le premier match doit être constant. Si ce n'est pas le cas, on peut écrire :
let notSame =
    match dayNumber with
    | v when v = DayNumber.MaxValue -> dayNumber - 1
    | _ -> dayNumber + 1

### Null's

https://latkin.org/blog/2015/05/18/null-checking-considerations-in-f-its-harder-than-you-think/

### Visual Studio
VS : pas de "Task List" :-(


Style
------

`'a` or `'T`? We don't follow the F# common rule. We use the first form with 
functions and the other one with types.

We us FSharpLint; see script `eng\lint.cmd`.

Custom rules:
- memberNames, underscores: AllowPrefix -> AllowAny
- redundantNewKeyword, enabled: true -> false

https://docs.microsoft.com/en-us/dotnet/fsharp/style-guide/conventions


Références
----------

https://fsharp.org/community/projects/
https://github.com/fsprojects/awesome-fsharp
