TODO
====

FIXME
-----

- Refactor CalendarCatalog, split into a registry and a registar.
  Currently, it's almost impossible to achieve full code coverage.
- Math: PlainMath, tests for ordinal dates.
  API for CalendarDay (PlusYears, PlusMonths?), humm no, use conversion to
  CalendarDate. We have the three forms of dates for that purpose.
- Add UserCalendars to all tests.
- DayOfWeekTester: Nearest().
- Check all occurences of ICalendricalSchema and CalendricalSchema.
- Parts and validation.
  Types concernés:
  - ICalendricalschemaPlus OK
  - SystemSchema
  - CalendricalSegmentBuilder
  - PartsFactory
  - Arithmetic
  DefaultArithmetic and Yemoda/Yedoy.
  TryGetCustomPreValidator() special cases of GJ
  ArithmeticalSchema -> private protected ctor
- Check scopes (ICalendricalScope) and Yemoda.SupportedYears / PartsFactory.
- Tests should not perform any conversion, e.g. CalendarDate -> OrdinalDate.
- CLSCompliant
- Yemoda as a binary repr for a date type when the later does not want to
  support y = 0 which is the default value of Yemoda


TODO
----

- Packaging: version numbering (timestamp).
- Publication: GitHub & NuGet.
- When .NET 7 is out:
  D.B.targets: disable preview features.
  Clean up unnecessary "#pragma warning disable".
  Generic math: using System.Numerics.
  Github action: test on Ubuntu and MacOS.
- Clean up compiler symbols.
- Exception messages: use ThrowHelpers.
  Only use OverflowException for truely arithmetic overflows?
  XML doc: "would", "was"
- Era type as a range of DayNumber's.
- ISpanFormattable
- Clean up DIM.
  Beware of DIM and structs.
  https://mattwarren.org/2020/02/19/Under-the-hood-of-Default-Interface-Methods/
- XML doc/comments: currently largely outdated or badly written.
- ISO calendar.
- LunisolarMath
- New code coverage? https://github.com/FortuneN/FineCodeCoverage
  - https://devblogs.microsoft.com/dotnet/automate-code-metrics-and-class-diagrams-with-github-actions/
  - https://github.com/microsoft/dotnet/


Subprojects
-----------

- C# source generators for specialized date types?
- C# analyzers.
- Parsing / formatting.

Calendars
---------

https://fr.wikipedia.org/wiki/Calendrier_solaire_de_364_jours
