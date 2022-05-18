// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Unbounded;

using static Zorglub.Testing.Data.Extensions.TheoryDataExtensions;

public partial class GregorianCalendarDataSet // IDayOfWeekDataSet
{
    public DataGroup<YemodaAnd<DayOfWeek>> DayOfWeekData => new()
    {
        // Epoch.
        new(1, 1, 1, DayOfWeek.Monday),
        // The 9 following days.
        // Used to test Previous(dayOfWeek) and related methods.
        new(1, 1, 2, DayOfWeek.Tuesday),
        new(1, 1, 3, DayOfWeek.Wednesday),
        new(1, 1, 4, DayOfWeek.Thursday),
        new(1, 1, 5, DayOfWeek.Friday),
        new(1, 1, 6, DayOfWeek.Saturday),
        new(1, 1, 7, DayOfWeek.Sunday),
        new(1, 1, 8, DayOfWeek.Monday),
        new(1, 1, 9, DayOfWeek.Tuesday),
        new(1, 1, 10, DayOfWeek.Wednesday),

        // Common year.
        // A week.
        new(51, 11, 6, DayOfWeek.Monday),
        new(51, 11, 7, DayOfWeek.Tuesday),
        new(51, 11, 8, DayOfWeek.Wednesday),
        new(51, 11, 9, DayOfWeek.Thursday),
        new(51, 11, 10, DayOfWeek.Friday),
        new(51, 11, 11, DayOfWeek.Saturday),
        new(51, 11, 12, DayOfWeek.Sunday),
        // January.
        new(51, 1, 1, DayOfWeek.Sunday),
        new(51, 1, 31, DayOfWeek.Tuesday),
        // February.
        new(51, 2, 1, DayOfWeek.Wednesday),
        new(51, 2, 28, DayOfWeek.Tuesday),

        // Leap year.
        // A week.
        new(40, 11, 5, DayOfWeek.Monday),
        new(40, 11, 6, DayOfWeek.Tuesday),
        new(40, 11, 7, DayOfWeek.Wednesday),
        new(40, 11, 8, DayOfWeek.Thursday),
        new(40, 11, 9, DayOfWeek.Friday),
        new(40, 11, 10, DayOfWeek.Saturday),
        new(40, 11, 11, DayOfWeek.Sunday),
        // January.
        new(40, 1, 1, DayOfWeek.Sunday),
        new(40, 1, 31, DayOfWeek.Tuesday),
        // February.
        new(40, 2, 1, DayOfWeek.Wednesday),
        new(40, 2, 29, DayOfWeek.Wednesday),

        // The month of the (official) Gregorian reform.
        new(1582, 12, 1, DayOfWeek.Wednesday),
        new(1582, 12, 2, DayOfWeek.Thursday),
        new(1582, 12, 3, DayOfWeek.Friday),
        new(1582, 12, 4, DayOfWeek.Saturday),
        new(1582, 12, 5, DayOfWeek.Sunday),
        new(1582, 12, 6, DayOfWeek.Monday),
        new(1582, 12, 7, DayOfWeek.Tuesday),
        new(1582, 12, 8, DayOfWeek.Wednesday),
        new(1582, 12, 9, DayOfWeek.Thursday),
        new(1582, 12, 10, DayOfWeek.Friday),
        new(1582, 12, 11, DayOfWeek.Saturday),
        new(1582, 12, 12, DayOfWeek.Sunday),
        new(1582, 12, 13, DayOfWeek.Monday),
        new(1582, 12, 14, DayOfWeek.Tuesday),
        new(1582, 12, 15, DayOfWeek.Wednesday),
        new(1582, 12, 16, DayOfWeek.Thursday),
        new(1582, 12, 17, DayOfWeek.Friday),
        new(1582, 12, 18, DayOfWeek.Saturday),
        new(1582, 12, 19, DayOfWeek.Sunday),
        new(1582, 12, 20, DayOfWeek.Monday),
        new(1582, 12, 21, DayOfWeek.Tuesday),
        new(1582, 12, 22, DayOfWeek.Wednesday),
        new(1582, 12, 23, DayOfWeek.Thursday),
        new(1582, 12, 24, DayOfWeek.Friday),
        new(1582, 12, 25, DayOfWeek.Saturday),
        new(1582, 12, 26, DayOfWeek.Sunday),
        new(1582, 12, 27, DayOfWeek.Monday),
        new(1582, 12, 28, DayOfWeek.Tuesday),
        new(1582, 12, 29, DayOfWeek.Wednesday),
        new(1582, 12, 30, DayOfWeek.Thursday),
        new(1582, 12, 31, DayOfWeek.Friday),

        // Common year.
        // A week.
        new(2018, 11, 5, DayOfWeek.Monday),
        new(2018, 11, 6, DayOfWeek.Tuesday),
        new(2018, 11, 7, DayOfWeek.Wednesday),
        new(2018, 11, 8, DayOfWeek.Thursday),
        new(2018, 11, 9, DayOfWeek.Friday),
        new(2018, 11, 10, DayOfWeek.Saturday),
        new(2018, 11, 11, DayOfWeek.Sunday),
        // January.
        new(2018, 1, 1, DayOfWeek.Monday),
        new(2018, 1, 31, DayOfWeek.Wednesday),
        // Feburary.
        new(2018, 2, 1, DayOfWeek.Thursday),
        new(2018, 2, 28, DayOfWeek.Wednesday),

        // Leap year.
        // A week.
        new(2020, 11, 2, DayOfWeek.Monday),
        new(2020, 11, 3, DayOfWeek.Tuesday),
        new(2020, 11, 4, DayOfWeek.Wednesday),
        new(2020, 11, 8, DayOfWeek.Sunday),
        new(2020, 11, 5, DayOfWeek.Thursday),
        new(2020, 11, 6, DayOfWeek.Friday),
        new(2020, 11, 7, DayOfWeek.Saturday),
        // January.
        new(2020, 1, 1, DayOfWeek.Wednesday),
        new(2020, 1, 31, DayOfWeek.Friday),
        // Feburary.
        new(2020, 2, 1, DayOfWeek.Saturday),
        new(2020, 2, 29, DayOfWeek.Saturday),

        // The 8 days before the last supported date.
        // Used to test Next(dayOfWeek) and related methods.
        new(9999, 12, 22, DayOfWeek.Wednesday),
        new(9999, 12, 23, DayOfWeek.Thursday),
        new(9999, 12, 24, DayOfWeek.Friday),
        new(9999, 12, 25, DayOfWeek.Saturday),
        new(9999, 12, 26, DayOfWeek.Sunday),
        new(9999, 12, 27, DayOfWeek.Monday),
        new(9999, 12, 28, DayOfWeek.Tuesday),
        new(9999, 12, 29, DayOfWeek.Wednesday),
        new(9999, 12, 30, DayOfWeek.Thursday),
        new   // Last supported date.
        (9999, 12, 31, DayOfWeek.Friday),
    };

    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Before_Data =>
        new List<(int date, int, int, int exp, int, int, DayOfWeek target)>
        {
            // Same day a week before.
            ( date: 2018, 12, 13, exp: 2018, 12,  6, target: DayOfWeek.Thursday ),
            // Preceding days.
            ( date: 2018, 12, 13, exp: 2018, 12,  7, target: DayOfWeek.Friday ),
            ( date: 2018, 12, 13, exp: 2018, 12,  8, target: DayOfWeek.Saturday ),
            ( date: 2018, 12, 13, exp: 2018, 12,  9, target: DayOfWeek.Sunday ),
            ( date: 2018, 12, 13, exp: 2018, 12, 10, target: DayOfWeek.Monday ),
            ( date: 2018, 12, 13, exp: 2018, 12, 11, target: DayOfWeek.Tuesday ),
            ( date: 2018, 12, 13, exp: 2018, 12, 12, target: DayOfWeek.Wednesday ),

            // Change of year.
            ( date: 2018,  1,  1, exp: 2017, 12, 31, target: DayOfWeek.Sunday ),

            // Change of month.
            ( date: 2018, 12,  1, exp: 2018, 11, 28, target: DayOfWeek.Wednesday ),
            // February, common year.
            ( date: 2018,  3,  1, exp: 2018,  2, 28, target: DayOfWeek.Wednesday ),
            // February, leap year.
            ( date: 2016,  3,  1, exp: 2016,  2, 29, target: DayOfWeek.Monday ),
        }
        .MapToTheoryDataOfTwoYemodas();

    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrBefore_Data =>
        new List<(int date, int, int, int exp, int, int, DayOfWeek target)>
        {
            // Preceding days.
            ( date: 2018, 12, 13, exp: 2018, 12,  7, target: DayOfWeek.Friday ),
            ( date: 2018, 12, 13, exp: 2018, 12,  8, target: DayOfWeek.Saturday ),
            ( date: 2018, 12, 13, exp: 2018, 12,  9, target: DayOfWeek.Sunday ),
            ( date: 2018, 12, 13, exp: 2018, 12, 10, target: DayOfWeek.Monday ),
            ( date: 2018, 12, 13, exp: 2018, 12, 11, target: DayOfWeek.Tuesday ),
            ( date: 2018, 12, 13, exp: 2018, 12, 12, target: DayOfWeek.Wednesday ),
            // Same day.
            ( date: 2018, 12, 13, exp: 2018, 12, 13, target: DayOfWeek.Thursday ),

            // Change of year.
            ( date: 2018,  1,  1, exp: 2017, 12, 31, target: DayOfWeek.Sunday ),

            // Change of month.
            ( date: 2018, 12,  1, exp: 2018, 11, 28, target: DayOfWeek.Wednesday ),
            // February, common year.
            ( date: 2018,  3,  1, exp: 2018,  2, 28, target: DayOfWeek.Wednesday ),
            // February, leap year.
            ( date: 2016,  3,  1, exp: 2016,  2, 29, target: DayOfWeek.Monday ),
        }
        .MapToTheoryDataOfTwoYemodas();

    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_Nearest_Data =>
        new List<(int date, int, int, int exp, int, int, DayOfWeek target)>
        {
            // Preceding days.
            ( date: 2018, 12,  6, exp: 2018, 12,  3, target: DayOfWeek.Monday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  4, target: DayOfWeek.Tuesday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  5, target: DayOfWeek.Wednesday ),
            // Same day.
            ( date: 2018, 12,  6, exp: 2018, 12,  6, target: DayOfWeek.Thursday ),
            // Following days.
            ( date: 2018, 12,  6, exp: 2018, 12,  7, target: DayOfWeek.Friday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  8, target: DayOfWeek.Saturday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  9, target: DayOfWeek.Sunday ),

            // Change of year.
            ( date: 2018, 12, 31, exp: 2019,  1,  1, target: DayOfWeek.Tuesday ),

            // Change of month.
            ( date: 2018, 11, 30, exp: 2018, 12,  1, target: DayOfWeek.Saturday ),
            // February, common year.
            ( date: 2018,  2, 28, exp: 2018,  3,  1, target: DayOfWeek.Thursday ),
            // February, leap year.
            ( date: 2016,  2, 29, exp: 2016,  3,  1, target: DayOfWeek.Tuesday ),
        }
        .MapToTheoryDataOfTwoYemodas();

    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_OnOrAfter_Data =>
        new List<(int date, int, int, int exp, int, int, DayOfWeek target)>
        {
            // Same day.
            ( date: 2018, 12,  6, exp: 2018, 12,  6, target: DayOfWeek.Thursday ),
            // Following days.
            ( date: 2018, 12,  6, exp: 2018, 12,  7, target: DayOfWeek.Friday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  8, target: DayOfWeek.Saturday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  9, target: DayOfWeek.Sunday ),
            ( date: 2018, 12,  6, exp: 2018, 12, 10, target: DayOfWeek.Monday ),
            ( date: 2018, 12,  6, exp: 2018, 12, 11, target: DayOfWeek.Tuesday ),
            ( date: 2018, 12,  6, exp: 2018, 12, 12, target: DayOfWeek.Wednesday ),

            // Change of year.
            ( date: 2018, 12, 31, exp: 2019,  1,  1, target: DayOfWeek.Tuesday ),

            // Change of month.
            ( date: 2018, 11, 30, exp: 2018, 12,  1, target: DayOfWeek.Saturday ),
            // February, common year.
            ( date: 2018,  2, 28, exp: 2018,  3,  1, target: DayOfWeek.Thursday ),
            // February, leap year.
            ( date: 2016,  2, 29, exp: 2016,  3,  1, target: DayOfWeek.Tuesday ),
        }
        .MapToTheoryDataOfTwoYemodas();

    public TheoryData<Yemoda, Yemoda, DayOfWeek> DayOfWeek_After_Data =>
        new List<(int date, int, int, int exp, int, int, DayOfWeek target)>
        {
            // Following days.
            ( date: 2018, 12,  6, exp: 2018, 12,  7, target: DayOfWeek.Friday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  8, target: DayOfWeek.Saturday ),
            ( date: 2018, 12,  6, exp: 2018, 12,  9, target: DayOfWeek.Sunday ),
            ( date: 2018, 12,  6, exp: 2018, 12, 10, target: DayOfWeek.Monday ),
            ( date: 2018, 12,  6, exp: 2018, 12, 11, target: DayOfWeek.Tuesday ),
            ( date: 2018, 12,  6, exp: 2018, 12, 12, target: DayOfWeek.Wednesday ),
            // Same day a week after.
            ( date: 2018, 12,  6, exp: 2018, 12, 13, target: DayOfWeek.Thursday ),

            // Change of year.
            ( date: 2018, 12, 31, exp: 2019,  1,  1, target: DayOfWeek.Tuesday ),

            // Change of month.
            ( date: 2018, 11, 30, exp: 2018, 12,  1, target: DayOfWeek.Saturday ),
            // February, common year.
            ( date: 2018,  2, 28, exp: 2018,  3,  1, target: DayOfWeek.Thursday ),
            // February, leap year.
            ( date: 2016,  2, 29, exp: 2016,  3,  1, target: DayOfWeek.Tuesday ),
        }
        .MapToTheoryDataOfTwoYemodas();
}
