TODO
====

- Public Range.Maximal32?
- Yemoda as a binary repr for a date type when the later does not want to
  support y = 0 which is the default value of Yemoda

https://stackoverflow.com/questions/61506982/fscheck-not-using-registered-arbs-gens
https://stackoverflow.com/questions/36191290/when-implementing-property-based-testing-when-should-i-use-an-input-generator-o
https://stackoverflow.com/questions/57772719/generating-custom-data-in-fscheck
https://github.com/OmanF/gilded_rose/blob/master/GildedRoseTests/Tests.fs
https://stackoverflow.com/questions/36355460/calling-default-fscheck-generator-from-a-custom-generator-of-the-same-type

New code coverage? https://github.com/FortuneN/FineCodeCoverage
https://devblogs.microsoft.com/dotnet/automate-code-metrics-and-class-diagrams-with-github-actions/
https://github.com/microsoft/dotnet/

FIXME
-----

- Check all occurences of ICalendricalSchema and CalendricalSchema.
- Parts and validation.
  Types concernés:
  - ICalendricalschemaPlus OK
  - ArithmeticalSchema
  - CalendricalSegmentBuilder
  - PartsFactory
  - Arithmetic
  DefaultArithmetic and Yemoda/Yedoy.
  TryGetCustomPreValidator() special cases of GJ
  ArithmeticalSchema -> private protected ctor
- Check scopes (ICalendricalScope) and Yemoda.SupportedYears / PartsFactory.
- CLSCompliant
- GregorianSchema.Instance

TODO
----

- Rewrite the test scripts: one for tesing and code coverage, one for packing.
- When .NET 7 is out:
  D.B.props: set LangVersion to latest.
  D.B.targets: disable preview features.
  Clean up unnecessary "#pragma warning disable".
  Generic math: using System.Numerics.
- Clean up compiler symbols.
- Exception messages: use ThrowHelpers.
  Onlu use OverflowException for truely arithmetic overflows.
  XML doc: "would", "was"
- Era type as a range of DayNumber's.
- ISpanFormattable
- Clean up DIM.
  Beware of DIM and structs.
  https://mattwarren.org/2020/02/19/Under-the-hood-of-Default-Interface-Methods/
- XML doc/comments: currently largely outdated or badly written.
- ISO calendar.

Subprojects
-----------

- C# source generators for specialized date types?
- C# analyzers.
- Parsing / formatting.

Calendars
---------

https://fr.wikipedia.org/wiki/Calendrier_solaire_de_364_jours
