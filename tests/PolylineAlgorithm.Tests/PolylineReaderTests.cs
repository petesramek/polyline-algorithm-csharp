namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Defines tests for the <see cref="PolylineReader"/> type.
/// </summary>
[TestClass]
public class PolylineReaderTests {
    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>.</remarks>
    [TestMethod]
    public void Decode_Valid_Input_Ok() {
        // Arrange
        PolylineReader reader = new(Values.Polyline.Valid);
        List<Coordinate> coordinates = new List<Coordinate>();

        // Act
        while (reader.Read()) {
            coordinates.Add(new(reader.Latitude, reader.Longitude));
        }

        // Assert
        CollectionAssert.AreEqual(Values.Coordinates.Valid.ToArray(), coordinates);
    }
}
