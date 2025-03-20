//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.IO.Pipelines;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.IO.Pipelines.Internal;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoder {
    public PolylineDecoder(CoordinateFormatter formatter) {
        Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    public CoordinateFormatter Formatter { get; }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    /// <exception cref="OperationCanceledException">Thrown when .</exception>
    public async Task DecodeAsync(PipeReader reader, PipeWriter writer, CancellationToken cancellation = default) {
        if (reader is null) {
            throw new ArgumentNullException(nameof(reader));
        }

        if (writer is null) {
            throw new ArgumentNullException(nameof(writer));
        }

        int latitude;
        int longitude;

        Memory<byte> buffer = new byte[6];

        PolylineCoordinate current = Coordinate.Default;

        while (true) {
            latitude = await ReadAsync(reader, buffer, cancellation)
                .ConfigureAwait(false);

            longitude = await ReadAsync(reader, buffer, cancellation)
                .ConfigureAwait(false);

            current += CoordinateVariance.Create(latitude, longitude);

            if (!await Formatter.TryWriteAsync(writer, current, cancellation).ConfigureAwait(false)) {
                throw new InvalidOperationException();
            }

            if (!reader.TryRead(out var result) || (result.IsCompleted || result.IsCanceled)) {
                break;
            }
        }

        await CompleteAsync(reader, writer)
            .ConfigureAwait(false);

        static async Task CompleteAsync(PipeReader reader, PipeWriter writer) {
            await reader
                .CompleteAsync()
                .ConfigureAwait(false);
            await writer
                .CompleteAsync()
                .ConfigureAwait(false);
        }

        static async Task<int> ReadAsync(PipeReader reader, Memory<byte> buffer, CancellationToken cancellationToken) {
            var result = await reader
                .ReadAtLeastAsync(buffer.Length, cancellationToken)
                .ConfigureAwait(false);

            result.Buffer
                .CopyTo(buffer.Span);

            int consumed = VarianceEncoding.Default
                .Decode(buffer.Span, out int value);

            var position = result.Buffer.GetPosition(consumed);

            reader.AdvanceTo(position);

            return value;
        }
    }

    //internal static int Decode(ref int value, ref readonly Span<byte> buffer) {
    //    int position = 0;
    //    int chunk = 0;
    //    int sum = 0;
    //    int shifter = 0;

    //    while (buffer.Length < position) {
    //        chunk = value - Defaults.Algorithm.QuestionMark;
    //        sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
    //        shifter += Defaults.Algorithm.ShiftLength;

    //        if (chunk < Defaults.Algorithm.Space) {
    //            break;
    //        }
    //    }

    //    if (buffer.Length == position && chunk >= Defaults.Algorithm.Space) {
    //        //InvalidPolylineException.Throw(reader.Length - reader.Remaining);
    //    }

    //    value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

    //    return position;
    //}
}