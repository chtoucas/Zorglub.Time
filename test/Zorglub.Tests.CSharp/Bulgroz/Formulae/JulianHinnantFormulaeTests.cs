// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Zorglub.Bulgroz.Formulae;

using Zorglub.Testing.Facts.Bulgroz;

public sealed class JulianHinnantFormulaeTests : ICalendricalFormulaeFacts<JulianDataSet>
{
    public JulianHinnantFormulaeTests() : base(new JulianHinnantFormulae()) { }
}
