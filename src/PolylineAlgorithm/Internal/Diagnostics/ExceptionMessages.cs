namespace PolylineAlgorithm.Internal.Diagnostics;

using PolylineAlgorithm.Properties;
using System.Globalization;
#if NET8_0_OR_GREATER
using System.Text;
#endif

internal static class ExceptionMessages
{
#if NET8_0_OR_GREATER
    private static readonly CompositeFormat StackAllocLimitFormat = CompositeFormat.Parse(ExceptionMessageResource.StackAllocLimitMustBeEqualOrGreaterThanMessageFormat);
    private static readonly CompositeFormat PolylineCannotBeShorterThanFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage);
    private static readonly CompositeFormat PolylineStringIsMalformedFormat = CompositeFormat.Parse(ExceptionMessageResource.PolylineStringIsMalformedMessage);
    private static readonly CompositeFormat CoordinateValueMustBeBetweenFormat = CompositeFormat.Parse(ExceptionMessageResource.CoordinateValueMustBeBetweenValuesMessageFormat);
#else
    private static readonly string StackAllocLimitFormat = ExceptionMessageResource.StackAllocLimitMustBeEqualOrGreaterThanMessageFormat;
    private static readonly string PolylineCannotBeShorterThanFormat = ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage;
    private static readonly string PolylineStringIsMalformedFormat = ExceptionMessageResource.PolylineStringIsMalformedMessage;
    private static readonly string CoordinateValueMustBeBetweenFormat = ExceptionMessageResource.CoordinateValueMustBeBetweenValuesMessageFormat;
#endif

    internal static string FormatStackAllocLimit(int minValue) =>
        string.Format(CultureInfo.InvariantCulture, StackAllocLimitFormat, minValue);

    internal static string FormatPolylineCannotBeShorterThan(int length, int minLength) =>
        string.Format(CultureInfo.InvariantCulture, PolylineCannotBeShorterThanFormat, length, minLength);

    internal static string FormatPolylineStringIsMalformed(long position) =>
        string.Format(CultureInfo.InvariantCulture, PolylineStringIsMalformedFormat, position);

    internal static string FormatCoordinateValueMustBeBetween(string name, double min, double max) =>
        string.Format(CultureInfo.InvariantCulture, CoordinateValueMustBeBetweenFormat, name, min, max);

    internal static string ArgumentValueMustBeFiniteNumber =>
        ExceptionMessageResource.ArgumentValueMustBeFiniteNumber;
}
