namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Properties;
using System.Globalization;

#if NET8_0_OR_GREATER
using System.Text;
#endif


[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Usage and readability.")]
internal static class ExceptionMessages {
#if NET8_0_OR_GREATER
    private static CompositeFormat PolylineCannotBeShorterThanExceptionMessage = CompositeFormat.Parse(ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage);
#else
    private static readonly string PolylineCannotBeShorterThanExceptionMessage = ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage;
#endif

    internal static string GetPolylineCannotBeShorterThanExceptionMessage(int length, int minLength) {
        return string.Format(CultureInfo.InvariantCulture, PolylineCannotBeShorterThanExceptionMessage, length, minLength);
    }
}
