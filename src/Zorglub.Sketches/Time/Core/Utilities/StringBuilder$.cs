// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#pragma warning disable IDE0058 // Expression value is never used (Style)

namespace Zorglub.Time.Core.Utilities;

using System.Text;

/// <summary>
/// Provides extension methods for <see cref="StringBuilder"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class StringBuilderExtensions
{
    /// <summary>
    /// Appends the culture-independent string representation of an integer
    /// in the range from 0 to 99.
    /// </summary>
    public static StringBuilder AppendInt_ge0le99(this StringBuilder buf, int num)
    {
        Debug.Assert(buf != null);
        Debug.Assert(0 <= num);
        Debug.Assert(num < 100);

        unchecked
        {
            if (num < 10)
            {
                return buf.Append((char)('0' + num));
            }
            else
            {
                buf.Append((char)('0' + num / 10));
                return buf.Append((char)('0' + num % 10));
            }
        }
    }

    /// <summary>
    /// Appends the culture-independent string representation of a positive
    /// integer.
    /// </summary>
    public static StringBuilder AppendInt_ge0(this StringBuilder buf, int num)
    {
        Debug.Assert(buf != null);
        Debug.Assert(num >= 0);

        unchecked
        {
            // On utilise cette méthode principalement avec des années ou
            // des jours d'une année. On arrange donc le code pour ces cas
            // de figure bien précis. On émet l'hypothèse que le cas le plus
            // courant sera celui de 1000 <= num < 10.000, ce qui joue
            // clairement en défaveur des jours d'une année.
            if (num < 10_000)
            {
                if (num >= 1000)
                {
                    buf.Append((char)('0' + num / 1000));
                    buf.Append((char)('0' + num / 100 % 10));
                    buf.Append((char)('0' + num / 10 % 10));
                    return buf.Append((char)('0' + num % 10));
                }

                if (num >= 100)
                {
                    buf.Append((char)('0' + num / 100));
                    buf.Append((char)('0' + num / 10 % 10));
                    return buf.Append((char)('0' + num % 10));
                }

                if (num >= 10)
                {
                    buf.Append((char)('0' + num / 10));
                    return buf.Append((char)('0' + num % 10));
                }

                return buf.Append((char)('0' + num));
            }
        }

        return AppendTen1000OrMore(buf, num);
    }

    /// <summary>
    /// Appends the string of length equal to 2 obtained by eventually
    /// left-padding with zeros the culture-independent representation of
    /// the specified integer in the range from 0 to 99.
    /// </summary>
    public static StringBuilder PadLeft_ff(this StringBuilder buf, int num)
    {
        Debug.Assert(buf != null);
        Debug.Assert(0 <= num);
        Debug.Assert(num < 100);

        unchecked
        {
            buf.Append((char)('0' + num / 10));
            return buf.Append((char)('0' + num % 10));
        }
    }

    /// <summary>
    /// Appends the string of length equal to 3 obtained by eventually
    /// left-padding with zeros the culture-independent representation of
    /// the specified integer in the range from 0 to 999.
    /// </summary>
    public static StringBuilder PadLeft_fff(this StringBuilder buf, int num)
    {
        Debug.Assert(buf != null);
        Debug.Assert(0 <= num);
        Debug.Assert(num < 1000);

        unchecked
        {
            buf.Append((char)('0' + num / 100));
            buf.Append((char)('0' + num / 10 % 10));
            return buf.Append((char)('0' + num % 10));
        }
    }

    /// <summary>
    /// Appends the string of length equal to 5 obtained by eventually
    /// left-padding with zeros the culture-independent representation of
    /// the absolute value of the specified integer in the range from -9999
    /// to 9999, and prefixed by its sign.
    /// </summary>
    public static StringBuilder PadLeft_Sffff(this StringBuilder buf, int year)
    {
        Debug.Assert(buf != null);
        Debug.Assert(-9999 <= year);
        Debug.Assert(year <= 9999);

        unchecked
        {
            if (year >= 0)
            {
                buf.Append('+');
            }
            else
            {
                buf.Append('-');
                year = -year;
            }

            buf.Append((char)('0' + year / 1000));
            buf.Append((char)('0' + year / 100 % 10));
            buf.Append((char)('0' + year / 10 % 10));
            return buf.Append((char)('0' + year % 10));
        }
    }

    /// <summary>
    /// Appends the culture-independent string representation of an integer
    /// greater than or equal to 10,000.
    /// </summary>
    private static StringBuilder AppendTen1000OrMore(StringBuilder buf, int num)
    {
        Debug.Assert(num >= 10_000);

        // Version 0.
        // Pas la plus rapide mais triviale.
        // > num.ToString(CultureInfo.InvariantCulture);
        //
        // Version 1 (avec allocation).
        // Rapide mais allocation d'un tableau de char's.
        // > int len = MathEx.AdjustedLog10(num, 10_000, 5);
        // > char[] arr = new char[len];
        // > for (int i = len - 1; i >= 0; i--)
        // > {
        // >     arr[i] = (char)('0' + num % 10);
        // >     num /= 10;
        // > }
        // > return buf.Append(arr);
        //
        // Version 2 (sans allocation).
        // Plus lente que la version 0...
        // Contrairement aux autres versions, on ajoute les chiffres dans
        // l'ordre.
        // > int len = MathEx.AdjustedLog10(num, 10_000, 5, out int pow);
        // > for (int i = 0; i < len; i++)
        // > {
        // >     buf.Append((char)('0' + num / pow % 10));
        // >     pow /= 10;
        // > }
        //
        // Version 3 (Span<char>).
        // La plus rapide et sans allocation mais nécessite
        // .NET Standard 2.1 (voir CalendarKit.More).

        // Version choisie :
        // Aussi rapide que la version 3 mais alloue systématiquement
        // 32B (similaire à ToString()).
        // On sait qu'on a au moins 5 digits car n >= 10.000
        // et que 10 est la taille max atteinte qd num >= 10^9.
        // On a juste à trouver les (au plus) 5 autres digits.
        unchecked
        {
            byte[] rev = new byte[5];
            int n = 0;
            int tmp = num / 10_000;
            while (tmp > 9)
            {
                tmp /= 10;
                n++;
                rev[n - 1] = (byte)(tmp % 10);
            }

            for (int i = n - 1; i >= 0; i--)
            {
                buf.Append((char)('0' + rev[i]));
            }

            // Attention, ici on doit garder le premier modulo 10 car on
            // ne sait pas si num < 100_0000.
            buf.Append((char)('0' + num / 10_000 % 10));
            buf.Append((char)('0' + num / 1000 % 10));
            buf.Append((char)('0' + num / 100 % 10));
            buf.Append((char)('0' + num / 10 % 10));
            return buf.Append((char)('0' + num % 10));
        }
    }
}

internal partial class StringBuilderExtensions // Méthodes spécifiques aux calendriers
{
    ///// <summary>
    ///// Appends the culture-independent string representation of a chronology.
    ///// </summary>
    //public static StringBuilder AppendChronology(
    //    this StringBuilder buf, Chronology chronology) =>
    //    buf.Append(FormattableString.Invariant($" ({chronology})"));

    /// <summary>
    /// Appends the culture-independent string representation of the
    /// specified algebraic year. If year is in the range from -9999 to 9999,
    /// its "absolute value" is left-padded with zeros.
    /// </summary>
    public static StringBuilder PadLeftYear_Sffffe(
        this StringBuilder buf, int year)
    {
        Debug.Assert(buf != null);
        // Normalement YearMonthDay.MinYear <= year <= year <= YearMonthDay.MaxYear
        // mais la méthode marche bien au-delà. Par contre, pour pouvoir
        // calculer abs(year), on doit obligatoirement avoir :
        Debug.Assert(year > Int32.MinValue);

        unchecked
        {
            int abs;
            if (year < 0)
            {
                buf.Append('-');
                abs = -year;
            }
            else
            {
                abs = year;
            }

            if (abs < 10_000)
            {
                // Left-padded.
                buf.Append((char)('0' + abs / 1000));
                buf.Append((char)('0' + abs / 100 % 10));
                buf.Append((char)('0' + abs / 10 % 10));
                return buf.Append((char)('0' + abs % 10));
            }
            else
            {
                return AppendTen1000OrMore(buf, abs);
            }
        }
    }

    /// <summary>
    /// Appends the culture-independent string representation of the year of
    /// the era for the specified algebraic year.
    /// </summary>
    public static StringBuilder AppendYearOfEra(this StringBuilder buf, int year)
    {
        Debug.Assert(buf != null);
        Debug.Assert(year > 1 + Int32.MinValue);

        return year > 0 ? buf.AppendInt_ge0(year)
            : buf.AppendInt_ge0(1 - year).Append(" BCE");
    }

    /// <summary>
    /// Appends the culture-independent string representation of the year of
    /// the era for the specified algebraic year. If the year of the era is
    /// lower than 9999, its "value" is left-padded with zeros.
    /// </summary>
    public static StringBuilder PadLeftYearOfEra_ffffe(
        this StringBuilder buf, int year)
    {
        Debug.Assert(buf != null);
        Debug.Assert(year > 1 + Int32.MinValue);

        int yearOfEra = toYearOfEra(year);
        unchecked
        {
            if (yearOfEra < 10_000)
            {
                // Left-padded.
                buf.Append((char)('0' + yearOfEra / 1000));
                buf.Append((char)('0' + yearOfEra / 100 % 10));
                buf.Append((char)('0' + yearOfEra / 10 % 10));
                buf.Append((char)('0' + yearOfEra % 10));
            }
            else
            {
                AppendTen1000OrMore(buf, yearOfEra);
            }

            if (year <= 0)
            {
                buf.Append(" BCE");
            }

            return buf;
        }

        static int toYearOfEra(int year) => year > 0 ? year : checked(1 - year);
    }

    /// <summary>
    /// Appends the culture-independent string representation of the
    /// specified day of the year. If dayOfYear is in the range from 1 to
    /// 999, its value is left-padded with zeros.
    /// </summary>
    public static StringBuilder PadLeftDayOfYear_fffe(
        this StringBuilder buf, int dayOfYear)
    {
        Debug.Assert(buf != null);
        Debug.Assert(Yedoy.MinDayOfYear <= dayOfYear);
        Debug.Assert(dayOfYear <= Yedoy.MaxDayOfYear);

        unchecked
        {
            if (dayOfYear < 1000)
            {
                // Left-padded.
                buf.Append((char)('0' + dayOfYear / 100));
                buf.Append((char)('0' + dayOfYear / 10 % 10));
                return buf.Append((char)('0' + dayOfYear % 10));
            }
            else
            {
                buf.Append((char)('0' + dayOfYear / 1000));
                buf.Append((char)('0' + dayOfYear / 100 % 10));
                buf.Append((char)('0' + dayOfYear / 10 % 10));
                return buf.Append((char)('0' + dayOfYear % 10));
            }
        }
    }
}

internal partial class StringBuilderExtensions // Chiffres romains
{
    private static readonly string[] s_RomanOnes =
        { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
    private static readonly string[] s_RomanTens =
        { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
    private static readonly string[] s_RomanHundreds =
        { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
    private static readonly string[] s_RomanThousands =
        { "", "M", "MM", "MMM", "MMMM" };

    // Si num >= 4000, on utilise la représentation décimale.
    public static StringBuilder AppendRomanNumeral(this StringBuilder buf, int num)
    {
        Debug.Assert(buf != null);
        Debug.Assert(num > 0);

        if (num < 4000)
        {
            AppendRomanUnit(buf, num / 1000, 'M', '�', '�');
            AppendRomanUnit(buf, num / 100 % 10, 'C', 'D', 'M');
            AppendRomanUnit(buf, num / 10 % 10, 'X', 'L', 'C');
            AppendRomanUnit(buf, num % 10, 'I', 'V', 'X');
            return buf;
        }

        // On utilise la représentation décimale.
        if (num < 10_000)
        {
            buf.Append((char)('0' + num / 1000));
            buf.Append((char)('0' + num / 100 % 10));
            buf.Append((char)('0' + num / 10 % 10));
            return buf.Append((char)('0' + num % 10));
        }

        return AppendTen1000OrMore(buf, num);
    }

    // Si num >= 5000, on utilise la représentation décimale.
    public static StringBuilder AppendRomanNumeralExt(this StringBuilder buf, int num)
    {
        Debug.Assert(buf != null);
        Debug.Assert(num > 0);

        unchecked
        {
            if (num < 10)
            {
                return buf.Append(s_RomanOnes[num]);
            }

            if (num < 100)
            {
                buf.Append(s_RomanTens[num / 10]);
                return buf.Append(s_RomanOnes[num % 10]);
            }

            if (num < 1000)
            {
                buf.Append(s_RomanHundreds[num / 100]);
                buf.Append(s_RomanTens[num / 10 % 10]);
                return buf.Append(s_RomanOnes[num % 10]);
            }

            if (num < 5000)
            {
                buf.Append(s_RomanThousands[num / 1000]);
                buf.Append(s_RomanHundreds[num / 100 % 10]);
                buf.Append(s_RomanTens[num / 10 % 10]);
                return buf.Append(s_RomanOnes[num % 10]);
            }

            if (num < 10_000)
            {
                // On utilise la représentation décimale.
                buf.Append((char)('0' + num / 1000));
                buf.Append((char)('0' + num / 100 % 10));
                buf.Append((char)('0' + num / 10 % 10));
                return buf.Append((char)('0' + num % 10));
            }
        }

        return AppendTen1000OrMore(buf, num);
    }

#pragma warning disable IDE1006 // Naming Styles
    private static void AppendRomanUnit(StringBuilder buf, int num, char I, char V, char X)
    {
        Debug.Assert(0 <= num && num <= 9);

        if (num == 0) return;

        if (num == 4)
        {
            buf.Append(I).Append(V);
        }
        else if (num == 9)
        {
            buf.Append(I).Append(X);
        }
        else
        {
            // num = 1, 2, 3, 5, 6, 7.

            if (num > 4) { buf.Append(V); }

            // On ajoute un à trois I's.
            for (int i = num % 5; i > 0; i--)
            {
                buf.Append(I);
            }
        }
    }
#pragma warning restore IDE1006
}
