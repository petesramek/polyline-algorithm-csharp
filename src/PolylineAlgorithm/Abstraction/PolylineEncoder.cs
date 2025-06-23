namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides an abstract base for encoding a collection of geographic coordinates into a polyline string.
/// This class implements the <see cref="IPolylineEncoder{TCoordinate}"/> interface and defines the core logic
/// for encoding coordinates using the polyline algorithm. Derived classes must implement methods to extract
/// latitude and longitude values from the coordinate type <typeparamref name="TCoordinate"/>.
/// </summary>
/// <typeparam name="TCoordinate">
/// The type representing a geographic coordinate, such as a struct or class containing latitude and longitude values.
/// </typeparam>
public abstract class PolylineEncoder<TCoordinate> : IPolylineEncoder<TCoordinate> {
    private const int MaxByteSize = 64_000;
    private const int MaxChars = MaxByteSize / sizeof(char);
    private const int MaxCount = MaxChars / Defaults.Polyline.MaxEncodedCoordinateLength;

    /// <summary>
    /// Encodes a set of geographic coordinates into a polyline string.
    /// </summary>
    /// <param name="coordinates">The collection of <typeparamref name="TCoordinate"/> objects to encode.</param>
    /// <returns>
    /// A <see cref="Polyline"/> representing the encoded coordinates. 
    /// Returns <see langword="default"/> if the input collection is empty or null.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="coordinates"/> argument is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="coordinates"/> argument is an empty enumeration.
    /// </exception>
    public Polyline Encode(IEnumerable<TCoordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(coordinates);

        if (count == 0) {
            return default;
        }

        CoordinateVariance variance = new();

        int position = 0;
        int consumed = 0;
        int length = GetMaxLength(count);
        bool asMultiSegment = count == -1 || count > MaxCount;
        PolylineBuilder builder = new();
        Span<char> buffer = stackalloc char[length];

        using var enumerator = coordinates.GetEnumerator();

        while (enumerator.MoveNext()) {
            variance
                .Next(PolylineEncoding.Normalize(GetLatitude(enumerator.Current)), PolylineEncoding.Normalize(GetLongitude(enumerator.Current)));

            if (asMultiSegment
                && GetRemainingBufferSize(position, buffer.Length) < GetRequiredLength(variance)) {
                builder
                    .Append(buffer[..position].ToString().AsMemory());

                position = 0;
            }

            if (!PolylineEncoding.TryWriteValue(variance.Latitude, ref buffer, ref position)
                || !PolylineEncoding.TryWriteValue(variance.Longitude, ref buffer, ref position)
            ) {
                throw new InvalidOperationException();
            }

            consumed++;
        }

        if (consumed == 0) {
            return default;
        }

        builder
            .Append(buffer[..position].ToString().AsMemory());

        return builder.Build();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetMaxLength(in int count) => count switch {
            1 => 12,
            > 1 and < MaxCount => count * Defaults.Polyline.MaxEncodedCoordinateLength,
            _ => MaxChars
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetCount(IEnumerable<TCoordinate> coordinates) => coordinates switch {
            ICollection<TCoordinate> collection => collection.Count,
            IEnumerable<TCoordinate> enumerable => enumerable.Count(),
            _ => -1,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRequiredLength(in CoordinateVariance variance) =>
            PolylineEncoding.GetCharCount(variance.Latitude) + PolylineEncoding.GetCharCount(variance.Longitude);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRemainingBufferSize(in int position, in int length) => length - position;
    }

    /// <summary>
    /// Gets the latitude value from the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate from which to extract the latitude.</param>
    /// <returns>The latitude value as a <see cref="double"/>.</returns>
    public abstract double GetLatitude(in TCoordinate coordinate);

    /// <summary>
    /// Gets the longitude value from the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate from which to extract the longitude.</param>
    /// <returns>The longitude value as a <see cref="double"/>.</returns>
    public abstract double GetLongitude(in TCoordinate coordinate);
}
