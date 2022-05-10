
Features
--------

An experimental calendar library with a focus on extensibility and performance.
As of now, we are only interested in arithmetical calendars.

### Core calendars

We restrict ourselves to calendars supposedly in use today:
- Armenian
- Coptic
- Ethiopic
- Gregorian
- Julian
- Tabular Islamic
- Zoroastrian

NB: most computer implementations of calendars with epagomenal days add a
thirteen month to hold them, we don't, but see below if you don't agree.

### Extensibility

One can create new calendars from a schema and an epoch
(see `Narvalo.Calendars.Specialized`).

Below, 12+1 means 12 months plus a virtual thirteenth month.

Arithmetical schemas:
- Coptic (12 or 12+1 months)
- Egyptian (12 or 12+1 months)
- Gregorian
- Julian
- Tabular Islamic
Arithmetisation of astronomical schemas:
- French Republican (12 or 12+1 months)
- Persian
Reforms:
- Tropicália (three versions)

For one reason or another, most proposed reforms are considered defective and
therefore have seen limited adoption, if any.

### Future additions?

For the sole purpose of validating the API, we would like to
have at least one schema in each category.
- Embolismic schemas:
  * Hebrew (*)
- Perennial blank-day schemas:
  * International Fixed (*)
  * Positivist aka Georgian (*)
  * World aka Universal (*)
- Perennial leap-week schemas (see also Reform.md in `Narvalo.Sketches`):
  * Pax (*)
  * ISO
- Astronomical schemas:
  * Persian (Solar Hijri)
  * French Revolutionary
  * Badi
  * Chinese
  * Vietnamese
- Other types of schemas:
  * Julian-Gregorian, an hybrid schema (*)
  * Pataphysical, an imaginary schema
  * Tolkian
  * Masonic, almost identical to the Gregorian calendar: a year starts the 1st
    of March and it uses the Anno Lucis.
- etc.

(*) A prototype can be found in `Narvalo.Sketches`.
