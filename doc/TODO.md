TODO
====

Naming: ident, id, cuid <- harmoniser
Struct fields instead of auto-props
SupportedYearsTester

Calendar<TDate> & Date: Min/MaxXXX and new prop Epoch (as a Date not a DayNumber,
maybe use Origin to avoid confusion). Remove IMinMaxValue
Mono-date: which props? epoch, domain

Explain the lessons learned so far: daysSinceEpoch is better than y/doy which is
better than y/m/d, poly-calendar is not such a good idea (JulianDate/Calendar
contains the name of the calendar, we can add specific methods). Proleptic is
only a bit slower than Standard (> 0).

- Math:
  * Optimize div in N / uint in enum comp / ulong?
  * Profile: OtherRegular, Other7 (MinDaysInMonth >= 7)?
  * Move standard ops (on CalendarMonth) from Math to Arithmetic.
    Problem: optimization of AddMonths() and Count...() depending on the profile.
    For regular schema, it's fine, but for lunisolar schema?
  * PlainMath, tests for ordinal dates.
  * API for CalendarDay (PlusYears, PlusMonths?), humm no, use conversion to
    CalendarDate. We have the three forms of dates for that purpose.
- Tests should not perform any conversion, e.g. CalendarDate -> OrdinalDate.

- pack.ps1: version numbering (timestamp).
  https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/versioning
  https://andrewlock.net/version-vs-versionsuffix-vs-packageversion-what-do-they-all-mean/
- Publication: GitHub & NuGet (for NuGet only when reach 1.0.0).
- When .NET 7 is out:
  D.B.props: LangVersion
  D.B.targets: disable preview features (ENABLE_PREVIEW_FEATURES).
  NET7_0_OR_GREATER (generic constraint: Range & tests)
  Remove unnecessary `#pragma warning disable`.
  Remove Future.cs
  Github action: test on Ubuntu and MacOS.
  C# 11
  - checked ops.
  - required for props.
  https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11
  https://devblogs.microsoft.com/dotnet/dotnet-7-generic-math/
  https://docs.microsoft.com/en-us/dotnet/standard/generics/math
- Clean up compiler symbols.
- CLSCompliant
- Exception messages: use ThrowHelpers everywhere.
  Only use OverflowException for truely arithmetic overflows?
  XML doc: "would", "was"
- Era type as a range of DayNumber's.
- ISpanFormattable
- Clean up DIM. Beware of DIM and structs.
  https://mattwarren.org/2020/02/19/Under-the-hood-of-Default-Interface-Methods/
- New code coverage? https://github.com/FortuneN/FineCodeCoverage
  https://devblogs.microsoft.com/dotnet/automate-code-metrics-and-class-diagrams-with-github-actions/


Plan?
-----

### Version 1.0
- Stable API and full test coverage.
  * Complete Core
  * At least one Lunisolar calendar (Hebrew)
  * At least one leap week calendar (ISO & Pax)
- XML doc/comments: currently largely outdated or badly written.
  https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags
- .NET 7, we use preview features

### Version 1.x
- Geometry
- Hemerology

### Version 2.x
- Horology

### Misc
- Eras, an era is a named range of day numbers. Relation to scopes & calendars?
- Day scales, moment, epoch
  Julian Day, Modified Julian Day
- Time scales: TAI, TT, UTC
- Religious calendars, liturgical calendars, computus
- Regnal calendars or (better?) dates
- Hybrid calendars or better?) dates, Gregorianizer
- Fictional calendars based on IAffineDate
- Time zones
  https://codeblog.jonskeet.uk/2019/03/27/storing-utc-is-not-a-silver-bullet/

https://fr.wikipedia.org/wiki/Calendrier_solaire_de_364_jours
https://en.wikipedia.org/wiki/Anthropocene
https://en.wikipedia.org/wiki/Geologic_time_scale
https://en.wikipedia.org/wiki/Geologic_Calendar
https://www.britannica.com/science/geologic-time
https://en.wikipedia.org/wiki/Counting#Inclusive_counting
https://en.wikipedia.org/wiki/Year
https://en.wikipedia.org/wiki/Old_Style_and_New_Style_dates
https://en.wikipedia.org/wiki/Julian_calendar#New_Year's_Day
https://en.wikipedia.org/wiki/Regnal_year


Subprojects
-----------

- T4 vs C# Source Generators
  T4: simple but requires VS
  SG: Code Analysis? VS experience is less than stellar
  https://github.com/amis92/csharp-source-generators
  - Enum.IsDefined
  - DIM (ICalendricalSchemaPlus)
  - specialized date types
  T4 TODOs:
    Use Mono.T4? Advantage: compatible with .NET Core.
    TransformOnBuild (Zorglub and Zorglub.Extras).
```xml
<Import Project="$(MSBuildExtensionsPath)\Microsoft\VisualStudio\v$(VisualStudioVersion)\TextTemplating\Microsoft.TextTemplating.targets"
          Condition=" '$(BuildingInsideVisualStudio)' == 'true' " />
<PropertyGroup>
<TransformOnBuild>true</TransformOnBuild>
<TransformOutOfDateOnly Condition=" '$(Configuration)' == 'Debug' ">true</TransformOutOfDateOnly>
</PropertyGroup>
<ItemGroup>
<T4ParameterValues Include="XXX">
  <Value>$(XXX)</Value>
</T4ParameterValues>
</ItemGroup>
```
    Shadowing

    https://learn.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates
    https://learn.microsoft.com/en-us/visualstudio/modeling/design-time-code-generation-by-using-t4-text-templates
    https://learn.microsoft.com/en-us/visualstudio/modeling/code-generation-in-a-build-process
- Parsing / formatting.


Design & Performance
--------------------

JIT ASM
-  https://github.com/dotnet/coreclr/blob/master/Documentation/building/viewing-jit-dumps.md
-  https://benchmarkdotnet.org/articles/features/disassembler.html
-  https://github.com/xoofx/JitBuddy
-  https://github.com/0xd4d/JitDasm
-  https://github.com/EgorBo/Disasmo
-  https://github.com/icedland/iced


Useful References
-----------------

### JodaTime
https://blog.joda.org/2014/11/converting-from-joda-time-to-javatime.html
https://blog.joda.org/2009/11/why-jsr-310-isn-joda-time_4941.html

### Java SE
https://stackoverflow.com/questions/32437550/whats-the-difference-between-instant-and-localdatetime

### Other libraries
https://docs.python.org/3/library/calendar.html
https://github.com/chronotope/chrono
https://day.js.org/
https://tc39.es/proposal-temporal/
https://tc39.es/proposal-temporal/docs/
https://www.infoq.com/news/2022/09/luxon-date-management/

C# Features
-----------

"covariant return type" ne marche pas pour une propriété provenant d'une
interface ou ayant un "setter".

https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/covariant-returns


À classer
---------

Nychthemeron is part of the jargon of calendar studies, it is
a period of 24 consecutive hours. In a uniform time scale, like TAI,
TT or UT1, it is equivalent to a period of 86,400 consecutive seconds.
In this context, it is rather a definition since the second is the
base unit. In a non-uniform time scale, like UTC, there is not such
thing as a nychthemeron --- one could argue that there is not one
but many kinds of nychthemerons. This is not really a concern for
what we have in mind here. We define a nychthemeron to be a period
of time spanning from midnight of one day to the onset of the
following day. The property `DaysSinceEpoch` is then the
number of consecutive nychthemerons from midnight of the epoch of
the calendar to midnight of the current instance.

- alt arith: see date4j.net, https://github.com/MenoData/Time4J
- Formatting: short, long, ISO, format, culture-dependent variants. Parsing,
  `IFormattable`.
- Serialization: `Serializable`, `IXmlSerializable`, `FromBinary`, `ToBinary`.
- F# & VB.NET: out params are difficult to use, `end` is a reserved keywords.
  Custom methods: value tuples and `Option<>`.
- Can method with out params be inlined? (see DOES_NOT_RETURN in second ref)
  * https://mattwarren.org/2016/03/09/adventures-in-benchmarking-method-inlining/
  * https://github.com/dotnet/coreclr/blob/master/src/jit/inline.def

https://nietras.com/2021/06/14/csharp-10-record-struct/

abstract override
https://ericlippert.com/2011/02/07/strange-but-legal/

Simply use a ValueTuple instead?
https://github.com/dotnet/roslyn/issues/347

https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/
https://habr.com/en/post/467689/

https://blog.ploeh.dk/2018/02/19/abstract-class-isomorphism/

https://datatracker.ietf.org/doc/html/rfc3339#appendix-B

CRTP (Curiously Recurring Template Pattern)
TSelf out
https://docs.microsoft.com/en-us/archive/blogs/ericlippert/curiouser-and-curiouser
https://github.com/dotnet/csharplang/discussions/169
https://dev.to/entomy/real-traits-in-c-4fpk
https://zpbappi.com/curiously-recurring-template-pattern-in-csharp/

https://devblogs.microsoft.com/dotnet/customizing-trimming-in-net-core-5/
https://www-fourier.ujf-grenoble.fr/~parisse/giac/doc/en/cascmd_en/node2.html

- Devirtualization
  https://www.youtube.com/watch?v=4yALYEINbyI
  https://medium.com/@idlerboris/hashsets-tricks-in-net-devirtualization-67871da5abf4
  https://www.infoq.com/news/2017/12/Devirtualization/
- Tricks
  https://stackoverflow.com/questions/22258070/datetime-dayofweek-micro-optimization
  https://github.com/nodatime/nodatime/blob/master/src/NodaTime/Utility/TickArithmetic.cs
  https://devblogs.microsoft.com/premier-developer/the-in-modifier-and-the-readonly-structs-in-c/
  https://stackoverflow.com/questions/11227809/why-is-processing-a-sorted-array-faster-than-processing-an-unsorted-array
  https://github.com/john-h-k/MathSharp
  https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-7.2/readonly-ref

