TODO
====

FIXME
-----

- Refactor CalendarCatalog, split into a registry and a registar.
  Currently, it's almost impossible to achieve full code coverage.
- Schemas:
  * Move prop Arithmetic from CalendricalSchema to SystemSchema.
  * Remove things using a conversion (y, m, d) <-> (y, doy)?
    - CountDaysInMonthBefore(y, doy)
    - CountDaysInMonthAfter(y, doy)
    - GetOrdinalPartsAtStartOfMonth(int y, int m)
    - GetOrdinalPartsAtEndOfMonth(int y, int m)
- PartsFactory & PartsCreator
  * Remove Yemoda.AtStartOfYear() & co (kind of).
- Math:
  * Profile: OtherRegular, Other7 (MinDaysInMonth >= 7)?
  * Move standard ops (on CalendarMonth) from Math to Arithmetic.
    Problem: optimization of AddMonths() and Count...() depending on the profile.
    For regular schema, it's fine, and for lunisolar schema?
  * PlainMath, tests for ordinal dates.
  * API for CalendarDay (PlusYears, PlusMonths?), humm no, use conversion to
    CalendarDate. We have the three forms of dates for that purpose.
- NotImplementedException
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
- Publication: GitHub & NuGet (for NuGet only when reach 1.0.0).
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
- New code coverage? https://github.com/FortuneN/FineCodeCoverage
  - https://devblogs.microsoft.com/dotnet/automate-code-metrics-and-class-diagrams-with-github-actions/
  - https://github.com/microsoft/dotnet/


Plan?
-----

### Version 1.0
- Stable API and full test coverage.
  * Complete Core
  * At least one Lunisolar calendar (Hebrew)
  * At least one leap week calendar (ISO & Pax)
- XML doc/comments: currently largely outdated or badly written.
- .NET 7, we use preview features

### Version 1.x
- Geometry
- Hemerology

### Version 2.x
- Horology


Subprojects
-----------

- C# source generators for specialized date types?
- C# analyzers.
- Parsing / formatting.


Calendars
---------

https://fr.wikipedia.org/wiki/Calendrier_solaire_de_364_jours
