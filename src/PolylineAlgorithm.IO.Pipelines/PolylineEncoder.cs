//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.IO.Pipelines;

using PolylineAlgorithm.Internal;
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

        CoordinateDifference diff = new();

        while (await Formatter.TryReadAsync(reader, out Coordinate coordinate, cancellation).ConfigureAwait(false)) {
            diff.Next(coordinate);

            Process(writer, diff.Latitude, _buffer);
            Process(writer, diff.Longitude, _buffer);
        }

        await CompleteAsync(reader, writer).ConfigureAwait(false);

        static void Process(PipeWriter writer, int value, Memory<char> buffer) {
            Span<char> temp = buffer.Span;

            int length = PolylineEncoding.Default.GetChars(value, ref temp);

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