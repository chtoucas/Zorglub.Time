// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Time.Geometry.Forms;

/// <summary>
/// Numérotation spéciale des mois.
/// <para><i>Intercalary days positioned at the end of a leap year.</i></para>
/// <para>Par ex., pour le calendrier grégorien cela donne :
/// mars 3, avril 4, ..., décembre 12, janvier 13, février 14.</para>
/// </summary>
public sealed record TroeschMonthForm(int A, int B, int Remainder, int ExceptionalMonth)
    : MonthForm(A, B, Remainder, MonthFormNumbering.Other);
