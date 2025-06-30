namespace PolylineAlgorithm.Abstraction.Tests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[TestClass]
public class PolylineEncoderTest {
    internal static readonly FakeLoggerProvider loggerProvider = new();

    [TestMethod]
    public void TestEncodeEmptyCollection() {
        var encoder = new PolylineEncoder(new() { LoggerFactory = new FakeLoggerFactory(loggerProvider) });
       void Encode() => encoder.Encode(Array.Empty<(double Latitude, double Longitude)>());
        var exception = Assert.ThrowsExactly<ArgumentException>(Encode, "The input collection cannot be empty.");
        ///loggerProvider.Collector.LatestRecord,(exception, LogLevel.Error, "Argument cannot be empty enumeration");
    }

    public class PolylineEncoder : PolylineEncoder<(double Latitude, double Longitude), string> {
        public PolylineEncoder(PolylineEncodingOptions options) : base(options) { }

        protected override string CreatePolyline(ReadOnlyMemory<char> polyline) => polyline.ToString();
        protected override double GetLatitude((double Latitude, double Longitude) coordinate) => coordinate.Latitude;
        protected override double GetLongitude((double Latitude, double Longitude) coordinate) => coordinate.Longitude;
    }
}
