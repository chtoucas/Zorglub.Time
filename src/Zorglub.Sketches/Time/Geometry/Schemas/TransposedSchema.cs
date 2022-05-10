// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.Time.Geometry.Schemas
{
    using Zorglub.Time.Geometry.Forms;

    public sealed class TransposedSchema : IGeometricSchema
    {
        private readonly IGeometricSchema _schema;
        private readonly IMonthRegularizer _regularizer;

        public TransposedSchema(IGeometricSchema schema!!, IMonthRegularizer regularizer!!)
        {
            _schema = schema;
            _regularizer = regularizer;
        }

        /// <inheritdoc />
        [Pure]
        public int CountDaysSinceEpoch(int y, int m, int d)
        {
            _regularizer.Regularize(ref y, ref m);

            return _schema.CountDaysSinceEpoch(y, m, d);
        }

        /// <inheritdoc />
        public void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
        {
            _schema.GetDateParts(daysSinceEpoch, out y, out m, out d);

            _regularizer.Deregularize(ref y, ref m);
        }
    }
}
