//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.IO.Pipelines;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.IO.Pipelines.Internal;
using System;
using System.IO.Pipelines;
using System.Text;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineEncoder {
    private Memory<char> _buffer = new char[6];
    public PolylineEncoder(CoordinateFormatter formatter) {
        Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    public CoordinateFormatter Formatter { get; }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public async Task EncodeAsync(PipeReader reader, PipeWriter writer, CancellationToken cancellation = default) {
        if (reader is null) {
            throw new ArgumentNullException(nameof(reader));
        }

        if (writer is null) {
            throw new ArgumentNullException(nameof(writer));
        }

        CoordinateVariance variance;
        Coordinate current = Coordinate.Default;

        while (await Formatter.TryReadAsync(reader, out Coordinate next, cancellation).ConfigureAwait(false)) {
            variance = CoordinateVariance.Calculate(current, next);

            Process(writer, variance.Latitude, _buffer);
            Process(writer, variance.Longitude, _buffer);

            current = next;
        }

        await CompleteAsync(reader, writer).ConfigureAwait(false);

        static void Process(PipeWriter writer, int value, Memory<char> buffer) {
            Span<char> temp = buffer.Span;

            int length = Encode(ref temp, value);

            Encoding.UTF8.GetBytes(temp[..length], writer.GetSpan(length));

            writer.Advance(length);
        }

        static async Task CompleteAsync(PipeReader reader, PipeWriter writer) {
            await reader
                .CompleteAsync()
                .ConfigureAwait(false);
            await writer
                .CompleteAsync()
                .ConfigureAwait(false);
        }
    }

    private static int Encode(ref Span<char> buffer, int difference) {
        int index = 0;
        int rem = difference << 1;

        if (difference < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            buffer[index++] = Convert.ToChar((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        buffer[index++] = Convert.ToChar(rem + Defaults.Algorithm.QuestionMark);

        return index;
    }

    public static int GetRequiredCharCount(int difference) => difference switch {
        // DO NOT CHANGE THE ORDER. We are skipping inside exclusive ranges as those are covered by previous statements.
        >= -16 and <= +15 => 1,
        >= -512 and <= +511 => 2,
        >= -16384 and <= +16383 => 3,
        >= -524288 and <= +524287 => 4,
        >= -16777216 and <= +16777215 => 5,
        _ => 6,
    };
}