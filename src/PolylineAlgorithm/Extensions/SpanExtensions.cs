using System;
using System.Collections.Generic;
using System.Text;

namespace PolylineAlgorithm.Extensions
{
    internal static class SpanExtensions
    {
        public static int Write(this ref Span<char> span, ref readonly ReadOnlyMemory<char> values, ref int index) {
            foreach (var value in values.Span) {
                span[index++] = value;
            }

            return index;
        }
    }
}
