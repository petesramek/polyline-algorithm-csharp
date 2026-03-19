namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Properties;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if NET8_0_OR_GREATER
using System.Text;
using static PolylineAlgorithm.Internal.Defaults.Polyline.Block;
#endif


[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Usage and readability.")]
internal static class ExceptionMessages {
#if NET8_0_OR_GREATER
    private static readonly CompositeFormat StackAllocLimitMustBeEqualOrGreaterThanMessageFormat = CompositeFormat.Parse(ExceptionMessageResource.StackAllocLimitMustBeEqualOrGreaterThanMessageFormat);
    private static readonly CompositeFormat PolylineCannotBeShorterThanExceptionMessage = CompositeFormat.Parse(ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage);
#else
    private static readonly string StackAllocLimitMustBeEqualOrGreaterThanMessageFormat = ExceptionMessageResource.StackAllocLimitMustBeEqualOrGreaterThanMessageFormat;
    private static readonly string PolylineCannotBeShorterThanExceptionMessage = ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage;
#endif

    internal static string GetPolylineCannotBeShorterThanExceptionMessage(int length, int minLength) {
        return string.Format(CultureInfo.InvariantCulture, PolylineCannotBeShorterThanExceptionMessage, length, minLength);
    }

    internal static string GetPolylineCannotBeShorterThanExceptionMessage(int minStackAllocLimit) {
        return string.Format(CultureInfo.InvariantCulture, StackAllocLimitMustBeEqualOrGreaterThanMessageFormat, minStackAllocLimit);
    }
}
