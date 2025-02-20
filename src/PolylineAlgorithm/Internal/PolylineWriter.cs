//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Provides functionality to write and encode a polyline.
/// </summary>
[StructLayout(LayoutKind.Auto)]
internal ref struct PolylineWriter {
    private WriterState _state = new();
    private Memory<char> _buffer;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineWriter"/> struct with the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer to write to.</param>
    public PolylineWriter(ref readonly Memory<char> buffer) {
        _buffer = buffer;
    }

    /// <summary>
    /// Gets a value indicating whether there is space left in the buffer to write.
    /// </summary>
    public readonly bool CanWrite => _state.Position < _buffer.Length;

    /// <summary>
    /// Gets the current position of the writer.
    /// </summary>
    public readonly int Position => _state.Position;

    /// <summary>
    /// Writes the specified coordinate to the buffer.
    /// </summary>
    /// <param name="coordinate">The coordinate to write.</param>
    /// <exception cref="InvalidCoordinateException">Thrown when the coordinate is invalid.</exception>
    /// <exception cref="InvalidWriterStateException">Thrown when the writer is in an invalid state.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(ref readonly Coordinate coordinate) {
        InvalidCoordinateException.ThrowIfNotValid(coordinate);
        InvalidWriterStateException.ThrowIfCannotWrite(CanWrite, Position, _buffer.Length);

        Imprecise(coordinate.Latitude, out int latitude);
        Imprecise(coordinate.Longitude, out int longitude);

        UpdateCurrent(in latitude, in longitude, out int latDiff, out int longDiff);

        WriteNext(in latDiff);
        WriteNext(in longDiff);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Imprecise(double value, out int result) {
            result = Convert.ToInt32(value * Defaults.Algorithm.Precision);
        }
    }

    /// <summary>
    /// Writes the next value to the buffer.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <exception cref="InvalidWriterStateException">Thrown when the writer is in an invalid state.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void WriteNext(ref readonly int value) {
        int rem = value << 1;

        if (value < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            WriteChar(Convert.ToChar((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark));
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        WriteChar(Convert.ToChar(rem + Defaults.Algorithm.QuestionMark));
    }

    /// <summary>
    /// Writes a character to the buffer.
    /// </summary>
    /// <param name="value">The character to write.</param>
    /// <exception cref="InvalidWriterStateException">Thrown when the writer is in an invalid state.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void WriteChar(char value) {
        InvalidWriterStateException.ThrowIfCannotWrite(CanWrite, Position, _buffer.Length);

        _buffer.Span[Position] = value;
        _state.Position += 1;
    }

    /// <summary>
    /// Updates the current latitude and longitude values and calculates their differences.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    /// <param name="latDiff">The difference in latitude.</param>
    /// <param name="longDiff">The difference in longitude.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateCurrent(ref readonly int latitude, ref readonly int longitude, out int latDiff, out int longDiff) {
        latDiff = latitude - _state.Latitude;
        _state.Latitude = latitude;

        longDiff = longitude - _state.Longitude;
        _state.Longitude = longitude;
    }

    /// <summary>
    /// Converts the buffer to a <see cref="Polyline"/> instance.
    /// </summary>
    /// <returns>The <see cref="Polyline"/> instance.</returns>
    public readonly Polyline ToPolyline() {
        ReadOnlyMemory<char> buffer = _buffer[.._state.Position];
        var polyline = Polyline.FromMemory(buffer);
        return polyline;
    }

    /// <summary>
    /// Represents the state of the writer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    private ref struct WriterState {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriterState"/> struct.
        /// </summary>
        public WriterState() {
            Position = Latitude = Longitude = 0;
        }

        /// <summary>
        /// Gets or sets the current position of the writer.
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