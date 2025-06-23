namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Properties;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// Provides an abstract base class for decoding encoded polyline strings into collections of geographic coordinates.
/// </summary>
/// <typeparam name="TCoordinate">
/// The type representing a geographic coordinate, such as <see cref="Coordinate"/>.
/// </typeparam>
public abstract class PolylineDecoder<TCoordinate> : IPolylineDecoder<TCoordinate> {
    /// <summary>
    /// Decodes an encoded polyline string into a collection of geographic coordinates.
    /// </summary>
    /// <param name="polyline">The <see cref="Polyline"/> instance representing the encoded polyline string.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TCoordinate"/> objects representing the decoded geographic coordinates.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the provided <paramref name="polyline"/> is empty.
    /// </exception>
    /// <exception cref="InvalidPolylineException">
    /// Thrown if the polyline contains malformed or invalid encoded data.
    /// </exception>
    public IEnumerable<TCoordinate> Decode(Polyline polyline) {
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int consumed = 0;
        int latitude = 0;
        int longitude = 0;

        ReadOnlySequence<char>.Enumerator enumerator = polyline.GetEnumerator();

        int position;
        ReadOnlyMemory<char> sequence;

        while (enumerator.MoveNext()) {
            position = 0;
            sequence = enumerator.Current;

            // Attempt to read latitude and longitude values from the encoded sequence.
            while (PolylineEncoding.TryReadValue(ref latitude, ref sequence, ref position)
                && PolylineEncoding.TryReadValue(ref longitude, ref sequence, ref position)
            ) {
                // Yield a coordinate created from the denormalized latitude and longitude.
                yield return CreateCoordinate(PolylineEncoding.Denormalize(latitude), PolylineEncoding.Denormalize(longitude));
            }

            consumed += position;

            // If not all characters in the sequence were consumed, the polyline is invalid.
            if (sequence.Length != position) {
                InvalidPolylineException.Throw(consumed);
            }
        }
    }

    /// <summary>
    /// Creates a coordinate instance from the specified latitude and longitude values.
    /// </summary>
    /// <param name="latitude">The latitude value, in degrees.</param>
    /// <param name="longitude">The longitude value, in degrees.</param>
    /// <returns>
    /// An instance of <typeparamref name="TCoordinate"/> representing the specified latitude and longitude.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public abstract TCoordinate CreateCoordinate(in double latitude, in double longitude);
}
