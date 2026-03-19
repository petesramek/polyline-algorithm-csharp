namespace PolylineAlgorithm.Internal.Diagnostics;

using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Properties;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if NETSTANDARD2_1 || NET5_0
using System.Runtime.CompilerServices;
using static PolylineAlgorithm.Internal.Diagnostics.ExceptionGuard;
#endif

#if NET6_0_OR_GREATER
using System.Diagnostics;
#endif

#if NET8_0_OR_GREATER
using System.Text;
#endif

internal static class ExceptionGuard {
#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowNotFiniteNumber(string paramName) {
        throw new ArgumentOutOfRangeException(paramName, ExceptionMessage.GetArgumentValueMustBeFiniteNumber());
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowArgumentNull(string paramName) {
        throw new ArgumentNullException(paramName);
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowBufferOverflow(string message) {
        throw new OverflowException(message);
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowCoordinateValueOutOfRange(double value, double min, double max, string paramName) {
        throw new ArgumentOutOfRangeException(paramName, ExceptionMessage.FormatCoordinateValueMustBeBetween(paramName, min, max));
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void StackAllocLimitMustBeEqualOrGreaterThan(int minValue, string paramName) {
        throw new ArgumentOutOfRangeException(paramName, ExceptionMessage.FormatStackAllocLimitMustBeEqualOrGreaterThan(minValue));
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrwoArgumentCannotBeEmptyEnumerationMessage(string paramName) {
        throw new ArgumentException(ExceptionMessage.GetArgumentCannotBeEmpty(), paramName);
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    internal static void ThrowCouldNotWriteEncodedValueToBuffer() {
        throw new InvalidOperationException(ExceptionMessage.GetCouldNotWriteEncodedValueToTheBuffer());
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(int destinationLength, int polylineLength, string paramName) {

        throw new ArgumentException(ExceptionMessage.FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength), paramName);
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineLength(int length, int min) {
        throw new InvalidPolylineException(ExceptionMessage.FormatInvalidPolylineLength(length, min));
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineCharacter(char character, int position) {
        throw new InvalidPolylineException(ExceptionMessage.FormatInvalidPolylineCharacter(character, position));
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowPolylineBlockTooLong(int position) {
        throw new InvalidPolylineException(ExceptionMessage.FormatPolylineBlockTooLong(position));
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineFormat(long position) {
        throw new InvalidPolylineException(ExceptionMessage.FormatMalformedPolyline(position));
    }

#if NET6_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.NoInlining)]
#endif
    [DoesNotReturn]
    internal static void ThrowInvalidPolylineBlockTerminator() {
        throw new InvalidPolylineException(ExceptionMessage.GetInvalidPolylineBlockTerminator());
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Internal use only.")]
    internal static class ExceptionMessage {
#if NET8_0_OR_GREATER
        private static readonly CompositeFormat StackAllocLimitMustBeEqualOrGreaterThanFormat = CompositeFormat.Parse(ExceptionMessageResource.StackAllocLimitMustBeEqualOrGreaterThanFormat);
        private static readonly CompositeFormat PolylineCannotBeShorterThanFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineCannotBeShorterThanFormat);
        private static readonly CompositeFormat PolylineIsMalformedAtFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineIsMalformedAtFormat);
        private static readonly CompositeFormat CoordinateValueMustBeBetweenFormat = CompositeFormat.Parse(ExceptionMessageResource.CoordinateValueMustBeBetweenValuesFormat);
        private static readonly CompositeFormat PolylineBlockTooLongFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineBlockTooLongFormat);
        private static readonly CompositeFormat InvalidPolylineCharacterFormat = CompositeFormat.Parse(ExceptionMessageResource.InvalidPolylineCharacterFormat);
        private static readonly CompositeFormat DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat = CompositeFormat.Parse(ExceptionMessageResource.DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat);
        private static readonly CompositeFormat InvalidPolylineLengthFormat = CompositeFormat.Parse(ExceptionMessageResource.InvalidPolylineLengthFormat);
#else
        private static readonly string StackAllocLimitMustBeEqualOrGreaterThanFormat = ExceptionMessageResource.StackAllocLimitMustBeEqualOrGreaterThanFormat;
        private static readonly string PolylineCannotBeShorterThanFormat = ExceptionMessageResource.PolylineCannotBeShorterThanFormat;
        private static readonly string PolylineIsMalformedAtFormat = ExceptionMessageResource.PolylineIsMalformedAtFormat;
        private static readonly string CoordinateValueMustBeBetweenFormat = ExceptionMessageResource.CoordinateValueMustBeBetweenValuesFormat;
        private static readonly string PolylineBlockTooLongFormat = ExceptionMessageResource.PolylineBlockTooLongFormat;
        private static readonly string InvalidPolylineCharacterFormat = ExceptionMessageResource.InvalidPolylineCharacterFormat;
        private static readonly string DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat = ExceptionMessageResource.DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat;
        private static readonly string InvalidPolylineLengthFormat = ExceptionMessageResource.InvalidPolylineLengthFormat;
#endif

        private static readonly string ArgumentCannotBeEmptyMessage = ExceptionMessageResource.ArgumentCannotBeEmptyMessage;
        private static readonly string CouldNotWriteEncodedValueToTheBufferMessage = ExceptionMessageResource.CouldNotWriteEncodedValueToTheBufferMessage;
        private static readonly string InvalidPolylineBlockTerminatorMessage = ExceptionMessageResource.InvalidPolylineBlockTerminatorMessage;

        internal static string FormatStackAllocLimitMustBeEqualOrGreaterThan(int minValue) =>
            string.Format(CultureInfo.InvariantCulture, StackAllocLimitMustBeEqualOrGreaterThanFormat, minValue);

        internal static string FormatPolylineCannotBeShorterThan(int length, int minLength) =>
            string.Format(CultureInfo.InvariantCulture, PolylineCannotBeShorterThanFormat, length, minLength);

        internal static string FormatMalformedPolyline(long position) =>
            string.Format(CultureInfo.InvariantCulture, PolylineIsMalformedAtFormat, position);

        internal static string FormatCoordinateValueMustBeBetween(string name, double min, double max) =>
            string.Format(CultureInfo.InvariantCulture, CoordinateValueMustBeBetweenFormat, name, min, max);

        internal static string FormatPolylineBlockTooLong(int position) =>
            string.Format(CultureInfo.InvariantCulture, PolylineBlockTooLongFormat, position);

        internal static string FormatInvalidPolylineCharacter(char character, int position) =>
            string.Format(CultureInfo.InvariantCulture, InvalidPolylineCharacterFormat, character, position);

        internal static string FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(int destinationLength, int polylineLength) =>
            string.Format(CultureInfo.InvariantCulture, DestinationArrayLengthMustBeEqualOrGreaterThanPolylineLengthFormat, destinationLength, polylineLength);

        internal static string FormatInvalidPolylineLength(int length, int min) =>
            string.Format(CultureInfo.InvariantCulture, InvalidPolylineLengthFormat, length, min);

        internal static string GetArgumentValueMustBeFiniteNumber() =>
            ArgumentCannotBeEmptyMessage;

        internal static string GetCouldNotWriteEncodedValueToTheBuffer() =>
            CouldNotWriteEncodedValueToTheBufferMessage;

        internal static string GetArgumentCannotBeEmpty() =>
            ArgumentCannotBeEmptyMessage;
        internal static string GetInvalidPolylineBlockTerminator() =>
            InvalidPolylineBlockTerminatorMessage;
    }
}