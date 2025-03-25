//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Runtime.CompilerServices;

public interface IPolylineDecoder {
    #region Default method implementations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryReadValue(ref int value, ref ReadOnlyMemory<char> buffer, ref int position) {
        if(position == buffer.Length) {
            return false;
        }
        
        int chunk = 0;
        int sum = 0;
        int shifter = 0;

        while (position < buffer.Length) {
            chunk = buffer.Span[position++] - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;

            if (chunk < Defaults.Algorithm.Space) {
                break;
            }
        }

        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return chunk < Defaults.Algorithm.Space;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Denormalize(int value) => value / Defaults.Algorithm.Precision;

    #endregion
}