
[![smoke](https://github.com/chtoucas/Zorglub.Time/workflows/smoke/badge.svg)](https://github.com/chtoucas/Zorglub.Time/actions?query=workflow%3Asmoke)
[![test](https://github.com/chtoucas/Zorglub.Time/workflows/test/badge.svg)](https://github.com/chtoucas/Zorglub.Time/actions?query=workflow%3Atest)
[![Coverlet](./test/coverage.svg)](./test/coverage.md)

An experimental calendar library with a focus on calendrical calculations,
extensibility and performance.

_This project is heavily inspired by NodaTime and JodaTime._

Features
--------

### Core calendars

We restrict ourselves to calendars supposedly in use today:
- Armenian
- Civil
- Coptic
- Ethiopic
- Gregorian
- Julian
- Tabular Islamic
- Zoroastrian

NB: most computer implementations of calendars with epagomenal days add a
thirteen month to hold them, we don't, but see below if you don't agree.

Features:
- Ability to create your own calendars.
- Custom types to represent a year, a month and a range of years, months or days.
- Custom arithmetic.

### Supported Calendars

Arithmetical calendars:
- Armenian (*)
- Civil
- Coptic (*)
- Egyptian (*)
- Ethiopic (*)
- Gregorian
- Holocene
- Julian
- Zoroastrian (*)
- _Arithmetisation of astronomical calendars:_
  - French Republican (*)
  - French Revolutionary, but only for years between I and XIV
  - Persian
  - Tabular Islamic
- _Perennial blank-day calendars:_
  - International Fixed
  - Positivist aka Georgian
  - World aka Universal, and Revised World
- _Miscellaneous calendars:_
  - Tropicália (three versions)
  - Universal

(*) Calendar available in two forms: 12 months or 12 months plus a virtual
thirteenth month.

For one reason or another, most proposed reforms are considered defective and
therefore have seen limited adoption, if any.

### Future additions?

For the sole purpose of validating the API, we would like to have at least one
calendar in each category.

Arithmetical calendars:
- _Embolismic calendars:_
  - Hebrew
- _Perennial leap-week calendars:_
  - Pax
  - ISO
- Hybrid calendars:
  - Julian-Gregorian
- Religious calendars:
  - Orthodox
  - Roman
- Imaginary calendars:
  - Pataphysical
  - Tolkian
- Offsetted calendars:
  - Mingo
  - Thai solar
  - Masonic, almost identical to the Gregorian calendar: a year starts the 1st
    of March and it uses the Anno Lucis.

Astronomical calendars:
- Persian (Solar Hijri)
- French Revolutionary
- Badi
- Chinese
- Vietnamese
