//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoder<TCoordinate> : IPolylineDecoder<TCoordinate> {
    public PolylineDecoder(ICoordinateFactory<TCoordinate> factory) {
        Factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public ICoordinateFactory<TCoordinate> Factory { get; }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<TCoordinate> Decode(Polyline polyline) {
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int chunk;
        int sum;
        int shifter;
        int position;
        int latitude = 0;
        int longitude = 0;
        ReadOnlySequence<byte>.Enumerator enumerator = polyline.GetEnumerator();

        while (enumerator.MoveNext()) {
            position = 0;

            while(TryGetValue(ref latitude) && TryGetValue(ref longitude)) {
                yield return Factory.Create(Denormalize(latitude), Denormalize(longitude));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool TryGetValue(ref int value) {
                if(position == enumerator.Current.Length) {
                    return false;
                }

                value += Decode();

                return chunk < Defaults.Algorithm.Space;

                int Decode() {
                    chunk = 0;
                    sum = 0;
                    shifter = 0;

                    while (position < enumerator.Current.Length) {
                        chunk = enumerator.Current.Span[position++] - Defaults.Algorithm.QuestionMark;
                        sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
                        shifter += Defaults.Algorithm.ShiftLength;

                        if (chunk < Defaults.Algorithm.Space) {
                            break;
                        }
                    }

                    return (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static double Denormalize(int value) => value / Defaults.Algorithm.Precision;

            //[MethodImpl(MethodImplOptions.AggressiveInlining)]
            //static int Normalize(double value) => (int)(value * Defaults.Algorithm.Precision);
        }
    }
}
