//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.IO.Pipelines;

using PolylineAlgorithm.IO.Pipelines.Internal;
using System.Buffers;
using System.IO.Pipelines;
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

        ReadResult result;
        int latitude = 0;
        int longitude = 0;
        SequencePosition position;
        Memory<byte> temp = new byte[6];
        Memory<char> buffer = new char[6];

        while (true) {
            result = await reader
                .ReadAtLeastAsync(6, cancellation)
                .ConfigureAwait(false);

            position = Process(result, ref latitude, ref temp, ref buffer);
            reader.AdvanceTo(position);

            result = await reader
                .ReadAtLeastAsync(6, cancellation)
                .ConfigureAwait(false);

            position = Process(result, ref longitude, ref temp, ref buffer);
            reader.AdvanceTo(position);

            await Formatter
                .TryWriteAsync(writer, new Coordinate(latitude, longitude), cancellation)
                .ConfigureAwait(false);

            if (result.IsCompleted) {
                break;
            }
        }

        await CompleteAsync(reader, writer)
            .ConfigureAwait(false);

        static SequencePosition Process(ReadResult result, ref int value, ref Memory<byte> temp, ref Memory<char> buffer) {
            result.Buffer.Slice(0, 6).CopyTo(temp.Span);
            Encoding.UTF8.GetChars(temp.Span, buffer.Span);

            long consumed = PolylineEncoding.Default.GetNextValue(buffer.Span, ref value);

            return result.Buffer.GetPosition(consumed);
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

    internal static int Decode(ref readonly Span<char> buffer, ref int value) {
        int position = 0;
        int chunk = 0;
        int sum = 0;
        int shifter = 0;

        while (buffer.Length < position) {
            chunk = value - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;

            if (chunk < Defaults.Algorithm.Space) {
                break;
            }
        }

        if (buffer.Length == position && chunk >= Defaults.Algorithm.Space) {
            //InvalidPolylineException.Throw(reader.Length - reader.Remaining);
        }

        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return position;
    }
}