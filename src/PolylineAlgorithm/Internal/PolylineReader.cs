//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Provides functionality to read and decode a polyline.
/// </summary>
[StructLayout(LayoutKind.Auto)]
internal ref struct PolylineReader {
    private ReaderState _state = new();
    private readonly ReadOnlySpan<char> _polyline;

    /// <summary>
    /// Creates a new instance of the <see cref="PolylineReader"/> struct with an empty polyline.
    /// </summary>
    public PolylineReader() {
        _polyline = ReadOnlySpan<char>.Empty;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PolylineReader"/> struct with the specified polyline.
    /// </summary>
    /// <param name="polyline">The polyline to read.</param>
    public PolylineReader(ref readonly Polyline polyline) {
        _polyline = polyline.Span;
    }

    /// <summary>
    /// Gets a value indicating whether there are more characters to read.
    /// </summary>
    public readonly bool CanRead => _state.Position < _polyline.Length;

    /// <summary>
    /// Gets the current position of the reader.
    /// </summary>
    public readonly int Position => _state.Position;

    /// <summary>
    /// Reads the next coordinate from the polyline.
    /// </summary>
    /// <returns>The next <see cref="Coordinate"/>.</returns>
    /// <exception cref="InvalidReaderStateException">Thrown when the reader is in an invalid state.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Coordinate Read() {
        InvalidReaderStateException.ThrowIfCannotRead(CanRead, Position, _polyline.Length);

        GetCurrent(out int currentLatitude, out int currentLongitude);

        int latitude = ReadNext(in currentLatitude);
        int longitude = ReadNext(in currentLongitude);

        SetCurrent(in latitude, in longitude);

        return new(Precise(ref latitude), Precise(ref longitude));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double Precise(ref int value) {
            return value / Defaults.Algorithm.Precision;
        }
    }

    /// <summary>
    /// Advances the reader to the next character.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Advance() {
        _state.Position += 1;
    }

    /// <summary>
    /// Sets the current latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void SetCurrent(ref readonly int latitude, ref readonly int longitude) {
        _state.Latitude = latitude;
        _state.Longitude = longitude;
    }

    /// <summary>
    /// Gets the current latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly void GetCurrent(out int latitude, out int longitude) {
        latitude = _state.Latitude;
        longitude = _state.Longitude;
    }

    /// <summary>
    /// Reads the next value from the polyline.
    /// </summary>
    /// <param name="value">The current value.</param>
    /// <returns>The next value.</returns>
    /// <exception cref="InvalidReaderStateException">Thrown when the reader is in an invalid state.</exception>
    /// <exception cref="InvalidPolylineException">Thrown when the polyline is malformed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int ReadNext(ref readonly int value) {
        int chunk;
        int sum = 0;
        int shifter = 0;

        do {
            InvalidReaderStateException.ThrowIfCannotRead(CanRead, Position, _polyline.Length);
            chunk = _polyline[Position] - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;
            Advance();
        } while (chunk >= Defaults.Algorithm.Space && CanRead);

        if (!CanRead && chunk >= Defaults.Algorithm.Space) {
            InvalidPolylineException.Throw(Position);
        }

        return value + ((sum & 1) == 1 ? ~(sum >> 1) : sum >> 1);
    }

    /// <summary>
    /// Represents the state of the reader.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    private ref struct ReaderState {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderState"/> struct.
        /// </summary>
        public ReaderState() {
            Position = Latitude = Longitude = 0;
        }

        /// <summary>
        /// Gets or sets the current position of the reader.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the current latitude value.
        /// </summary>
        public int Latitude { get; set; }

        /// <summary>
        /// Gets or sets the current longitude value.
        /// </summary>
        public int Longitude { get; set; }
    }
}
