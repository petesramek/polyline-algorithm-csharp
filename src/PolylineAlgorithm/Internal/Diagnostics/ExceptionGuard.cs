namespace PolylineAlgorithm.Internal.Diagnostics;

using PolylineAlgorithm;
using PolylineAlgorithm.Properties;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if NETSTANDARD2_1 || NET5_0
using System.Runtime.CompilerServices;
#endif

#if NET6_0_OR_GREATER
using System.Diagnostics;
#endif

#if NET8_0_OR_GREATER
using System.Text;
#endif

/// <summary>
/// Centralizes exception throwing for common validation and error scenarios used across the library.
/// </summary>
/// <remarks>
/// Methods in this class are intentionally small and annotated so that they can act as single
/// call sites for throwing exceptions (improving inlining and stack traces). Many members have
/// attributes to avoid polluting callers' stack traces (__StackTraceHidden__ on supported targets)
/// or to prevent inlining on older targets.
/// </remarks>
internal static class ExceptionGuard {
    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> when a numeric argument is not a finite value.
    /// </summary>
    /// <param name="paramName">Name of the parameter that had a non-finite value.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowNotFiniteNumber(string paramName) {
        throw new ArgumentOutOfRangeException(paramName, ExceptionMessage.GetArgumentValueMustBeFiniteNumber());
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> for a null argument.
    /// </summary>
    /// <param name="paramName">Name of the parameter that was null.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowArgumentNull(string paramName) {
        throw new ArgumentNullException(paramName);
    }

    /// <summary>
    /// Throws an <see cref="OverflowException"/> with a provided message.
    /// </summary>
    /// <param name="message">Message that describes the overflow condition.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowBufferOverflow(string message) {
        throw new OverflowException(message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> when a coordinate value is outside the allowed range.
    /// </summary>
    /// <param name="value">The coordinate value that was out of range.</param>
    /// <param name="min">Inclusive minimum allowed value.</param>
    /// <param name="max">Inclusive maximum allowed value.</param>
    /// <param name="paramName">Name of the parameter containing the coordinate.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowCoordinateValueOutOfRange(double value, double min, double max, string paramName) {
        throw new ArgumentOutOfRangeException(paramName, ExceptionMessage.FormatCoordinateValueMustBeBetween(paramName, min, max));
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when an enumeration argument is empty but must contain at least one element.
    /// </summary>
    /// <param name="paramName">Name of the parameter representing the enumeration.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowArgumentCannotBeEmptyEnumerationMessage(string paramName) {
        throw new ArgumentException(ExceptionMessage.GetArgumentCannotBeEmpty(), paramName);
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> when an encoded value could not be written to the destination buffer.
    /// </summary>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowCouldNotWriteEncodedValueToBuffer() {
        throw new InvalidOperationException(ExceptionMessage.GetCouldNotWriteEncodedValueToTheBuffer());
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when a destination array is not large enough to contain the polyline data.
    /// </summary>
    /// <param name="destinationLength">The length of the destination array.</param>
    /// <param name="polylineLength">The required polyline length.</param>
    /// <param name="paramName">Name of the parameter representing the destination array.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(int destinationLength, int polylineLength, string paramName) {

        throw new ArgumentException(ExceptionMessage.FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength), paramName);
    }

    /// <summary>
    /// Throws an <see cref="InvalidPolylineException"/> when the polyline length is invalid.
    /// </summary>
    /// <param name="length">The invalid length.</param>
    /// <param name="min">The minimum required length.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineLength(int length, int min) {
        throw new InvalidPolylineException(ExceptionMessage.FormatInvalidPolylineLength(length, min));
    }

    /// <summary>
    /// Throws an <see cref="InvalidPolylineException"/> when an unexpected character is encountered in the polyline.
    /// </summary>
    /// <param name="character">The invalid character.</param>
    /// <param name="position">Position in the polyline where the character was found.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineCharacter(char character, int position) {
        throw new InvalidPolylineException(ExceptionMessage.FormatInvalidPolylineCharacter(character, position));
    }

    /// <summary>
    /// Throws an <see cref="InvalidPolylineException"/> when a polyline block is longer than allowed.
    /// </summary>
    /// <param name="position">Position in the polyline where the block exceeded the allowed length.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowPolylineBlockTooLong(int position) {
        throw new InvalidPolylineException(ExceptionMessage.FormatPolylineBlockTooLong(position));
    }

    /// <summary>
    /// Throws an <see cref="InvalidPolylineException"/> when the polyline format is malformed.
    /// </summary>
    /// <param name="position">Approximate position where the polyline became malformed.</param>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineFormat(long position) {
        throw new InvalidPolylineException(ExceptionMessage.FormatMalformedPolyline(position));
    }

    /// <summary>
    /// Throws an <see cref="InvalidPolylineException"/> when the polyline block terminator is invalid.
    /// </summary>
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineBlockTerminator() {
        throw new InvalidPolylineException(ExceptionMessage.GetInvalidPolylineBlockTerminator());
    }

    /// <summary>
    /// Helper that formats localized exception messages used by <see cref="ExceptionGuard"/>.
    /// </summary>
    /// <remarks>
    /// Keeps message formatting centralized so exception text remains consistent and can reuse
    /// resource strings. This class is internal because messages are only used by the guard methods.
    /// </remarks>
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal use only.")]
    internal static class ExceptionMessage {
#if NET8_0_OR_GREATER
        private static readonly CompositeFormat PolylineCannotBeShorterThanFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineCannotBeShorterThanFormat);
        private static readonly CompositeFormat PolylineIsMalformedAtFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineIsMalformedAtFormat);
        private static readonly CompositeFormat CoordinateValueMustBeBetweenFormat = CompositeFormat.Parse(ExceptionMessageResource.CoordinateValueMustBeBetweenValuesFormat);
        private static readonly CompositeFormat PolylineBlockTooLongFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineBlockTooLongFormat);
        private static readonly CompositeFormat InvalidPolylineCharacterFormat = CompositeFormat.Parse(ExceptionMessageResource.InvalidPolylineCharacterFormat);
        private static readonly CompositeFormat DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat = CompositeFormat.Parse(ExceptionMessageResource.DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat);
        private static readonly CompositeFormat InvalidPolylineLengthFormat = CompositeFormat.Parse(ExceptionMessageResource.InvalidPolylineLengthFormat);
#else
        private static readonly string PolylineCannotBeShorterThanFormat = ExceptionMessageResource.PolylineCannotBeShorterThanFormat;
        private static readonly string PolylineIsMalformedAtFormat = ExceptionMessageResource.PolylineIsMalformedAtFormat;
        private static readonly string CoordinateValueMustBeBetweenFormat = ExceptionMessageResource.CoordinateValueMustBeBetweenValuesFormat;
        private static readonly string PolylineBlockTooLongFormat = ExceptionMessageResource.PolylineBlockTooLongFormat;
        private static readonly string InvalidPolylineCharacterFormat = ExceptionMessageResource.InvalidPolylineCharacterFormat;
        private static readonly string DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat = ExceptionMessageResource.DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat;
        private static readonly string InvalidPolylineLengthFormat = ExceptionMessageResource.InvalidPolylineLengthFormat;
#endif

        private static readonly string ArgumentCannotBeEmptyMessage = ExceptionMessageResource.ArgumentCannotBeEmptyMessage;
        private static readonly string ArgumentValueMustBeFiniteNumberMessage = ExceptionMessageResource.ArgumentValueMustBeFiniteNumber;
        private static readonly string CouldNotWriteEncodedValueToTheBufferMessage = ExceptionMessageResource.CouldNotWriteEncodedValueToTheBufferMessage;
        private static readonly string InvalidPolylineBlockTerminatorMessage = ExceptionMessageResource.InvalidPolylineBlockTerminatorMessage;

        /// <summary>
        /// Formats the message when a polyline is shorter than the required minimum.
        /// </summary>
        internal static string FormatPolylineCannotBeShorterThan(int length, int minLength) =>
            string.Format(CultureInfo.InvariantCulture, PolylineCannotBeShorterThanFormat, length, minLength);

        /// <summary>
        /// Formats a message indicating the polyline is malformed at a given position.
        /// </summary>
        internal static string FormatMalformedPolyline(long position) =>
            string.Format(CultureInfo.InvariantCulture, PolylineIsMalformedAtFormat, position);

        /// <summary>
        /// Formats a message indicating a coordinate parameter must be within a range.
        /// </summary>
        internal static string FormatCoordinateValueMustBeBetween(string name, double min, double max) =>
            string.Format(CultureInfo.InvariantCulture, CoordinateValueMustBeBetweenFormat, name, min, max);

        /// <summary>
        /// Formats a message for polyline blocks that exceed allowed length.
        /// </summary>
        internal static string FormatPolylineBlockTooLong(int position) =>
            string.Format(CultureInfo.InvariantCulture, PolylineBlockTooLongFormat, position);

        /// <summary>
        /// Formats a message for invalid characters found in a polyline.
        /// </summary>
        internal static string FormatInvalidPolylineCharacter(char character, int position) =>
            string.Format(CultureInfo.InvariantCulture, InvalidPolylineCharacterFormat, character, position);

        /// <summary>
        /// Formats a message when a destination array is too small for the polyline.
        /// </summary>
        internal static string FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(int destinationLength, int polylineLength) =>
            string.Format(CultureInfo.InvariantCulture, DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat, destinationLength, polylineLength);

        /// <summary>
        /// Formats an invalid polyline length message.
        /// </summary>
        internal static string FormatInvalidPolylineLength(int length, int min) =>
            string.Format(CultureInfo.InvariantCulture, InvalidPolylineLengthFormat, length, min);

        /// <summary>
        /// Gets the message used when a numeric argument must be finite.
        /// </summary>
        internal static string GetArgumentValueMustBeFiniteNumber() =>
            ArgumentValueMustBeFiniteNumberMessage;

        /// <summary>
        /// Gets the message used when the library could not write an encoded value to a buffer.
        /// </summary>
        internal static string GetCouldNotWriteEncodedValueToTheBuffer() =>
            CouldNotWriteEncodedValueToTheBufferMessage;

        /// <summary>
        /// Gets the message used when an argument collection must not be empty.
        /// </summary>
        internal static string GetArgumentCannotBeEmpty() =>
            ArgumentCannotBeEmptyMessage;

        /// <summary>
        /// Gets the message used when a polyline block terminator is invalid.
        /// </summary>
        internal static string GetInvalidPolylineBlockTerminator() =>
            InvalidPolylineBlockTerminatorMessage;
    }
}