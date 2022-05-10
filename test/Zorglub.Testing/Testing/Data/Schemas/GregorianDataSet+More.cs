// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Testing.Data.Schemas;

public partial class GregorianDataSet // IDaysAfterDataSet
{
    public TheoryData<YemoAnd<int>> DaysInYearAfterMonthData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 334),
        new(CommonYear, 2, 306),
        new(CommonYear, 3, 275),
        new(CommonYear, 4, 245),
        new(CommonYear, 5, 214),
        new(CommonYear, 6, 184),
        new(CommonYear, 7, 153),
        new(CommonYear, 8, 122),
        new(CommonYear, 9, 92),
        new(CommonYear, 10, 61),
        new(CommonYear, 11, 31),
        new(CommonYear, 12, 0),
        // Leap year.
        new(LeapYear, 1, 335),
        new(LeapYear, 2, 306),
        new(LeapYear, 3, 275),
        new(LeapYear, 4, 245),
        new(LeapYear, 5, 214),
        new(LeapYear, 6, 184),
        new(LeapYear, 7, 153),
        new(LeapYear, 8, 122),
        new(LeapYear, 9, 92),
        new(LeapYear, 10, 61),
        new(LeapYear, 11, 31),
        new(LeapYear, 12, 0),
    };

    public TheoryData<YemodaAnd<int>> DaysInYearAfterDateData { get; } = new()
    {
        // Common year.
        new(CommonYear, 1, 31, 334),
        new(CommonYear, 2, 28, 306),
        new(CommonYear, 3, 31, 275),
        new(CommonYear, 4, 30, 245),
        new(CommonYear, 5, 31, 214),
        new(CommonYear, 6, 30, 184),
        new(CommonYear, 7, 31, 153),
        new(CommonYear, 8, 31, 122),
        new(CommonYear, 9, 30, 92),
        new(CommonYear, 10, 31, 61),
        new(CommonYear, 11, 30, 31),
        new(CommonYear, 12, 31, 0),
        // Leap year.
        new(LeapYear, 1, 31, 335),
        new(LeapYear, 2, 29, 306),
        new(LeapYear, 3, 31, 275),
        new(LeapYear, 4, 30, 245),
        new(LeapYear, 5, 31, 214),
        new(LeapYear, 6, 30, 184),
        new(LeapYear, 7, 31, 153),
        new(LeapYear, 8, 31, 122),
        new(LeapYear, 9, 30, 92),
        new(LeapYear, 10, 31, 61),
        new(LeapYear, 11, 30, 31),
        new(LeapYear, 12, 31, 0),
    };

    public TheoryData<YemodaAnd<int>> DaysInMonthAfterDateData { get; } = new()
    {
        new(CommonYear, 1, 1, 30),
        new(CommonYear, 1, 31, 0),
        new(CommonYear, 2, 1, 27),
        new(CommonYear, 2, 28, 0),
        new(LeapYear, 2, 1, 28),
        new(LeapYear, 2, 28, 1),
        new(LeapYear, 2, 29, 0),
        new(CommonYear, 3, 1, 30),
        new(CommonYear, 3, 31, 0),
        new(CommonYear, 4, 1, 29),
        new(CommonYear, 4, 30, 0),
        new(CommonYear, 5, 1, 30),
        new(CommonYear, 5, 31, 0),
        new(CommonYear, 6, 1, 29),
        new(CommonYear, 6, 30, 0),
        new(CommonYear, 7, 1, 30),
        new(CommonYear, 7, 31, 0),
        new(CommonYear, 8, 1, 30),
        new(CommonYear, 8, 31, 0),
        new(CommonYear, 9, 1, 29),
        new(CommonYear, 9, 30, 0),
        new(CommonYear, 10, 1, 30),
        new(CommonYear, 10, 31, 0),
        new(CommonYear, 11, 1, 29),
        new(CommonYear, 11, 30, 0),
        new(CommonYear, 12, 1, 30),
        new(CommonYear, 12, 31, 0),
    };
}

//// Min/Max.
//public partial class GregorianData
//{
//    public static TheoryData<Yemoda, Yemoda, bool, bool> MinMax => s_MinMax.ToTheoryDataOf2Yemoda();

//    private static readonly List<(int left, int, int, int right, int, int, bool leftIsMax, bool areEqual)> s_MinMax = new()
//    {
//        // Identical dates.
//        (left: 3, 4, 5, right: 3, 4, 5, leftIsMax: true, areEqual: true),
//        // One day before and after.
//        (left: 3, 4, 5, right: 3, 4, 4, leftIsMax: true, areEqual: false),
//        (left: 3, 4, 5, right: 3, 4, 6, leftIsMax: false, areEqual: false),
//        // One month before and after.
//        (left: 3, 4, 5, right: 3, 3, 5, leftIsMax: true, areEqual: false),
//        (left: 3, 4, 5, right: 3, 5, 5, leftIsMax: false, areEqual: false),
//        // One year before and after.
//        (left: 3, 4, 5, right: 2, 4, 5, leftIsMax: true, areEqual: false),
//        (left: 3, 4, 5, right: 5, 4, 5, leftIsMax: false, areEqual: false),
//        // Right is before even if month and day are "after".
//        (left: 3, 4, 5, right: 1, 5, 6, leftIsMax: true, areEqual: false),
//        // Left is after even if month and day are "before".
//        (left: 3, 4, 5, right: 4, 1, 1, leftIsMax: false, areEqual: false),
//    };
//}

public partial class GregorianDataSet // IYearAdjustmentDataSet
{
    public TheoryData<YemodaAnd<int>> InvalidYearAdjustementData { get; } = new()
    {
        // Intercalary day mapped to a common year.
        new(LeapYear, 2, 29, CommonYear)
    };

    public TheoryData<YemodaAnd<int>> YearAdjustementData { get; } = new()
    {
        // Intercalary day mapped to another leap year.
        new(4, 2, 29, 8),
    };
}
