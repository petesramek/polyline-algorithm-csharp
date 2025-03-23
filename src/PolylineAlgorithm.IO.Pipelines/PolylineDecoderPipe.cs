//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.IO.Pipelines;

using PolylineAlgorithm.Internal;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoderPipe {
    public PolylineDecoderPipe(CoordinateFormatter formatter) {
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

        int latitude = 0;
        int longitude = 0;

        Memory<char> buffer = new char[6];

        while (true) {
            latitude += await ReadAsync(reader, buffer, cancellation)
                .ConfigureAwait(false);

            longitude += await ReadAsync(reader, buffer, cancellation)
                .ConfigureAwait(false);



            var current = new Coordinate(latitude / Defaults.Algorithm.Precision, longitude / Defaults.Algorithm.Precision);

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

        static async Task<int> ReadAsync(PipeReader reader, Memory<char> buffer, CancellationToken cancellationToken) {
            var result = await reader
                .ReadAtLeastAsync(buffer.Length, cancellationToken)
                .ConfigureAwait(false);

            Span<byte> bytes = stackalloc byte[buffer.Length];

            result.Buffer.CopyTo(bytes);

            Encoding.UTF8.GetChars(bytes, buffer.Span);

            ReadOnlyMemory<char> temp = buffer;

            int consumed = VarianceEncoding.Default
                .Decode(temp, out int value);

            var position = result.Buffer.GetPosition(consumed);

            reader.AdvanceTo(position);

            return value;
        }
    }
}